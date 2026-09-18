using Microsoft.EntityFrameworkCore;
using CustomerProduct.Models;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace CustomerProduct.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasColumnType("decimal(18,2)");
            //Burada decimal tipini SQL Server'a "18 basamak, 2'si ondalık şeklinde
        }
    }
}
