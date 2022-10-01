using InfrastuctureLayer.Data.Parameters;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Api.Helpers.HealthChecks
{
    public class MongoDbHealthCheck : IHealthCheck
    {
        private readonly IOptions<DatabaseSettings> _databaseSettings;

        public MongoDbHealthCheck(IOptions<DatabaseSettings> databaseSettings)
        {
            _databaseSettings = databaseSettings ?? throw new ArgumentNullException(nameof(databaseSettings));
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                var mongoClient = new MongoClient(_databaseSettings.Value.ConnectionString);
                var mongoDatabase = mongoClient.GetDatabase(_databaseSettings.Value.DatabaseName);
                await mongoDatabase.RunCommandAsync((Command<BsonDocument>)"{ping:1}", cancellationToken: cancellationToken);
            }
            catch (Exception ex)
            {
                return new HealthCheckResult(context.Registration.FailureStatus, exception: ex);
            }

            return HealthCheckResult.Healthy();
        }
    }
}
