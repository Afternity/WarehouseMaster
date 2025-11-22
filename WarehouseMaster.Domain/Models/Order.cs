using WarehouseMaster.Domain.Models.BaseModels;

namespace WarehouseMaster.Domain.Models
{
    public class Order
        : BaseModel
    {
        public DateTime DateTime { get; set; } = DateTime.UtcNow;
        public int ProductCount { get; set; } = 0;
        public int PalletCount { get; set; } = 0;
        public decimal TotalPrice { get; set; } = decimal.Zero;

        public Guid PalletId { get; set; }
        public virtual Pallet Pallet { get; set; } = null!;
        public Guid ProductId { get; set; }
        public virtual Product Product { get; set; } = null!;
        public Guid EmployeeId { get; set; }
        public virtual Employee Employee { get; set; } = null!;
        public Guid LocationId { get; set; }
        public virtual Location Location { get; set; } = null!;
        public Guid SupplierId { get; set; }
        public virtual Supplier Supplier { get; set; } = null!;
    }
}
