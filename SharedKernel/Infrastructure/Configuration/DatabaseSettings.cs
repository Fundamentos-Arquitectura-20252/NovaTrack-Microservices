using SharedKernel.Domain.Constants;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Builder;
using SharedKernel.Domain.Model.Events;
using SharedKernel.Infrastructure.Middleware;


// Configuration Helpers
namespace SharedKernel.Infrastructure.Configuration
{
    public class DatabaseSettings
    {
        public string ConnectionString { get; set; } = string.Empty;
        public int CommandTimeout { get; set; } = 30;
        public int MaxRetryCount { get; set; } = 3;
        public bool EnableSensitiveDataLogging { get; set; } = false;
        public bool EnableDetailedErrors { get; set; } = false;
    }

    public class ApplicationSettings
    {
        public string Name { get; set; } = ApplicationConstants.APPLICATION_NAME;
        public string Version { get; set; } = ApplicationConstants.API_VERSION;
        public string Environment { get; set; } = "Development";
        public string Culture { get; set; } = ApplicationConstants.DEFAULT_CULTURE;
        public string TimeZone { get; set; } = ApplicationConstants.DEFAULT_TIMEZONE;
        public bool EnableSwagger { get; set; } = true;
        public bool EnableHealthChecks { get; set; } = true;
    }

    public class CachingSettings
    {
        public bool Enabled { get; set; } = true;
        public int DefaultExpirationMinutes { get; set; } = 60;
        public int StatsExpirationMinutes { get; set; } = 15;
        public string RedisConnectionString { get; set; } = string.Empty;
    }

    public class LoggingSettings
    {
        public string MinimumLevel { get; set; } = "Information";
        public bool EnableRequestLogging { get; set; } = true;
        public bool EnableSqlLogging { get; set; } = false;
        public bool IncludeScopes { get; set; } = true;
        public string LogTemplate { get; set; } = "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level:u3}] {Message:lj}{NewLine}{Exception}";
    }
}
