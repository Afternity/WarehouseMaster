using WarehouseMaster.Domain.Models.BaseModels;

namespace WarehouseMaster.Domain.Models
{
    public class Pallet
        : BaseModel
    {
        public string Name { get; set; } = string.Empty;
        public string Barcode { get; set; } = string.Empty;
        public decimal Weight { get; set; } = decimal.Zero;
        public decimal Length { get; set; } = decimal.Zero;

        public virtual ICollection<Order> Orders { get; set; } = [];
    }
}
