
namespace Mantenimiento.Core.Domain.RepositoryInterfaces.GenericRepositoryInterfaces
{
    public interface IGenericRepository <T> where T : class
    {
        Task<T> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> AddAsync(T entidad);
        Task UpdateAsync(T entidad);
        Task DeleteAsync(int id);
    }
}
