using System.Text.Json.Serialization;
using Dapper;
using GambitoServer.GambitoContext;
using Microsoft.EntityFrameworkCore;
using Npgsql;

// [module:DapperAot]

// var builder = WebApplication.CreateSlimBuilder(args);
var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options =>
{
  options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);
});

// Register database
var ds = NpgsqlDataSource.Create($"Host=localhost:15432;Username=postgres;Password=postgres;Database=gambito");

builder.Services.AddDbContext<GambitoContext>(options => options.UseNpgsql(
    "Host=localhost:15432;Username=postgres;Password=postgres;Database=gambito",
    o => o.MapEnum<TipoHora>("tipo_hora")));

builder.Services.AddSingleton(ds);

// builder.Services.AddControllers();
builder.Services.AddProblemDetails();

// Localization
builder.Services.AddLocalization();

// Logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddResponseCompression();

var app = builder.Build();
await using var scope = app.Services.CreateAsyncScope();

app.UseStatusCodePages();

app.UseResponseCompression();

var supportedCultures = new[] { "pt-BR", "es-419", "en-US", };
var localizationOptions = new RequestLocalizationOptions()
    .SetDefaultCulture(supportedCultures[0])
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);
app.UseRequestLocalization();

app.UseAntiforgery();

if (!app.Environment.IsDevelopment())
{
  app.UseExceptionHandler("/Error", createScopeForErrors: true);
  // app.UseExceptionHandler(exceptionHandlerApp
  //     => exceptionHandlerApp.Run(async context
  //       => await Results.Problem()
  //       .ExecuteAsync(context)));
  app.UseHsts();
  app.UseResponseCompression();
}

app.MapStaticAssets();

var router = app.MapGroup("/api");

// app.MapControllers();

LinhaProducaoMinimalController.Register(router);

app.Run();

public class LinhaProducao
{
  public required int Id { get; set; }
  public required string Descricao { get; set; }
}

public static class LinhaProducaoMinimalController
{
  public static void Register(RouteGroupBuilder router)
  {
    var group = router.MapGroup("/linha-producao-m");

    group.MapGet("/", async (NpgsqlDataSource dataSource, HttpContext context) =>
    {
      await using var db = await dataSource.OpenConnectionAsync();
      var res = await db.QueryAsync<LinhaProducao>("SELECT * FROM linha_producao");
      return res.ToArray();
    });

    group.MapGet("/{id}", async (int id, NpgsqlDataSource dataSource) =>
    {
      await using var db = await dataSource.OpenConnectionAsync();
      var res = await db.QueryAsync<LinhaProducao>("SELECT * FROM linha_producao WHERE id = @id", new { id });
      try
      {
        return Results.Ok(res.First());
      }
      catch (Exception)
      {
        return Results.NotFound();
      }
    });
  }
}

// [ApiController]
// [Route("api/linha-producao")]
// public class LinhaProducaoController : ControllerBase
// {
//   [HttpGet("")]
//   public async Task<List<LinhaProducao>> GetAll(NpgsqlDataSource dataSource)
//   {
//     await using var db = await dataSource.OpenConnectionAsync();
//     var res = await db.QueryAsync<LinhaProducao>("SELECT * FROM linha_producao");
//     return res.ToList();
//   }
//
//   [HttpGet("{id}")]
//   public async Task<IResult> Get(int id, NpgsqlDataSource dataSource)
//   {
//     await using var db = await dataSource.OpenConnectionAsync();
//     var res = await db.QueryAsync<LinhaProducao>("SELECT * FROM linha_producao WHERE id = @id", new { id });
//     try
//     {
//       return Results.Ok(res.First());
//     }
//     catch (Exception)
//     {
//       return Results.NotFound();
//     }
//   }
// }

[JsonSerializable(typeof(LinhaProducao))]
[JsonSerializable(typeof(LinhaProducao[]))]
internal partial class AppJsonSerializerContext : JsonSerializerContext
{

}

public enum TipoHora
{
  HORA_EXTRA,
  BANCO_HORAS
};

