using bageri.api.Entities;
using Microsoft.EntityFrameworkCore;

namespace bageri.api.Data
{
    public class DataContext : DbContext
    {
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<Supplier> Suppliers {get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<ContactInformation> ContactInformations { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<AddressType> AddressTypes { get; set; }
        public DbSet<PostalAddress> PostalAddresses { get; set; }
        public DbSet<SupplierAddress> SupplierAddresses { get; set; }
        public DbSet<SupplierIngredient> SupplierIngredients { get; set; }
        public DbSet<SupplierContactInformation> SupplierContactInformations { get; set; }
        public DbSet<CustomerContactInformation> CustomerContactInformations { get; set; }
        public DbSet<CustomerAddress> CustomerAddresses { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductPreparation> ProductPreparations { get; set; }
        public DbSet<CustomerOrder> CustomerOrders { get; set; }
        public DbSet<OrderProduct> OrderProducts { get; set; }
        

        public DataContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SupplierIngredient>().HasKey(o => new{o.IngredientId, o.SupplierId});
            modelBuilder.Entity<ContactInformation>().HasKey(o => new {o.ContactInformationId});
            modelBuilder.Entity<SupplierContactInformation>().HasKey(o => new{o.ContactInformationId, o.SupplierId});
            modelBuilder.Entity<CustomerContactInformation>().HasKey(o => new{o.CustomerId, o.ContactInformationId});
            modelBuilder.Entity<SupplierAddress>().HasKey(o => new{o.SupplierId, o.AddressId});
            modelBuilder.Entity<CustomerAddress>().HasKey(o => new{o.CustomerId, o.AddressId});
            modelBuilder.Entity<CustomerOrder>().HasKey(o => new{ o.OrderId, o.CustomerId});
            modelBuilder.Entity<OrderProduct>().HasKey(o => new{ o.OrderId, o.ProductId});

        }

    }
}