using WarehouseMaster.Domain.Models.BaseModels;

namespace WarehouseMaster.Domain.Models
{
    public class Location
        : BaseModel
    {
        public string Name { get; set; } = string.Empty; 
        public string Code { get; set; } = string.Empty;
        public decimal Width { get; set; } = decimal.Zero;
        public decimal Length { get; set; } = decimal.Zero;
        public decimal Height { get; set; } = decimal.Zero;

        public virtual ICollection<Order> Orders { get; set; } = [];
    }
}
