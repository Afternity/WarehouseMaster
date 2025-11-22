using WarehouseMaster.Domain.Models.BaseModels;

namespace WarehouseMaster.Domain.Models
{
    public class Product 
        : BaseModel
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; } = decimal.Zero;
        public string Type { get; set; } = string.Empty; 

        public virtual ICollection<Order> Orders { get; set; } = [];
    }
}
