using System.Text.RegularExpressions;

namespace FleetManagement.API.Domain.Model.ValueObjects
{
    public record Mileage
    {
        public int Value { get; }
        public DateTime LastUpdated { get; }

        public Mileage(int value, DateTime? lastUpdated = null)
        {
            if (value < 0)
                throw new ArgumentException("Mileage cannot be negative");
            
            Value = value;
            LastUpdated = lastUpdated ?? DateTime.UtcNow;
        }

        public Mileage UpdateMileage(int newValue)
        {
            if (newValue < Value)
                throw new InvalidOperationException("New mileage cannot be less than current mileage");
            
            return new Mileage(newValue);
        }

        public bool IsHighMileage() => Value > 200000; // 200k km
        public bool NeedsService() => Value % 10000 == 0; // Every 10k km

        public static implicit operator int(Mileage mileage) => mileage.Value;
        public static explicit operator Mileage(int value) => new(value);
    }
}