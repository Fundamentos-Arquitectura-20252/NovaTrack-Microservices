using SharedKernel.Domain.Constants;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Builder;
using SharedKernel.Domain.Model.Events;

namespace SharedKernel.Infrastructure.Utilities
{
    public static class SecurityHelper
    {
        public static string GenerateSecureCode(int length = 8)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static bool IsValidStrongPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password) || password.Length < ApplicationConstants.Validation.MIN_PASSWORD_LENGTH)
                return false;

            var hasUpperChar = password.Any(char.IsUpper);
            var hasLowerChar = password.Any(char.IsLower);
            var hasDigit = password.Any(char.IsDigit);
            var hasSpecialChar = password.Any(ch => !char.IsLetterOrDigit(ch));

            return hasUpperChar && hasLowerChar && hasDigit && hasSpecialChar;
        }
    }

    public static class FormatHelper
    {
        public static string FormatCurrency(decimal amount, string currency = ApplicationConstants.DEFAULT_CURRENCY)
        {
            return currency switch
            {
                "PEN" => $"S/ {amount:N2}",
                "USD" => $"$ {amount:N2}",
                "EUR" => $"€ {amount:N2}",
                _ => $"{currency} {amount:N2}"
            };
        }

        public static string FormatDistance(int kilometers)
        {
            return kilometers switch
            {
                < 1000 => $"{kilometers} km",
                >= 1000 and < 1000000 => $"{kilometers / 1000.0:F1}K km",
                _ => $"{kilometers / 1000000.0:F1}M km"
            };
        }

        public static string FormatDuration(TimeSpan duration)
        {
            if (duration.TotalDays >= 1)
                return $"{(int)duration.TotalDays}d {duration.Hours}h";
            if (duration.TotalHours >= 1)
                return $"{(int)duration.TotalHours}h {duration.Minutes}m";
            return $"{duration.Minutes}m";
        }

        public static string FormatRelativeTime(DateTime dateTime)
        {
            var timeSpan = DateTime.UtcNow - dateTime;

            return timeSpan switch
            {
                { TotalDays: >= 365 } => $"{(int)(timeSpan.TotalDays / 365)} año(s) atrás",
                { TotalDays: >= 30 } => $"{(int)(timeSpan.TotalDays / 30)} mes(es) atrás",
                { TotalDays: >= 7 } => $"{(int)(timeSpan.TotalDays / 7)} semana(s) atrás",
                { TotalDays: >= 1 } => $"{(int)timeSpan.TotalDays} día(s) atrás",
                { TotalHours: >= 1 } => $"{(int)timeSpan.TotalHours} hora(s) atrás",
                { TotalMinutes: >= 1 } => $"{(int)timeSpan.TotalMinutes} minuto(s) atrás",
                _ => "Hace un momento"
            };
        }
    }
}