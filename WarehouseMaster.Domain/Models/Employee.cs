using WarehouseMaster.Domain.Models.BaseModels;

namespace WarehouseMaster.Domain.Models
{
    public class Employee
        : BaseModel
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        
        public virtual ICollection<Order> Orders { get; set; } = [];
    }
}
