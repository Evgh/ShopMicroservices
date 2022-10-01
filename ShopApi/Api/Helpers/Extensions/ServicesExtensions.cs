using Api.Helpers.HealthChecks;

namespace Api.Helpers.Extensions
{
    public static class ServicesExtensions
    {
        public static void RegisterHealthChecks(this IServiceCollection services)
        {
            services.AddHealthChecks().AddCheck<MongoDbHealthCheck>("MongoDb");
        }
    }
}
