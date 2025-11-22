using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WarehouseMaster.Domain.Models;

namespace WarehouseMaster.Persistence.Data.Configurations
{
    public class PalletConfiguration : IEntityTypeConfiguration<Pallet>
    {
        public void Configure(EntityTypeBuilder<Pallet> builder)
        {
            builder.HasKey(pallet => pallet.Id);

            builder.HasMany(pallet => pallet.Orders)
                   .WithOne(order => order.Pallet)
                   .HasForeignKey(order => order.PalletId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
