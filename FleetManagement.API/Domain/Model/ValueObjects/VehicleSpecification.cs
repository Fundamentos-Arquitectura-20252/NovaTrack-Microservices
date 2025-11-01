using System.Text.RegularExpressions;


namespace FleetManagement.API.Domain.Model.ValueObjects
{
    public record VehicleSpecification
    {
        public string Brand { get; }
        public string Model { get; }
        public int Year { get; }
        public string Category { get; }

        public VehicleSpecification(string brand, string model, int year, string category = "Standard")
        {
            Brand = brand ?? throw new ArgumentNullException(nameof(brand));
            Model = model ?? throw new ArgumentNullException(nameof(model));
            Year = ValidateYear(year);
            Category = category ?? "Standard";
        }

        private static int ValidateYear(int year)
        {
            var currentYear = DateTime.UtcNow.Year;
            if (year < 1900 || year > currentYear + 1)
                throw new ArgumentException($"Year must be between 1900 and {currentYear + 1}");
            return year;
        }

        public string GetFullName() => $"{Brand} {Model} {Year}";
        public int GetAge() => DateTime.UtcNow.Year - Year;
        public bool IsVintage() => GetAge() > 25;
    }
}