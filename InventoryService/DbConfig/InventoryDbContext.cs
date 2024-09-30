using InventoryService.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryService.DbConfig;

public class InventoryDbContext : DbContext
{
    public DbSet<Inventory> Inventories { get; set; }
    public DbSet<Image> Images { get; set; }
    
    public InventoryDbContext(DbContextOptions<InventoryDbContext> options)
        : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // One-to-many relationship between Inventory and Image
        modelBuilder.Entity<Inventory>()
            .HasMany(i => i.Images)
            .WithOne(img => img.Inventory)
            .HasForeignKey(img => img.InventoryId);
    }
}