namespace Maintenance.API.Domain.Model.ValueObjects
{
    public record ServiceCost
    {
        public decimal Amount { get; }
        public string Currency { get; }
        public ServiceCostType Type { get; }
        public List<CostItem> Items { get; set; }
        
        private ServiceCost() { }

        public ServiceCost(decimal amount, string currency = "PEN", ServiceCostType type = ServiceCostType.Total)
        {
            if (amount < 0)
                throw new ArgumentException("Service cost cannot be negative");
            
            Amount = amount;
            Currency = currency ?? "PEN";
            Type = type;
            Items = new List<CostItem>();
        }

        public ServiceCost AddCostItem(string description, decimal amount, ServiceCostCategory category)
        {
            var newItems = new List<CostItem>(Items)
            {
                new CostItem(description, amount, category)
            };

            return new ServiceCost(Amount + amount, Currency, Type) with { Items = newItems };
        }

        public decimal GetLaborCost() => Items.Where(i => i.Category == ServiceCostCategory.Labor).Sum(i => i.Amount);
        public decimal GetPartsCost() => Items.Where(i => i.Category == ServiceCostCategory.Parts).Sum(i => i.Amount);
        public decimal GetMiscCost() => Items.Where(i => i.Category == ServiceCostCategory.Miscellaneous).Sum(i => i.Amount);

        public static implicit operator decimal(ServiceCost cost) => cost.Amount;
    }

    public record CostItem(
        string Description,
        decimal Amount,
        ServiceCostCategory Category
    );

    public enum ServiceCostType
    {
        Estimate,
        Actual,
        Total
    }

    public enum ServiceCostCategory
    {
        Labor,
        Parts,
        Miscellaneous
    }
}
