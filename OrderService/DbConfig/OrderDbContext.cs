using Microsoft.EntityFrameworkCore;
using OrderService.Models;

namespace OrderService.DbConfig;

public class OrderDbContext : DbContext
{
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    
    public OrderDbContext(DbContextOptions<OrderDbContext> options)
        : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure relationships if needed
        modelBuilder.Entity<Order>()
            .HasMany(o => o.OrderItems)
            .WithOne(oi => oi.Order)
            .HasForeignKey(oi => oi.OrderId);
        
    }
}