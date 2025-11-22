using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WarehouseMaster.Domain.Models;

namespace WarehouseMaster.Persistence.Data.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(order => order.Id);

            builder.HasOne(order => order.Pallet)
                   .WithMany(pallet => pallet.Orders)
                   .HasForeignKey(order => order.PalletId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(order => order.Product)
                   .WithMany(product => product.Orders)
                   .HasForeignKey(order => order.ProductId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(order => order.Employee)
                   .WithMany(employee => employee.Orders)
                   .HasForeignKey(order => order.EmployeeId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(order => order.Location)
                   .WithMany(location => location.Orders)
                   .HasForeignKey(order => order.LocationId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(order => order.Supplier)
                   .WithMany(supplier => supplier.Orders)
                   .HasForeignKey(order => order.SupplierId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
