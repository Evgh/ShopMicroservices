using Identity.Data;
using IdentityServer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;
using System.Reflection;


var builder = WebApplication.CreateBuilder(args);

string dbConnectionString = builder.Configuration.GetConnectionString("DefaultDBConnection");
string migrationAssemblyName = typeof(Program).GetTypeInfo().Assembly.GetName().Name;


builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlite(dbConnectionString, opt => opt.MigrationsAssembly(migrationAssemblyName));
});

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();


builder.Services.AddIdentityServer(options =>
{
    options.EmitStaticAudienceClaim = true;
})
    .AddConfigurationStore(options =>
    {
        options.ConfigureDbContext = builder => builder.UseSqlite(dbConnectionString, opt => opt.MigrationsAssembly(migrationAssemblyName));
    })
    .AddOperationalStore(options =>
    {
        options.ConfigureDbContext = builder => builder.UseSqlite(dbConnectionString, opt => opt.MigrationsAssembly(migrationAssemblyName));
    })
    .AddDeveloperSigningCredential();

SeedData.EnsureSeedData(dbConnectionString);


var logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
    .MinimumLevel.Override("HealthChecks", LogEventLevel.Warning)
    .WriteTo.Console(outputTemplate:
        "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
    .CreateLogger();

builder.Host.UseSerilog(logger);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseSerilogRequestLogging();
app.UseIdentityServer();
app.Run();
