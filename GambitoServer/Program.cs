using GambitoServer.Db;
using GambitoServer.Identity;
using Microsoft.AspNetCore.Identity;
using Scalar.AspNetCore;

// var builder = WebApplication.CreateSlimBuilder(args);
var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureHttpJsonOptions(o =>
{
  // o.SerializerOptions.ConfigureForNodaTime(DateTimeZoneProviders.Tzdb);
});

builder.Services.AddDbContext<GambitoContext>();
builder.Services.AddDbContext<GambitoIdentityContext>();

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

app.Run();
