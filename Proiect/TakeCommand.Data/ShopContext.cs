using Microsoft.EntityFrameworkCore;
using TakeCommand.Data.Models;

namespace TakeCommand.Data
{
    public class ShopContext : DbContext
    {

        public ShopContext(DbContextOptions<ShopContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderProducts> OrderProducts { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("Shop");

            modelBuilder.Entity<Product>().ToTable("Product").HasKey(s => s.Id);

            modelBuilder.Entity<Order>().ToTable("Order").HasKey(s => s.Id);

            modelBuilder.Entity<OrderProducts>().ToTable("Order_Product").HasKey(op => new { op.OrderId, op.ProductId });
            
        }

    }
}
