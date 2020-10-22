using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MusicalInstrumentsShop.Domain.Entities;
using System;

namespace MusicalInstrumentsShop.DataAccess.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Shipping> Shippings { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Wishlist> Wishlists { get; set; }
        public DbSet<CreditCard> CreditCards { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<OrderProduct> OrderProducts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
