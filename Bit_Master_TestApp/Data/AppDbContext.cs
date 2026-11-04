using Bit_Master_TestApp.Models;
using Microsoft.EntityFrameworkCore;

namespace Bit_Master_TestApp.Data;

public class AppDbContext: DbContext
{
    public DbSet<Car> Cars { get; set; }
    public DbSet<Driver> Drivers { get; set; }
    public DbSet<Crew> Crews { get; set; }
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Crew>()
            .HasOne(c => c.Car)
            .WithMany()
            .HasForeignKey(c => c.CarId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Crew>()
            .HasOne(c => c.Driver)
            .WithMany()
            .HasForeignKey(c => c.DriverId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Crew>()
            .Property(c => c.Active)
            .HasDefaultValue(true);

        modelBuilder.Entity<Crew>()
            .HasIndex(c => new { c.CarId, c.DriverId })
            .IsUnique();
    }
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries<Crew>()
            .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = DateTime.UtcNow;
                entry.Entity.UpdatedAt = DateTime.UtcNow;
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Entity.UpdatedAt = DateTime.UtcNow;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}

