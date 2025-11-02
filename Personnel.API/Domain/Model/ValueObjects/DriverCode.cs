using System.Text.RegularExpressions;

namespace Personnel.API.Domain.Model.ValueObjects
{
    public record DriverCode
    {
        public string Value { get; }

        public DriverCode(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Driver code cannot be empty");
            
            if (!IsValidFormat(value))
                throw new ArgumentException("Invalid driver code format");
            
            Value = value.ToUpperInvariant();
        }

        private static bool IsValidFormat(string code)
        {
            // Formato: DR + 2-4 letras + 2 dígitos
            var pattern = @"^DR[A-Z]{2,4}\d{2}$";
            return Regex.IsMatch(code.ToUpperInvariant(), pattern);
        }

        public static DriverCode Generate(string firstName, string lastName, int? year = null)
        {
            var yearSuffix = (year ?? DateTime.UtcNow.Year) % 100;
            var initials = $"{firstName.Substring(0, Math.Min(2, firstName.Length))}" +
                          $"{lastName.Substring(0, Math.Min(2, lastName.Length))}";
            
            return new DriverCode($"DR{initials.ToUpperInvariant()}{yearSuffix:00}");
        }

        public static implicit operator string(DriverCode code) => code.Value;
        public static explicit operator DriverCode(string value) => new(value);
        
        public override string ToString() => Value;
    }
}
