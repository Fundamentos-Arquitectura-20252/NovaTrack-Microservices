using IAM.API.Domain.Model.Aggregates;
using Microsoft.EntityFrameworkCore;
namespace IAM.API.Infrastructure.Persistence;

public class IAMDbContext : DbContext
{
    public IAMDbContext(DbContextOptions<IAMDbContext> options) : base(options) { }
    
    public DbSet<User> Users { get; set; } = null!;
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure FleetManagement entities
        ConfigureIAMEntities(modelBuilder);
        
    }
    
    private static void ConfigureIAMEntities(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
            entity.HasIndex(e => e.Email).IsUnique();
            entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.PasswordHash).IsRequired();
            entity.Property(e => e.Role).IsRequired().HasMaxLength(50);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.CreatedAt).HasColumnType("datetime(6)")
                .ValueGeneratedNever();
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime(6)")
                .ValueGeneratedNever();
        });
    }
    
}