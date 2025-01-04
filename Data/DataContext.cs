using bageri.api.Entities;
using Microsoft.EntityFrameworkCore;

namespace bageri.api.Data
{
    public class DataContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Supplier> Suppliers {get; set; }
        public DbSet<SupplierProduct> SupplierProducts { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<ContactInformation> ContactInformations { get; set; }
        public DbSet<SupplierAddress> SupplierAddresses { get; set; }

        public DataContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SupplierProduct>().HasKey(o => new{o.ProductId, o.SupplierId});
            modelBuilder.Entity<ContactInformation>().HasKey(o => new{o.SupplierId});

            modelBuilder.Entity<ContactInformation>().Property(o => o.SupplierId).ValueGeneratedNever();

            modelBuilder.Entity<SupplierAddress>().HasKey(o => new{o.SupplierId, o.AddressId});
        }

    }
}