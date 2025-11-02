using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Personnel.API.Domain.Model.ValueObjects;

namespace Personnel.API.Domain.Model.Aggregates
{
    public class Driver
    {
        [Key]
        public int Id { get; private set; }
        
        [Required]
        [StringLength(20)]
        public string Code { get; private set; } = string.Empty;
        
        [Required]
        [StringLength(100)]
        public string FirstName { get; private set; } = string.Empty;
        
        [Required]
        [StringLength(100)]
        public string LastName { get; private set; } = string.Empty;
        
        public DriverLicense License { get; private set; }
        
        public ContactInformation ContactInfo { get; private set; }
        
        public int ExperienceYears { get; private set; } = 0;
        
        public DriverStatus Status { get; private set; } = DriverStatus.Available;
        
        public bool IsActive { get; private set; } = true;
        
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; private set; } = DateTime.UtcNow;
        
        [NotMapped]
        public string FullName => $"{FirstName} {LastName}";
        
        [NotMapped]
        public bool IsLicenseExpiringSoon => License.IsExpiringSoon();

        // Constructor para EF Core
        private Driver() { }

        // Constructor de dominio
        public Driver(string code, string firstName, string lastName, string licenseNumber, 
                     DateTime licenseExpiryDate, string phone, string email, int experienceYears)
        {
            Code = GenerateCodeIfEmpty(code, firstName, lastName);
            FirstName = firstName ?? throw new ArgumentNullException(nameof(firstName));
            LastName = lastName ?? throw new ArgumentNullException(nameof(lastName));
            License = new DriverLicense(licenseNumber, licenseExpiryDate);
            ContactInfo = new ContactInformation(phone, email);
            ExperienceYears = ValidateExperience(experienceYears);
            Status = DriverStatus.Available;
            IsActive = true;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        // Métodos de dominio
        public void UpdatePersonalInfo(string firstName, string lastName, int experienceYears)
        {
            FirstName = firstName ?? throw new ArgumentNullException(nameof(firstName));
            LastName = lastName ?? throw new ArgumentNullException(nameof(lastName));
            ExperienceYears = ValidateExperience(experienceYears);
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateContactInfo(string phone, string email)
        {
            ContactInfo = new ContactInformation(phone, email);
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateLicense(string licenseNumber, DateTime expiryDate)
        {
            License = new DriverLicense(licenseNumber, expiryDate);
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetOnRoute()
        {
            if (!IsActive)
                throw new InvalidOperationException("Cannot set inactive driver on route");
            
            Status = DriverStatus.OnRoute;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetAvailable()
        {
            if (!IsActive)
                throw new InvalidOperationException("Cannot set inactive driver as available");
            
            Status = DriverStatus.Available;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetOnBreak()
        {
            Status = DriverStatus.OnBreak;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Suspend(string reason)
        {
            Status = DriverStatus.Suspended;
            UpdatedAt = DateTime.UtcNow;
            // TODO: Add SuspensionReason as a value object
        }

        public void Deactivate()
        {
            if (Status == DriverStatus.OnRoute)
                throw new InvalidOperationException("Cannot deactivate driver who is on route");
            
            IsActive = false;
            Status = DriverStatus.Inactive;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Activate()
        {
            IsActive = true;
            Status = DriverStatus.Available;
            UpdatedAt = DateTime.UtcNow;
        }

        public bool CanBeAssignedToVehicle()
        {
            return IsActive && 
                   Status == DriverStatus.Available && 
                   !License.IsExpired() &&
                   !License.IsExpiringSoon();
        }

        public void ValidateLicenseStatus()
        {
            if (License.IsExpired())
                throw new InvalidOperationException("Driver license has expired");
        }

        // Métodos privados de validación
        private static string GenerateCodeIfEmpty(string code, string firstName, string lastName)
        {
            if (!string.IsNullOrEmpty(code))
                return code;
            
            return $"DR{firstName.Substring(0, Math.Min(2, firstName.Length))}" +
                   $"{lastName.Substring(0, Math.Min(2, lastName.Length))}" +
                   $"{DateTime.UtcNow.Year % 100:00}";
        }

        private static int ValidateExperience(int years)
        {
            if (years < 0 || years > 60)
                throw new ArgumentException("Experience years must be between 0 and 60");
            return years;
        }
    }

    public enum DriverStatus
    {
        Available,
        OnRoute,
        OnBreak,
        Suspended,
        Inactive
    }
}