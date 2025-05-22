using Dapper;
using GambitoServer.Db;
using GambitoServer.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

// var builder = WebApplication.CreateSlimBuilder(args);
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<GambitoContext>();
builder.Services.AddDbContext<GambitoIdentityContext>();

builder.Services.AddControllers();
builder.Services.AddProblemDetails();

// Localization
builder.Services.AddLocalization();

builder.Services.AddAuthorization();
builder.Services.AddAuthentication().AddCookie(IdentityConstants.ApplicationScheme);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddIdentityCore<User>()
  .AddEntityFrameworkStores<GambitoIdentityContext>()
  .AddApiEndpoints();

// Logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddResponseCompression();

var app = builder.Build();
await using var scope = app.Services.CreateAsyncScope();

app.UseStatusCodePages();

var supportedCultures = new[] { "pt-BR", "es-419", "en-US", };
var localizationOptions = new RequestLocalizationOptions()
    .SetDefaultCulture(supportedCultures[0])
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);
app.UseRequestLocalization();

app.UseAntiforgery();

if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();

  app.UseDeveloperExceptionPage();
}
else
{
  app.UseExceptionHandler(exceptionHandlerApp
      => exceptionHandlerApp.Run(async context
        => await Results.Problem()
        .ExecuteAsync(context)));
  app.UseHsts();
  app.UseResponseCompression();
}

app.UseAuthorization();
app.UseAuthentication();

app.MapIdentityApi<User>();

var router = app.MapGroup("/api");

app.MapControllers();

LinhaProducaoMinimalController.Register(router);

app.Run();

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

[ApiController]
[Route("api/linha-producao")]
public class LinhaProducaoController : ControllerBase
{
  [HttpGet("")]
  public List<LinhaProducao> GetAll(GambitoContext dataSource)
  {
    var res = dataSource.LinhaProducaos;
    return res.ToList();
  }

  [HttpGet("{id}")]
  public async Task<IResult> Get(int id, NpgsqlDataSource dataSource)
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
  }
}

