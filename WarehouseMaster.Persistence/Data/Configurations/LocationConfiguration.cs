using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WarehouseMaster.Domain.Models;

namespace WarehouseMaster.Persistence.Data.Configurations
{
    internal class LocationConfiguration : IEntityTypeConfiguration<Location>
    {
        public void Configure(EntityTypeBuilder<Location> builder)
        {
            builder.HasKey(location => location.Id);

            builder.HasMany(location => location.Orders)
                   .WithOne(order => order.Location)
                   .HasForeignKey(order => order.LocationId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
