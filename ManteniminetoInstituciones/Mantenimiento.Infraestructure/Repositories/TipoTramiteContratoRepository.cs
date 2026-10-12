using Mantenimiento.Core.Domain.Entities;
using Mantenimiento.Core.Domain.RepositoryInterfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Mantenimiento.Infraestructure.Persistence.Repositories
{
    public class TipoTramiteContratoRepository : ITipoTramiteRepository
    {
        private readonly ApplicationDbContext _context;

        public TipoTramiteContratoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<TramiteContrato>> GetAllTipoTramiteAsync()
        {
            return await _context.TipoTramiteContratos
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<TramiteContrato> GetTipoTramiteByIdAsync(int id)
        {
            return (await _context.TipoTramiteContratos
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == id))!;
        }
    }
}
