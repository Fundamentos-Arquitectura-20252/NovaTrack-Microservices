namespace Maintenance.API.Domain.Model.ValueObjects
{
    public record MaintenanceCost
    {
        public decimal EstimatedAmount { get; }
        public decimal? ActualAmount { get; }
        public string Currency { get; }
        public DateTime EstimatedDate { get; }
        public DateTime? ActualDate { get; }
        public List<string> CostBreakdown { get; }

        public MaintenanceCost(decimal estimatedAmount, string currency = "PEN")
        {
            if (estimatedAmount < 0)
                throw new ArgumentException("Estimated amount cannot be negative");
            
            EstimatedAmount = estimatedAmount;
            Currency = currency ?? "PEN";
            EstimatedDate = DateTime.UtcNow;
            CostBreakdown = new List<string>();
        }

        private MaintenanceCost(decimal estimatedAmount, decimal? actualAmount, string currency, 
                              DateTime estimatedDate, DateTime? actualDate, List<string> breakdown)
        {
            EstimatedAmount = estimatedAmount;
            ActualAmount = actualAmount;
            Currency = currency;
            EstimatedDate = estimatedDate;
            ActualDate = actualDate;
            CostBreakdown = breakdown ?? new List<string>();
        }

        public MaintenanceCost UpdateCost(decimal newEstimatedAmount, string reason = "")
        {
            if (ActualAmount.HasValue)
                throw new InvalidOperationException("Cannot update cost after actual amount is set");

            var breakdown = new List<string>(CostBreakdown);
            if (!string.IsNullOrEmpty(reason))
                breakdown.Add($"{DateTime.UtcNow:yyyy-MM-dd}: Updated from {EstimatedAmount} to {newEstimatedAmount} - {reason}");

            return new MaintenanceCost(newEstimatedAmount, ActualAmount, Currency, EstimatedDate, ActualDate, breakdown);
        }

        public MaintenanceCost SetActualCost(decimal actualAmount)
        {
            if (actualAmount < 0)
                throw new ArgumentException("Actual amount cannot be negative");

            var breakdown = new List<string>(CostBreakdown)
            {
                $"{DateTime.UtcNow:yyyy-MM-dd}: Actual cost set to {actualAmount}"
            };

            return new MaintenanceCost(EstimatedAmount, actualAmount, Currency, EstimatedDate, DateTime.UtcNow, breakdown);
        }

        public decimal GetVariance() => ActualAmount.HasValue ? ActualAmount.Value - EstimatedAmount : 0;
        public double GetVariancePercentage() => EstimatedAmount > 0 ? (double)(GetVariance() / EstimatedAmount) * 100 : 0;
        public bool IsOverBudget() => ActualAmount.HasValue && ActualAmount.Value > EstimatedAmount;
        public bool IsCompleted() => ActualAmount.HasValue;

        public static implicit operator decimal(MaintenanceCost cost) => cost.ActualAmount ?? cost.EstimatedAmount;
    }
}
