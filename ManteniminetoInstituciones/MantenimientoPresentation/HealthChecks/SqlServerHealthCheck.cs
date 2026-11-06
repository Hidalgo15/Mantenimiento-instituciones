using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace MantenimientoPresentation.HealthChecks
{
    public sealed class SqlServerHealthCheck : IHealthCheck
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<SqlServerHealthCheck> _logger;

        public SqlServerHealthCheck(
            IConfiguration configuration,
            ILogger<SqlServerHealthCheck> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = default)
        {
            var connectionString = _configuration.GetConnectionString("ConexionSIGOB");
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                _logger.LogError("SQL Server readiness check failed because its connection string is not configured.");
                return HealthCheckResult.Unhealthy("A required dependency is unavailable.");
            }

            try
            {
                var connectionStringBuilder = new SqlConnectionStringBuilder(connectionString)
                {
                    ConnectTimeout = 3
                };

                await using var connection = new SqlConnection(connectionStringBuilder.ConnectionString);
                await connection.OpenAsync(cancellationToken);
                return HealthCheckResult.Healthy();
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (Exception exception)
            {
                _logger.LogWarning(exception, "SQL Server readiness check failed.");
                return HealthCheckResult.Unhealthy("A required dependency is unavailable.");
            }
        }
    }
}
