using WarehouseMaster.Domain.Models.BaseModels;

namespace WarehouseMaster.Domain.Models
{
    public class Supplier 
        : BaseModel
    {
        public string CompanyName { get; set; } = string.Empty;
        public string ContactPerson { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;

        public virtual ICollection<Order> Orders { get; set; } = [];
    }
}

