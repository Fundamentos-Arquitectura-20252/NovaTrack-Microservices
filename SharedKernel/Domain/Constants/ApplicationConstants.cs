using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Builder;
using SharedKernel.Domain.Model.Events;
using SharedKernel.Infrastructure.Middleware;

// Application Constants
namespace SharedKernel.Domain.Constants
{
    public static class ApplicationConstants
    {
        public const string APPLICATION_NAME = "Flota365 Platform";
        public const string API_VERSION = "v1.0";
        public const string DEFAULT_CULTURE = "es-PE";
        public const string DEFAULT_CURRENCY = "PEN";
        public const string DEFAULT_TIMEZONE = "America/Lima";
        
        public static class Authentication
        {
            public const string DEFAULT_SCHEME = "Bearer";
            public const string ADMIN_ROLE = "Administrator";
            public const string SUPERVISOR_ROLE = "Supervisor";
            public const string OPERATOR_ROLE = "Operator";
            public const int TOKEN_EXPIRY_HOURS = 24;
        }

        public static class Pagination
        {
            public const int DEFAULT_PAGE_SIZE = 10;
            public const int MAX_PAGE_SIZE = 100;
            public const int MIN_PAGE_SIZE = 1;
        }

        public static class Validation
        {
            public const int MAX_NAME_LENGTH = 100;
            public const int MAX_DESCRIPTION_LENGTH = 500;
            public const int MAX_NOTES_LENGTH = 1000;
            public const int MIN_PASSWORD_LENGTH = 8;
            public const int MAX_EMAIL_LENGTH = 255;
            public const int MAX_PHONE_LENGTH = 15;
            public const int LICENSE_EXPIRY_WARNING_DAYS = 30;
            public const int SERVICE_DUE_WARNING_DAYS = 7;
        }

        public static class FleetManagement
        {
            public const int MAX_VEHICLE_YEAR = 2030;
            public const int MIN_VEHICLE_YEAR = 1900;
            public const int MAX_MILEAGE = 2000000; // 2M km
            public const int SERVICE_INTERVAL_KM = 10000;
            public const int MAINTENANCE_WARNING_DAYS = 14;
        }

        public static class Personnel
        {
            public const int MIN_DRIVER_AGE = 18;
            public const int MAX_DRIVER_AGE = 70;
            public const int MIN_EXPERIENCE_YEARS = 0;
            public const int MAX_EXPERIENCE_YEARS = 60;
            public const int LICENSE_EXPIRY_CRITICAL_DAYS = 7;
        }

        public static class Maintenance
        {
            public const int DEFAULT_MAINTENANCE_PRIORITY = 3;
            public const int MAX_MAINTENANCE_PRIORITY = 5;
            public const int MIN_MAINTENANCE_PRIORITY = 1;
            public const int WARRANTY_MONTHS = 6;
            public const int RECENT_SERVICE_DAYS = 30;
        }

        public static class ErrorCodes
        {
            public const string ENTITY_NOT_FOUND = "ENTITY_NOT_FOUND";
            public const string DUPLICATE_ENTITY = "DUPLICATE_ENTITY";
            public const string INVALID_OPERATION = "INVALID_OPERATION";
            public const string VALIDATION_FAILED = "VALIDATION_FAILED";
            public const string UNAUTHORIZED = "UNAUTHORIZED";
            public const string FORBIDDEN = "FORBIDDEN";
            public const string BUSINESS_RULE_VIOLATION = "BUSINESS_RULE_VIOLATION";
        }
    }

    public static class CacheKeys
    {
        public const string ACTIVE_VEHICLES = "active_vehicles";
        public const string FLEET_SUMMARY = "fleet_summary";
        public const string DRIVER_STATS = "driver_stats";
        public const string MAINTENANCE_STATS = "maintenance_stats";
        public const string DASHBOARD_STATS = "dashboard_stats";
        
        public static string VehicleById(int id) => $"vehicle_{id}";
        public static string DriverById(int id) => $"driver_{id}";
        public static string FleetById(int id) => $"fleet_{id}";
        public static string UserById(int id) => $"user_{id}";
        public static string VehiclesByFleet(int fleetId) => $"vehicles_fleet_{fleetId}";
    }
}