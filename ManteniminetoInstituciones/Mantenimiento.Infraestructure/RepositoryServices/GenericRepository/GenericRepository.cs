
using Mantenimiento.Core.Domain.RepositoryInterfaces.GenericRepositoryInterfaces;

namespace Mantenimiento.Infraestructure.Persistence.RepositoryServices.GenericRepository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        public Task<T> AddAsync(T entidad)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<T> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(T entidad)
        {
            throw new NotImplementedException();
        }
    }

}
