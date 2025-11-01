using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using FleetManagement.API.Domain.Model.Aggregates;

namespace FleetManagement.API.Infrastructure.Persistence;

public class FleetDbContext : DbContext
{
    public FleetDbContext(DbContextOptions<FleetDbContext> options) : base(options) { }
    public DbSet<Fleet> Fleets { get; set; } = null!;
    public DbSet<Vehicle> Vehicles { get; set; } = null!;
    
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure FleetManagement entities
        ConfigureFleetManagementEntities(modelBuilder);
        
    }
    
     private static void ConfigureFleetManagementEntities(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Fleet>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Code).IsRequired().HasMaxLength(20);
                entity.HasIndex(e => e.Code).IsUnique();
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.Type).HasConversion<string>();
                entity.Property(e => e.IsActive).HasDefaultValue(true);
                entity.Property(e => e.CreatedAt).HasColumnType("datetime(6)")
                    .ValueGeneratedNever();
                entity.Property(e => e.UpdatedAt).HasColumnType("datetime(6)")
                    .ValueGeneratedNever();

                entity.HasMany(e => e.Vehicles)
                      .WithOne(v => v.Fleet)
                      .HasForeignKey(v => v.FleetId)
                      .OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<Vehicle>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Brand).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Model).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Year).IsRequired();
                entity.Property(e => e.Mileage).HasDefaultValue(0);
                entity.Property(e => e.Status).HasConversion<string>();
                entity.Property(e => e.IsActive).HasDefaultValue(true);
                entity.Property(e => e.CreatedAt).HasColumnType("datetime(6)")
                    .ValueGeneratedNever();
                entity.Property(e => e.UpdatedAt).HasColumnType("datetime(6)")
                    .ValueGeneratedNever();

                // Configure LicensePlate value object
                entity.OwnsOne(e => e.LicensePlate, licensePlate =>
                {
                    licensePlate.Property(lp => lp.Value)
                               .HasColumnName("LicensePlate")
                               .IsRequired()
                               .HasMaxLength(20);
                    licensePlate.HasIndex(lp => lp.Value).IsUnique();
                });
            });
        }
    
}