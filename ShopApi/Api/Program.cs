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

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IShopService, ShopService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ApplicationLayer.Interfaces.ILogger, InfrastuctureLayer.Utilitues.Logger>();

builder.Services.AddAutoMapper(typeof(ApiLayerMappingProfile), typeof(InfrastructureLayerMappingProfile));
builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("Database"));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.RegisterHealthChecks();
builder.Services.AddHealthChecksUI().AddInMemoryStorage(); ;

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

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
