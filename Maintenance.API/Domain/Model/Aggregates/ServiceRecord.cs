using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Maintenance.API.Domain.Model.ValueObjects;

namespace Maintenance.API.Domain.Model.Aggregates
{
    public class ServiceRecord
    {
        [Key]
        public int Id { get; private set; }
        
        [Required]
        public int VehicleId { get; private set; }
        
        public ServiceType ServiceType { get; private set; }
        
        [StringLength(500)]
        public string Description { get; private set; } = string.Empty;
        
        public ServiceCost Cost { get; private set; }
        
        public DateTime ServiceDate { get; private set; }
        public int MileageAtService { get; private set; }
        
        [StringLength(200)]
        public string ServiceProvider { get; private set; } = string.Empty;
        
        [StringLength(100)]
        public string TechnicianName { get; private set; } = string.Empty;
        
        public ServiceQuality Quality { get; private set; } = ServiceQuality.Good;
        
        [StringLength(1000)]
        public string PartsUsed { get; private set; } = string.Empty;
        
        [StringLength(500)]
        public string Notes { get; private set; } = string.Empty;
        
        public DateTime? NextServiceDue { get; private set; }
        public int? NextServiceMileage { get; private set; }
        
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

        // Constructor para EF Core
        private ServiceRecord() { }

        // Constructor de dominio
        public ServiceRecord(int vehicleId, ServiceType serviceType, string description, 
                           decimal cost, DateTime serviceDate, int mileageAtService, 
                           string serviceProvider, string technicianName = "")
        {
            VehicleId = vehicleId;
            ServiceType = serviceType;
            Description = description ?? throw new ArgumentNullException(nameof(description));
            Cost = new ServiceCost(cost);
            ServiceDate = ValidateServiceDate(serviceDate);
            MileageAtService = ValidateMileage(mileageAtService);
            ServiceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            TechnicianName = technicianName ?? string.Empty;
            Quality = ServiceQuality.Good;
            PartsUsed = string.Empty;
            Notes = string.Empty;
            CreatedAt = DateTime.UtcNow;

            CalculateNextServiceDue();
        }

        // Métodos de dominio
        public void UpdateDescription(string description)
        {
            Description = description ?? throw new ArgumentNullException(nameof(description));
        }

        public void UpdateCost(decimal newCost)
        {
            Cost = new ServiceCost(newCost);
        }

        public void SetQuality(ServiceQuality quality)
        {
            Quality = quality;
        }

        public void AddPartsUsed(string parts)
        {
            if (!string.IsNullOrEmpty(parts))
            {
                PartsUsed = string.IsNullOrEmpty(PartsUsed) 
                    ? parts 
                    : $"{PartsUsed}; {parts}";
            }
        }

        public void AddNotes(string notes)
        {
            if (!string.IsNullOrEmpty(notes))
            {
                Notes = string.IsNullOrEmpty(Notes) 
                    ? notes 
                    : $"{Notes}\n{notes}";
            }
        }

        public void UpdateServiceProvider(string provider, string technician = "")
        {
            ServiceProvider = provider ?? throw new ArgumentNullException(nameof(provider));
            if (!string.IsNullOrEmpty(technician))
                TechnicianName = technician;
        }

        public void SetNextServiceSchedule(DateTime? nextDate, int? nextMileage)
        {
            NextServiceDue = nextDate;
            NextServiceMileage = nextMileage;
        }

        public bool IsWarrantyValid(int warrantyMonths = 6)
        {
            return ServiceDate.AddMonths(warrantyMonths) >= DateTime.UtcNow;
        }

        public bool IsRecentService(int daysThreshold = 30)
        {
            return ServiceDate >= DateTime.UtcNow.AddDays(-daysThreshold);
        }

        public ServiceRecord CreateFollowUpService(string description, decimal estimatedCost)
        {
            if (!IsWarrantyValid())
                throw new InvalidOperationException("Cannot create follow-up service outside warranty period");

            return new ServiceRecord(
                VehicleId,
                ServiceType.FollowUp,
                $"Follow-up: {description}",
                estimatedCost,
                DateTime.UtcNow,
                MileageAtService, // Would need current mileage in real scenario
                ServiceProvider,
                TechnicianName
            );
        }

        // Métodos privados
        private void CalculateNextServiceDue()
        {
            var serviceInterval = GetServiceInterval();
            if (serviceInterval.HasValue)
            {
                NextServiceDue = ServiceDate.AddMonths(serviceInterval.Value);
                NextServiceMileage = MileageAtService + GetMileageInterval();
            }
        }

        private int? GetServiceInterval()
        {
            return ServiceType switch
            {
                ServiceType.OilChange => 3,
                ServiceType.GeneralInspection => 6,
                ServiceType.TireRotation => 6,
                ServiceType.BrakeInspection => 12,
                ServiceType.TransmissionService => 24,
                ServiceType.MajorService => 12,
                _ => null
            };
        }

        private int GetMileageInterval()
        {
            return ServiceType switch
            {
                ServiceType.OilChange => 5000,
                ServiceType.GeneralInspection => 10000,
                ServiceType.TireRotation => 10000,
                ServiceType.BrakeInspection => 20000,
                ServiceType.TransmissionService => 50000,
                ServiceType.MajorService => 20000,
                _ => 15000
            };
        }

        private static DateTime ValidateServiceDate(DateTime serviceDate)
        {
            if (serviceDate > DateTime.UtcNow.AddDays(1))
                throw new ArgumentException("Service date cannot be in the future");
            
            if (serviceDate < DateTime.UtcNow.AddYears(-10))
                throw new ArgumentException("Service date cannot be more than 10 years ago");
            
            return serviceDate;
        }

        private static int ValidateMileage(int mileage)
        {
            if (mileage < 0)
                throw new ArgumentException("Mileage cannot be negative");
            
            if (mileage > 2000000) // 2M km seems reasonable max
                throw new ArgumentException("Mileage seems unusually high");
            
            return mileage;
        }
    }

    public enum ServiceType
    {
        OilChange,
        TireRotation,
        BrakeInspection,
        BrakeService,
        TransmissionService,
        EngineService,
        GeneralInspection,
        AirFilterChange,
        FuelFilterChange,
        CoolantService,
        BatteryService,
        MajorService,
        EmergencyRepair,
        FollowUp,
        Other
    }

    public enum ServiceQuality
    {
        Poor = 1,
        Fair = 2,
        Good = 3,
        VeryGood = 4,
        Excellent = 5
    }
}