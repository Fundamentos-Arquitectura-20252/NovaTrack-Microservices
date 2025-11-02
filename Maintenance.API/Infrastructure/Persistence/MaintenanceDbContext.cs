using System.Text.Json;
using Maintenance.API.Domain.Model.Aggregates;
using Maintenance.API.Domain.Model.ValueObjects;
using Microsoft.EntityFrameworkCore;
namespace Maintenance.API.Infrastructure.Persistence;

public class MaintenanceDbContext : DbContext
{
    public MaintenanceDbContext(DbContextOptions<MaintenanceDbContext> options) : base(options) { }
    
    public DbSet<MaintenanceRecord> MaintenanceRecords { get; set; } = null!;
    public DbSet<ServiceRecord> ServiceRecords { get; set; } = null!;
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        ConfigureMaintenanceEntities(modelBuilder);
    }
    
            private static void ConfigureMaintenanceEntities(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MaintenanceRecord>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.VehicleId).IsRequired();
                entity.Property(e => e.Description).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Type).HasConversion<string>();
                entity.Property(e => e.Status).HasConversion<string>();
                entity.Property(e => e.Notes).HasMaxLength(500);
                entity.Property(e => e.ServiceProvider).HasMaxLength(200);
                entity.Property(e => e.CreatedAt).HasColumnType("datetime(6)")
                    .ValueGeneratedNever();
                entity.Property(e => e.UpdatedAt).HasColumnType("datetime(6)")
                    .ValueGeneratedNever();

                // Configure MaintenanceCost value object
                entity.OwnsOne(e => e.Cost, cost =>
                {
                    cost.Property(c => c.EstimatedAmount)
                        .HasColumnName("EstimatedCost")
                        .HasColumnType("decimal(10,2)")
                        .IsRequired();
                    cost.Property(c => c.ActualAmount)
                        .HasColumnName("ActualCost")
                        .HasColumnType("decimal(10,2)");
                    cost.Property(c => c.Currency)
                        .HasColumnName("Currency")
                        .HasMaxLength(3)
                        .HasDefaultValue("PEN");
                });

                // Configure MaintenanceSchedule value object
                entity.OwnsOne(e => e.Schedule, schedule =>
                {
                    schedule.Property(s => s.ScheduledDate)
                           .HasColumnName("ScheduledDate")
                           .IsRequired();
                    schedule.Property(s => s.CompletedDate)
                           .HasColumnName("CompletedDate");
                    schedule.Property(s => s.Priority)
                           .HasColumnName("Priority")
                           .HasDefaultValue(3);
                });
            });

            modelBuilder.Entity<ServiceRecord>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.VehicleId).IsRequired();
                entity.Property(e => e.ServiceType).HasConversion<string>();
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.ServiceDate).IsRequired();
                entity.Property(e => e.MileageAtService).IsRequired();
                entity.Property(e => e.ServiceProvider).IsRequired().HasMaxLength(200);
                entity.Property(e => e.TechnicianName).HasMaxLength(100);
                entity.Property(e => e.Quality).HasConversion<string>();
                entity.Property(e => e.PartsUsed).HasMaxLength(1000);
                entity.Property(e => e.Notes).HasMaxLength(500);
                entity.Property(e => e.CreatedAt).HasColumnType("datetime(6)")
                    .ValueGeneratedNever();

                // Configure ServiceCost value object
                entity.OwnsOne(e => e.Cost, cost =>
                {
                    cost.Property(c => c.Amount)
                        .HasColumnName("Cost")
                        .HasColumnType("decimal(10,2)")
                        .IsRequired();
                    cost.Property(c => c.Currency)
                        .HasColumnName("Currency")
                        .HasMaxLength(3)
                        .HasDefaultValue("PEN");
                    cost.Property(c => c.Type)
                        .HasConversion<string>();


                    // Guardar los CostItems como JSON
                    cost.Property(c => c.Items)
                        .HasConversion(
                            v => JsonSerializer.Serialize(v, new JsonSerializerOptions()),
                            v => JsonSerializer.Deserialize<List<CostItem>>(v, new JsonSerializerOptions())!
                        )
                        .HasColumnName("CostItems")
                        .HasColumnType("json");
                });
            });
        }
}