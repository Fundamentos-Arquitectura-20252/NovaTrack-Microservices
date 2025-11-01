using System.ComponentModel.DataAnnotations;
using FleetManagement.API.Domain.Model.ValueObjects;

namespace FleetManagement.API.Domain.Model.Aggregates
{
    public class Fleet
    {
        [Key]
        public int Id { get; private set; }
        
        [Required]
        [StringLength(20)]
        public string Code { get; private set; } = string.Empty;
        
        [Required]
        [StringLength(100)]
        public string Name { get; private set; } = string.Empty;
        
        [StringLength(500)]
        public string Description { get; private set; } = string.Empty;
        
        public FleetType Type { get; private set; } = FleetType.Primary;
        
        public bool IsActive { get; private set; } = true;
        
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; private set; } = DateTime.UtcNow;
        
        // Navigation properties
        public virtual ICollection<Vehicle> Vehicles { get; private set; } = new List<Vehicle>();

        // Constructor para EF Core
        private Fleet() { }

        // Constructor de dominio
        public Fleet(string code, string name, string description, FleetType type)
        {
            Code = GenerateCodeIfEmpty(code, name);
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Description = description ?? string.Empty;
            Type = type;
            IsActive = true;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        // Métodos de dominio
        public void UpdateFleetInfo(string name, string description, FleetType type)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Description = description ?? string.Empty;
            Type = type;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Activate()
        {
            IsActive = true;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Deactivate()
        {
            if (Vehicles.Any(v => v.IsActive))
                throw new InvalidOperationException("Cannot deactivate fleet with active vehicles");
            
            IsActive = false;
            UpdatedAt = DateTime.UtcNow;
        }

        public void AddVehicle(Vehicle vehicle)
        {
            if (!IsActive)
                throw new InvalidOperationException("Cannot add vehicles to inactive fleet");
            
            if (vehicle == null)
                throw new ArgumentNullException(nameof(vehicle));
            
            Vehicles.Add(vehicle);
            UpdatedAt = DateTime.UtcNow;
        }

        public void RemoveVehicle(Vehicle vehicle)
        {
            if (vehicle == null)
                throw new ArgumentNullException(nameof(vehicle));
            
            Vehicles.Remove(vehicle);
            UpdatedAt = DateTime.UtcNow;
        }

        public int GetActiveVehicleCount()
        {
            return Vehicles.Count(v => v.IsActive);
        }

        public int GetVehiclesInMaintenanceCount()
        {
            return Vehicles.Count(v => v.IsActive && v.Status == VehicleStatus.Maintenance);
        }

        public double GetPerformanceRate()
        {
            var activeVehicles = GetActiveVehicleCount();
            if (activeVehicles == 0) return 100;
            
            var vehiclesInMaintenance = GetVehiclesInMaintenanceCount();
            return Math.Round((double)(activeVehicles - vehiclesInMaintenance) / activeVehicles * 100, 1);
        }

        public double GetUtilizationRate()
        {
            var totalVehicles = Vehicles.Count;
            if (totalVehicles == 0) return 0;
            
            var activeVehicles = GetActiveVehicleCount();
            return Math.Round((double)activeVehicles / totalVehicles * 100, 1);
        }

        public IEnumerable<Vehicle> GetAvailableVehicles()
        {
            return Vehicles.Where(v => v.IsActive && v.Status == VehicleStatus.Active && v.DriverId == null);
        }

        public IEnumerable<Vehicle> GetVehiclesNeedingMaintenance()
        {
            return Vehicles.Where(v => v.IsActive && (v.IsServiceDue() || v.IsServiceOverdue()));
        }

        public bool CanAcceptNewVehicles()
        {
            return IsActive && GetActiveVehicleCount() < GetMaxVehicleCapacity();
        }

        public void ValidateForDeletion()
        {
            if (Vehicles.Any(v => v.IsActive))
                throw new InvalidOperationException("Cannot delete fleet with active vehicles");
        }

        public FleetStatistics GetStatistics()
        {
            return new FleetStatistics
            {
                TotalVehicles = Vehicles.Count,
                ActiveVehicles = GetActiveVehicleCount(),
                VehiclesInMaintenance = GetVehiclesInMaintenanceCount(),
                AvailableVehicles = GetAvailableVehicles().Count(),
                PerformanceRate = GetPerformanceRate(),
                UtilizationRate = GetUtilizationRate(),
                VehiclesNeedingMaintenance = GetVehiclesNeedingMaintenance().Count()
            };
        }

        // Métodos privados
        private static string GenerateCodeIfEmpty(string code, string name)
        {
            if (!string.IsNullOrEmpty(code))
                return code.ToUpperInvariant();
            
            // Generar código automático basado en el nombre
            var namePrefix = name.Length >= 3 
                ? name.Substring(0, 3).ToUpperInvariant()
                : name.ToUpperInvariant().PadRight(3, '0');
            
            var yearSuffix = DateTime.UtcNow.Year % 100;
            return $"FL{namePrefix}{yearSuffix:00}";
        }

        private int GetMaxVehicleCapacity()
        {
            return Type switch
            {
                FleetType.Primary => 50,
                FleetType.Secondary => 30,
                FleetType.External => 20,
                FleetType.Rental => 15,
                _ => 25
            };
        }
    }

    // Clase auxiliar para estadísticas
    public class FleetStatistics
    {
        public int TotalVehicles { get; set; }
        public int ActiveVehicles { get; set; }
        public int VehiclesInMaintenance { get; set; }
        public int AvailableVehicles { get; set; }
        public double PerformanceRate { get; set; }
        public double UtilizationRate { get; set; }
        public int VehiclesNeedingMaintenance { get; set; }
    }
}