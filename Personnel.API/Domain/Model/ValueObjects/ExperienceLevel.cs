namespace Personnel.API.Domain.Model.ValueObjects
{
    public record ExperienceLevel
    {
        public int Years { get; }
        public string Level { get; }
        public string Description { get; }

        public ExperienceLevel(int years)
        {
            if (years < 0 || years > 60)
                throw new ArgumentException("Experience years must be between 0 and 60");

            Years = years;
            (Level, Description) = CalculateLevel(years);
        }

        private static (string Level, string Description) CalculateLevel(int years)
        {
            return years switch
            {
                < 1 => ("Novice", "Less than 1 year of experience"),
                < 3 => ("Junior", "1-2 years of experience"),
                < 5 => ("Intermediate", "3-4 years of experience"),
                < 10 => ("Senior", "5-9 years of experience"),
                < 20 => ("Expert", "10-19 years of experience"),
                _ => ("Master", "20+ years of experience")
            };
        }

        public bool IsExperienced() => Years >= 5;
        public bool IsSenior() => Years >= 10;
        public double GetExperienceRating() => Math.Min(10.0, Years / 2.0);

        public static implicit operator int(ExperienceLevel experience) => experience.Years;
        public static explicit operator ExperienceLevel(int years) => new(years);
    }
}
