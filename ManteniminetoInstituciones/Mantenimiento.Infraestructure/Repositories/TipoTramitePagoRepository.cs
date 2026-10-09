using Mantenimiento.Core.Domain.Entities;
using Mantenimiento.Core.Domain.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mantenimiento.Infraestructure.Persistence.Repositories
{
    public class TipoTramitePagoRepository : ITipoTramitePagoRepository
    {
        private readonly ApplicationDbContext _context;

        public TipoTramitePagoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<TipoTramitePago>> ObtenerTodosAsync()
        {
            return await _context.TipoTramitePago
                .AsNoTracking()
                .OrderBy(t => t.Tipo)
                .ToListAsync();
        }

        public async Task<TipoTramitePago?> ObtenerPorIdAsync(int id)
        {
            return await _context.TipoTramitePago
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == id);
        }
    }
}
