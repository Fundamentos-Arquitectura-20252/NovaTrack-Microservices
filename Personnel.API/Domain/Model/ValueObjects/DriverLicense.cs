using System.Text.RegularExpressions;

namespace Personnel.API.Domain.Model.ValueObjects
{
    public record DriverLicense
    {
        public string Number { get; }
        public DateTime ExpiryDate { get; }

        public DriverLicense(string number, DateTime expiryDate)
        {
            if (string.IsNullOrWhiteSpace(number))
                throw new ArgumentException("License number cannot be empty");
            
            if (!IsValidLicenseFormat(number))
                throw new ArgumentException("Invalid license number format");
            
            if (expiryDate <= DateTime.UtcNow)
                throw new ArgumentException("License expiry date must be in the future");

            Number = number.ToUpperInvariant();
            ExpiryDate = expiryDate;
        }

        private static bool IsValidLicenseFormat(string licenseNumber)
        {
            // Formato peruano: Letras seguidas de números
            var pattern = @"^[A-Z]{1,3}\d{6,8}$";
            return Regex.IsMatch(licenseNumber.ToUpperInvariant(), pattern);
        }

        public bool IsExpired() => ExpiryDate <= DateTime.UtcNow;
        
        public bool IsExpiringSoon(int daysThreshold = 30) 
            => ExpiryDate <= DateTime.UtcNow.AddDays(daysThreshold);
        
        public int DaysUntilExpiry() 
            => Math.Max(0, (ExpiryDate - DateTime.UtcNow).Days);

        public override string ToString() => Number;
    }
}
