using Mantenimiento.Core.Domain.Entities;
using Mantenimiento.Core.Domain.RepositoryInterfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
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
                .FromSqlRaw("EXEC dbo.sp_TipoTramitePago_Obtener")
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<TipoTramitePago?> ObtenerPorIdAsync(int id)
        {
            var pId = new SqlParameter("@id", SqlDbType.Int) { Value = id };

            var lista = await _context.TipoTramitePago
                .FromSqlRaw("EXEC dbo.sp_TipoTramitePago_Obtener @id = @id", pId)
                .AsNoTracking()
                .ToListAsync();

            return lista.FirstOrDefault();
        }
    }
}
