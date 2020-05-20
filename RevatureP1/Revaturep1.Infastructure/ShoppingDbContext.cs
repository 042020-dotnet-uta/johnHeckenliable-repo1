using Microsoft.EntityFrameworkCore;
using RevatureP1.Models;
using System;

namespace Revaturep1.DataAccess
{
    /// <summary>
    /// Context used to access the database
    /// </summary>
    public class ShoppingDbContext : DbContext
    {
        /// <summary>
        /// The Customers Table
        /// </summary>
        public DbSet<Customer> Customers { get; set; }
        /// <summary>
        /// The Products Table
        /// </summary>
        public DbSet<Product> Products { get; set; }
        /// <summary>
        /// The Orders Table
        /// </summary>
        public DbSet<Order> Orders { get; set; }
        /// <summary>
        /// The Order Details/Line Items Table
        /// </summary>
        public DbSet<OrderDetails> OrderDetails { get; set; }
        /// <summary>
        /// The Store Locations Table
        /// </summary>
        public DbSet<Store> Stores { get; set; }
        /// <summary>
        /// The Store Locatations Inventory Table
        /// </summary>
        public DbSet<Inventory> StoreInventories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderDetails>()
                .HasKey(c => new { c.ProductId, c.OrderId });

            modelBuilder.Entity<Inventory>()
                .HasKey(c => new { c.ProductId, c.StoreId });
        }

        public ShoppingDbContext(DbContextOptions<ShoppingDbContext> options)
            : base(options)
        {
        }
    }
}
