using System.ComponentModel.DataAnnotations.Schema;

namespace Maintenance.API.Domain.Model.ValueObjects
{
    public record MaintenanceSchedule
    {
        public DateTime ScheduledDate { get; }
        public DateTime? CompletedDate { get; }
        
        [NotMapped]
        public List<ScheduleChange> Changes { get; }
        public int Priority { get; }

        public MaintenanceSchedule(DateTime scheduledDate, int priority = 3)
        {
            if (scheduledDate < DateTime.UtcNow.AddDays(-1))
                throw new ArgumentException("Scheduled date cannot be more than 1 day in the past");
            
            ScheduledDate = scheduledDate;
            Priority = ValidatePriority(priority);
            Changes = new List<ScheduleChange>();
        }

        private MaintenanceSchedule(DateTime scheduledDate, DateTime? completedDate, 
                                  List<ScheduleChange> changes, int priority)
        {
            ScheduledDate = scheduledDate;
            CompletedDate = completedDate;
            Changes = changes ?? new List<ScheduleChange>();
            Priority = priority;
        }

        public MaintenanceSchedule Reschedule(DateTime newDate, string reason = "")
        {
            if (CompletedDate.HasValue)
                throw new InvalidOperationException("Cannot reschedule completed maintenance");

            var changes = new List<ScheduleChange>(Changes)
            {
                new ScheduleChange(ScheduledDate, newDate, reason, DateTime.UtcNow)
            };

            return new MaintenanceSchedule(newDate, CompletedDate, changes, Priority);
        }

        public MaintenanceSchedule Complete()
        {
            if (CompletedDate.HasValue)
                throw new InvalidOperationException("Maintenance is already completed");

            return new MaintenanceSchedule(ScheduledDate, DateTime.UtcNow, Changes, Priority);
        }

        public bool IsOverdue() => !CompletedDate.HasValue && ScheduledDate < DateTime.UtcNow;
        public bool IsUpcoming(int daysThreshold = 7) => !CompletedDate.HasValue && 
            ScheduledDate <= DateTime.UtcNow.AddDays(daysThreshold) && ScheduledDate >= DateTime.UtcNow;
        public bool IsCompleted() => CompletedDate.HasValue;
        public int DaysUntilScheduled() => Math.Max(0, (ScheduledDate - DateTime.UtcNow).Days);
        public int DaysOverdue() => IsOverdue() ? (DateTime.UtcNow - ScheduledDate).Days : 0;
        public bool IsHighPriority() => Priority >= 4;
        public bool IsRescheduled() => Changes.Any();

        private static int ValidatePriority(int priority)
        {
            if (priority < 1 || priority > 5)
                throw new ArgumentException("Priority must be between 1 (low) and 5 (critical)");
            return priority;
        }
    }

    public record ScheduleChange(
        DateTime OriginalDate,
        DateTime NewDate,
        string Reason,
        DateTime ChangedAt
    );
}
