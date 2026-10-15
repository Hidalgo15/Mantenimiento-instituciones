using Mantenimiento.Core.Domain.Entities;
using Mantenimiento.Core.Domain.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace Mantenimiento.Infraestructure.Persistence.Repositories
{
    public class InstitucionRepository : IInstitucionRepository
    {
        private readonly ApplicationDbContext _context;

        public InstitucionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtiene todas las instituciones de la base de datos de manera asíncrona.
        /// </summary>
        /// <returns></returns>
        public async Task<List<Institucion>> ObtenerInstitucionesAsync()
        {
            return await _context.Instituciones
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
