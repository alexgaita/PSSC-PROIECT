using Microsoft.EntityFrameworkCore;
using TakeCommand.Data.Models;

namespace TakeCommand.Data
{
    public class ShopContext : DbContext
    {

        public ShopContext(DbContextOptions<ShopContext> options) : base(options)
        {
        }

        public DbSet<ProductDto> Products { get; set; }

        public DbSet<OrderDto> Orders { get; set; }

        public DbSet<OrderProductsDto> OrderProducts { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("Shop");

            modelBuilder.Entity<ProductDto>().ToTable("Product").HasKey(s => s.Id);

            modelBuilder.Entity<OrderDto>().ToTable("Order").HasKey(s => s.Id);

            modelBuilder.Entity<OrderProductsDto>().ToTable("Order_Product").HasKey(op => new { op.OrderId, op.ProductId });
            
        }

    }
}
