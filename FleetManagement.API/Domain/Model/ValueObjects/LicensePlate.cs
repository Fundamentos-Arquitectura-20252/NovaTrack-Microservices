using System.Text.RegularExpressions;

namespace FleetManagement.API.Domain.Model.ValueObjects
{
    public record LicensePlate
    {
        public string Value { get; }

        public LicensePlate(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("License plate cannot be empty");
                
            if (!IsValidFormat(value))
                throw new ArgumentException("Invalid license plate format");
                
            Value = value.ToUpperInvariant();
        }

        private static bool IsValidFormat(string licensePlate)
        {
            // Formato peruano: ABC-123 o ABC123
            var pattern = @"^[A-Z]{3}-?\d{3,4}$";
            return Regex.IsMatch(licensePlate.ToUpperInvariant(), pattern);
        }

        public static implicit operator string(LicensePlate licensePlate) => licensePlate.Value;
        public static explicit operator LicensePlate(string value) => new(value);
        
        public override string ToString() => Value;
    }
}
