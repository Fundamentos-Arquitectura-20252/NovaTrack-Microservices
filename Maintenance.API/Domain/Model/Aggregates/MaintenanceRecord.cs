using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Maintenance.API.Domain.Model.ValueObjects;

namespace Maintenance.API.Domain.Model.Aggregates
{
    public class MaintenanceRecord
    {
        [Key]
        public int Id { get; private set; }
        
        [Required]
        public int VehicleId { get; private set; }
        
        [Required]
        [StringLength(200)]
        public string Description { get; private set; } = string.Empty;
        
        public MaintenanceType Type { get; private set; }
        
        public MaintenanceCost Cost { get; private set; }
        
        public MaintenanceSchedule Schedule { get; private set; }
        
        public MaintenanceStatus Status { get; private set; } = MaintenanceStatus.Scheduled;
        
        [StringLength(500)]
        public string Notes { get; private set; } = string.Empty;
        
        [StringLength(200)]
        public string ServiceProvider { get; private set; } = string.Empty;
        
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; private set; } = DateTime.UtcNow;

        // Constructor para EF Core
        private MaintenanceRecord() { }

        // Constructor de dominio
        public MaintenanceRecord(int vehicleId, string description, MaintenanceType type, 
                               decimal estimatedCost, DateTime scheduledDate, string serviceProvider = "")
        {
            VehicleId = vehicleId;
            Description = description ?? throw new ArgumentNullException(nameof(description));
            Type = type;
            Cost = new MaintenanceCost(estimatedCost);
            Schedule = new MaintenanceSchedule(scheduledDate);
            Status = MaintenanceStatus.Scheduled;
            ServiceProvider = serviceProvider ?? string.Empty;
            Notes = string.Empty;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        // Métodos de dominio
        public void UpdateDescription(string description)
        {
            Description = description ?? throw new ArgumentNullException(nameof(description));
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateCost(decimal newCost, string reason = "")
        {
            Cost = Cost.UpdateCost(newCost, reason);
            UpdatedAt = DateTime.UtcNow;
        }

        public void RescheduleService(DateTime newDate, string reason = "")
        {
            Schedule = Schedule.Reschedule(newDate, reason);
            UpdatedAt = DateTime.UtcNow;
        }

        public void StartMaintenance()
        {
            if (Status != MaintenanceStatus.Scheduled)
                throw new InvalidOperationException("Can only start scheduled maintenance");
            
            Status = MaintenanceStatus.InProgress;
            UpdatedAt = DateTime.UtcNow;
        }

        public void CompleteMaintenance(decimal actualCost, string completionNotes = "")
        {
            if (Status != MaintenanceStatus.InProgress)
                throw new InvalidOperationException("Can only complete maintenance that is in progress");
            
            Status = MaintenanceStatus.Completed;
            Schedule = Schedule.Complete();
            Cost = Cost.SetActualCost(actualCost);
            
            if (!string.IsNullOrEmpty(completionNotes))
                Notes = completionNotes;
            
            UpdatedAt = DateTime.UtcNow;

            // Disparar evento de dominio
            // TODO: Implement domain events
        }

        public void CancelMaintenance(string reason)
        {
            if (Status == MaintenanceStatus.Completed)
                throw new InvalidOperationException("Cannot cancel completed maintenance");
            
            Status = MaintenanceStatus.Cancelled;
            Notes = $"Cancelled: {reason}";
            UpdatedAt = DateTime.UtcNow;
        }

        public void AddNotes(string additionalNotes)
        {
            if (!string.IsNullOrEmpty(additionalNotes))
            {
                Notes = string.IsNullOrEmpty(Notes) 
                    ? additionalNotes 
                    : $"{Notes}\n{DateTime.UtcNow:yyyy-MM-dd}: {additionalNotes}";
                UpdatedAt = DateTime.UtcNow;
            }
        }

        public void UpdateServiceProvider(string provider)
        {
            ServiceProvider = provider ?? string.Empty;
            UpdatedAt = DateTime.UtcNow;
        }

        public bool IsOverdue()
        {
            return Status == MaintenanceStatus.Scheduled && Schedule.IsOverdue();
        }

        public bool IsUpcoming(int daysThreshold = 7)
        {
            return Status == MaintenanceStatus.Scheduled && Schedule.IsUpcoming(daysThreshold);
        }

        public int DaysUntilScheduled()
        {
            return Schedule.DaysUntilScheduled();
        }

        public void ValidateForCompletion()
        {
            if (Status != MaintenanceStatus.InProgress)
                throw new InvalidOperationException("Maintenance must be in progress to complete");
            
            if (Cost.EstimatedAmount <= 0)
                throw new InvalidOperationException("Maintenance must have a valid cost estimate");
        }
    }

    public enum MaintenanceStatus
    {
        Scheduled,
        InProgress,
        Completed,
        Cancelled,
        Postponed
    }

    public enum MaintenanceType
    {
        Preventive,
        Corrective,
        Emergency,
        Inspection,
        OilChange,
        TireChange,
        BrakeService,
        EngineService,
        Transmission,
        Other
    }
}