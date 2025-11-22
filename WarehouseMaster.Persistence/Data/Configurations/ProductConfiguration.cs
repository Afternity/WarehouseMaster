using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WarehouseMaster.Domain.Models;

namespace WarehouseMaster.Persistence.Data.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(product => product.Id);

            builder.HasMany(product => product.Orders)
                   .WithOne(order => order.Product)
                   .HasForeignKey(order => order.ProductId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
