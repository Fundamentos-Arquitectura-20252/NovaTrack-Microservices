using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FleetManagement.API.Domain.Model.ValueObjects;

namespace FleetManagement.API.Domain.Model.Aggregates
{
    public class Vehicle
    {
        [Key]
        public int Id { get; private set; }
        
        public LicensePlate LicensePlate { get; private set; }
        
        [Required]
        [StringLength(50)]
        public string Brand { get; private set; } = string.Empty;
        
        [Required]
        [StringLength(50)]
        public string Model { get; private set; } = string.Empty;
        
        public int Year { get; private set; }
        
        public int Mileage { get; private set; } = 0;
        
        public VehicleStatus Status { get; private set; } = VehicleStatus.Active;
        
        public DateTime? LastServiceDate { get; private set; }
        public DateTime? NextServiceDate { get; private set; }
        
        public bool IsActive { get; private set; } = true;
        
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; private set; } = DateTime.UtcNow;
        
        // Foreign Keys
        public int? FleetId { get; private set; }
        public int? DriverId { get; private set; }
        
        // Navigation properties
        [ForeignKey("FleetId")]
        public virtual Fleet? Fleet { get; private set; }

        // Constructor para EF Core
        private Vehicle() { }

        // Constructor de dominio
        public Vehicle(string licensePlate, string brand, string model, int year, int mileage, int? fleetId, int? driverId)
        {
            LicensePlate = new LicensePlate(licensePlate);
            Brand = brand ?? throw new ArgumentNullException(nameof(brand));
            Model = model ?? throw new ArgumentNullException(nameof(model));
            Year = ValidateYear(year);
            Mileage = ValidateMileage(mileage);
            FleetId = fleetId;
            DriverId = driverId;
            Status = VehicleStatus.Active;
            IsActive = true;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        // Métodos de dominio
        public void AssignToFleet(int fleetId)
        {
            FleetId = fleetId;
            UpdatedAt = DateTime.UtcNow;
        }

        public void RemoveFromFleet()
        {
            FleetId = null;
            UpdatedAt = DateTime.UtcNow;
        }

        public void AssignToDriver(int driverId)
        {
            DriverId = driverId;
            UpdatedAt = DateTime.UtcNow;
        }

        public void RemoveFromDriver()
        {
            DriverId = null;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateMileage(int newMileage)
        {
            if (newMileage < Mileage)
                throw new InvalidOperationException("New mileage cannot be less than current mileage");
            
            Mileage = newMileage;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateVehicleInfo(string brand, string model, int year)
        {
            Brand = brand ?? throw new ArgumentNullException(nameof(brand));
            Model = model ?? throw new ArgumentNullException(nameof(model));
            Year = ValidateYear(year);
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetInMaintenance()
        {
            Status = VehicleStatus.Maintenance;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetActive()
        {
            Status = VehicleStatus.Active;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetInactive()
        {
            Status = VehicleStatus.Inactive;
            IsActive = false;
            UpdatedAt = DateTime.UtcNow;
        }

        public void ScheduleService(DateTime serviceDate)
        {
            NextServiceDate = serviceDate;
            UpdatedAt = DateTime.UtcNow;
        }

        public void CompleteService()
        {
            LastServiceDate = DateTime.UtcNow;
            NextServiceDate = null;
            if (Status == VehicleStatus.Maintenance)
            {
                Status = VehicleStatus.Active;
            }
            UpdatedAt = DateTime.UtcNow;
        }

        public bool IsServiceDue()
        {
            return NextServiceDate.HasValue && NextServiceDate.Value <= DateTime.UtcNow;
        }

        public bool IsServiceOverdue()
        {
            return NextServiceDate.HasValue && NextServiceDate.Value < DateTime.UtcNow;
        }

        // Métodos de validación privados
        private static int ValidateYear(int year)
        {
            var currentYear = DateTime.UtcNow.Year;
            if (year < 1900 || year > currentYear + 1)
                throw new ArgumentException($"Year must be between 1900 and {currentYear + 1}");
            return year;
        }

        private static int ValidateMileage(int mileage)
        {
            if (mileage < 0)
                throw new ArgumentException("Mileage cannot be negative");
            return mileage;
        }
    }

    public enum VehicleStatus
    {
        Active,
        Maintenance,
        Inactive,
        Sold
    }
}