using Microsoft.EntityFrameworkCore;
using WarehouseMaster.Domain.Models;
using WarehouseMaster.Persistence.Data.Configurations;

namespace WarehouseMaster.Persistence.Data.DbContexts
{
    public class WarehouseMasterDbContext
        : DbContext
    {
        public WarehouseMasterDbContext(
            DbContextOptions<WarehouseMasterDbContext> options)
            : base(options)
        {
        }

        public WarehouseMasterDbContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=AFTERNITY;Initial Catalog=WarehouseMasterDB;Integrated Security=True;Trust Server Certificate=True");
            }
        }

        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Pallet> Pallets { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Supplier> Suppliers { get; set; }

        protected override void OnModelCreating(
            ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new EmployeeConfiguration());
            modelBuilder.ApplyConfiguration(new LocationConfiguration());
            modelBuilder.ApplyConfiguration(new OrderConfiguration());
            modelBuilder.ApplyConfiguration(new PalletConfiguration());
            modelBuilder.ApplyConfiguration(new ProductConfiguration());
            modelBuilder.ApplyConfiguration(new SupplierConfiguration());
        }
    }
}
