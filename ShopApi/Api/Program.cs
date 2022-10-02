using DomainLayer.Interfaces;
using ApplicationLayer.Services;
using InfrastuctureLayer.Data;
using InfrastuctureLayer.Data.Parameters;
using ApplicationLayer.Interfaces;
using InfrastuctureLayer.Mappers;
using Api.Mappers;
using Api.Helpers.Extensions;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using HealthChecks.UI.Client;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IShopService, ShopService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ApplicationLayer.Interfaces.ILogger, InfrastuctureLayer.Utilitues.Logger>();

builder.Services.AddAutoMapper(typeof(ApiLayerMappingProfile), typeof(InfrastructureLayerMappingProfile));
builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("Database"));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();


builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
     {
         options.Audience = "shopApi";
         options.Authority = "https://localhost:5001";
     });

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "1.0",
        Title = "Shop Api",
        Description = "An ASP.NET Core Web API for store shops and their locations"
    });

    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.OAuth2,
        Flows = new OpenApiOAuthFlows
        {
            AuthorizationCode = new OpenApiOAuthFlow
            {
                AuthorizationUrl = new Uri("https://localhost:5001/connect/authorize"),
                TokenUrl = new Uri("https://localhost:5001/connect/token"),
                Scopes = new Dictionary<string, string> { { "test", "full access" } }
            }
        }
    });
});

builder.Services.RegisterHealthChecks();
builder.Services.AddHealthChecksUI().AddInMemoryStorage(); ;

var app = builder.Build();

//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();

app.MapHealthChecks("/hc", new HealthCheckOptions
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});
app.MapHealthChecks("/liveness", new HealthCheckOptions
{
    Predicate = healthCheckRegistration => healthCheckRegistration.Name.Contains("self")
});
app.MapHealthChecksUI();
app.MapControllers();

app.Run();
