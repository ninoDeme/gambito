using GambitoServer.Db;
using GambitoServer.LinhaProducao;
using Microsoft.AspNetCore.Identity;
using Scalar.AspNetCore;

// var builder = WebApplication.CreateSlimBuilder(args);
var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureHttpJsonOptions(o =>
{
  // o.SerializerOptions.ConfigureForNodaTime(DateTimeZoneProviders.Tzdb);
});

builder.Services.AddDbContext<GambitoContext>();

builder.Services.AddControllers();
builder.Services.AddProblemDetails();

builder.Services.AddAntiforgery();

// Localization
builder.Services.AddLocalization();

builder.Services.AddAuthorization();
builder.Services.AddAuthentication().AddCookie(IdentityConstants.ApplicationScheme);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddOpenApi();

builder.Services.AddIdentityCore<User>()
  .AddEntityFrameworkStores<GambitoContext>()
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

if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();

  app.MapOpenApi();
  app.MapScalarApiReference();

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
  app.UseAntiforgery();
}

app.UseAuthorization();
app.UseAuthentication();

app.MapGroup("api/auth").WithTags("Auth").MapIdentityApi<User>();

app.MapControllers();

app.MapLinhaProducaoEndpoints();

app.MapGroup("api/org").WithTags("Org").MapPost("{name}", (GambitoContext db, string name) =>
{
  var o = db.Org.First()!;
  o.Nome = name;
  db.SaveChanges();
  return o;
});

app.MapGroup("api/org").WithTags("Org").MapPost("", (GambitoContext db) =>
{
  var o = new OrgEntity
  {
    Nome = "Teste"
  };
  db.Org.Add(o);
  db.SaveChanges();
  return o;
});

app.Run();
