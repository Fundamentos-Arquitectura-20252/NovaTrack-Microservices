using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IAM.API.Domain.Model.Aggregates
{
    public class User
    {
        [Key]
        public int Id { get; private set; }
        
        [Required]
        [StringLength(100)]
        public string FirstName { get; private set; } = string.Empty;
        
        [Required]
        [StringLength(100)]
        public string LastName { get; private set; } = string.Empty;
        
        [Required]
        [EmailAddress]
        [StringLength(255)]
        public string Email { get; private set; } = string.Empty;
        
        [Required]
        public string PasswordHash { get; private set; } = string.Empty;
        
        [Required]
        [StringLength(50)]
        public string Role { get; private set; } = "Administrator";
        
        public bool IsActive { get; private set; } = true;
        
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; private set; } = DateTime.UtcNow;
        
        [NotMapped]
        public string FullName => $"{FirstName} {LastName}";

        // Constructor para EF Core
        private User() { }

        // Constructor de dominio
        public User(string firstName, string lastName, string email, string passwordHash, string role = "Administrator")
        {
            FirstName = firstName ?? throw new ArgumentNullException(nameof(firstName));
            LastName = lastName ?? throw new ArgumentNullException(nameof(lastName));
            Email = email ?? throw new ArgumentNullException(nameof(email));
            PasswordHash = passwordHash ?? throw new ArgumentNullException(nameof(passwordHash));
            Role = role;
            IsActive = true;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        // Métodos de dominio
        public void UpdateProfile(string firstName, string lastName, string email)
        {
            FirstName = firstName ?? throw new ArgumentNullException(nameof(firstName));
            LastName = lastName ?? throw new ArgumentNullException(nameof(lastName));
            Email = email ?? throw new ArgumentNullException(nameof(email));
            UpdatedAt = DateTime.UtcNow;
        }

        public void ChangePassword(string newPasswordHash)
        {
            PasswordHash = newPasswordHash ?? throw new ArgumentNullException(nameof(newPasswordHash));
            UpdatedAt = DateTime.UtcNow;
        }

        public void Deactivate()
        {
            IsActive = false;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Activate()
        {
            IsActive = true;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}