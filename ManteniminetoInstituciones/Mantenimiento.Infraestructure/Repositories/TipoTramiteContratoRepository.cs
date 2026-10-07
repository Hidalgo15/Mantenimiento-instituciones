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

        public async Task<List<TipoTramiteContrato>> GetAllTipoTramiteAsync()
        {
            return await _context.TipoTramiteContratos
                .FromSqlRaw("SELECT [id] AS Id, " +
                "[tipo_tramite] AS TipoTramite " +
                "FROM [dbo].[tipo_tramite_contrato]")
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<TipoTramiteContrato> GetTipoTramiteByIdAsync(int id)
        {
            var pId = new SqlParameter("@Id", id);

            var resultado = await _context.TipoTramiteContratos
                .FromSqlRaw("SELECT [id] AS Id, " +
                "[tipo_tramite] AS TipoTramite " +
                "FROM [dbo].[tipo_tramite_contrato] " +
                "WHERE [id] = @Id", pId)
                .AsNoTracking()
                .ToListAsync();

            return resultado.FirstOrDefault()!;
        }
    }
}
