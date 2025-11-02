using Microsoft.EntityFrameworkCore;
using Personnel.API.Domain.Model.Aggregates;

namespace Personnel.API.Infrastructure.Persistence;

public class PersonnelDbContext: DbContext
{
    public PersonnelDbContext(DbContextOptions<PersonnelDbContext> options) : base(options) { }
    public DbSet<Driver> Drivers { get; set; } = null!;
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        ConfigurePersonnelEntities(modelBuilder);
    }
            private static void ConfigurePersonnelEntities(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Driver>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Code).IsRequired().HasMaxLength(20);
                entity.HasIndex(e => e.Code).IsUnique();
                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.ExperienceYears).HasDefaultValue(0);
                entity.Property(e => e.Status).HasConversion<string>();
                entity.Property(e => e.IsActive).HasDefaultValue(true);
                entity.Property(e => e.CreatedAt).HasColumnType("datetime(6)")
                    .ValueGeneratedNever();
                entity.Property(e => e.UpdatedAt).HasColumnType("datetime(6)")
                    .ValueGeneratedNever();

                // Configure DriverLicense value object
                entity.OwnsOne(e => e.License, license =>
                {
                    license.Property(l => l.Number)
                          .HasColumnName("LicenseNumber")
                          .IsRequired()
                          .HasMaxLength(50);
                    license.Property(l => l.ExpiryDate)
                          .HasColumnName("LicenseExpiryDate")
                          .IsRequired();
                    license.HasIndex(l => l.Number).IsUnique();
                });

                // Configure ContactInformation value object
                entity.OwnsOne(e => e.ContactInfo, contact =>
                {
                    contact.Property(c => c.Phone)
                          .HasColumnName("Phone")
                          .HasMaxLength(15);
                    contact.Property(c => c.Email)
                          .HasColumnName("Email")
                          .HasMaxLength(255);
                });
            });
        }
}