
using Mantenimiento.Core.Domain.RepositoryInterfaces.GenericRepositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace Mantenimiento.Infraestructure.Persistence.RepositoryServices.GenericRepository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            // AsNoTracking() optimiza el rendimiento en operaciones de solo lectura
            return await _dbSet.AsNoTracking().ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            // FindAsync busca directamente por la clave primaria
            return await _dbSet.FindAsync(id);
        }

        public async Task<T> AddAsync(T entidad)
        {
            await _dbSet.AddAsync(entidad);
            await _context.SaveChangesAsync();
            return entidad;
        }

        public async Task UpdateAsync(T entidad)
        {
            _dbSet.Update(entidad);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entidad = await GetByIdAsync(id);
            if (entidad != null)
            {
                _dbSet.Remove(entidad);
                await _context.SaveChangesAsync();
            }
        }
    }

}
