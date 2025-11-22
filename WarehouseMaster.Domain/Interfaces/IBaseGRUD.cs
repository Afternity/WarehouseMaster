namespace WarehouseMaster.Domain.Interfaces
{
    public interface IBaseGRUD<T>
    {
        Task CreateAsync(T model);
        Task UpdateAsync(T model);
        Task DeleteAsync(T model);
        Task GetAllAsync();
    }
}
