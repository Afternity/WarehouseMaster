using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WarehouseMaster.Domain.Models;

namespace WarehouseMaster.Persistence.Data.Configurations
{
    public class SupplierConfiguration : IEntityTypeConfiguration<Supplier>
    {
        public void Configure(EntityTypeBuilder<Supplier> builder)
        {
            builder.HasKey(supplier => supplier.Id);

            builder.HasMany(supplier => supplier.Orders)
                   .WithOne(order => order.Supplier)
                   .HasForeignKey(order => order.SupplierId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
