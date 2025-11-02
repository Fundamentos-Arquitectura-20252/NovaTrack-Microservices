using System.Text.RegularExpressions;

namespace Personnel.API.Domain.Model.ValueObjects
{
    public record ContactInformation
    {
        public string Phone { get; }
        public string Email { get; }

        public ContactInformation(string phone, string email)
        {
            Phone = ValidatePhone(phone);
            Email = ValidateEmail(email);
        }

        private static string ValidatePhone(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                return string.Empty;
            
            // Formato peruano: +51 seguido de 9 dígitos o solo 9 dígitos
            var cleanPhone = Regex.Replace(phone, @"[^\d+]", "");
            
            if (!Regex.IsMatch(cleanPhone, @"^(\+51)?9\d{8}$"))
                throw new ArgumentException("Invalid phone number format for Peru");
            
            return cleanPhone.StartsWith("+51") ? cleanPhone : $"+51{cleanPhone}";
        }

        private static string ValidateEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return string.Empty;
            
            if (!IsValidEmail(email))
                throw new ArgumentException("Invalid email format");
            
            return email.ToLowerInvariant();
        }

        private static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public bool HasPhone() => !string.IsNullOrEmpty(Phone);
        public bool HasEmail() => !string.IsNullOrEmpty(Email);
        public bool IsComplete() => HasPhone() && HasEmail();
    }
}
