using Mantenimiento.Core.Domain.Entities;
using Mantenimiento.Core.Domain.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace Mantenimiento.Infraestructure.Persistence.Repositories
{
    public class CategoriaTramiteRepository : ICategoriaTramiteRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoriaTramiteRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CategoriaTramite>> ObtenerTodasAsync()
        {
            // Consultar directamente la vista v_categoria_tramite
            return await _context.Database
                .SqlQueryRaw<CategoriaTramite>("SELECT codigo_categoria AS CodigoCategoria, descripcion AS Descripcion FROM [SIGOB].[dbo].[v_categoria_tramite]")
                .AsNoTracking()
                .ToListAsync();
        }
    }
}