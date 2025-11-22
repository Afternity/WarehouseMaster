using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WarehouseMaster.Domain.Models;

namespace WarehouseMaster.Persistence.Data.Configurations
{
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.HasKey(employee => employee.Id);

            builder.HasMany(employee => employee.Orders)
                   .WithOne(order => order.Employee)
                   .HasForeignKey(order => order.EmployeeId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
