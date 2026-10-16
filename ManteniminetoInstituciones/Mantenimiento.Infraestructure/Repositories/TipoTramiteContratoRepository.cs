using Mantenimiento.Core.Domain.Entities;
using Mantenimiento.Core.Domain.RepositoryInterfaces;
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

        /// <summary>
        /// Obtiene todos los tipos de trámite de contrato de la base de datos de manera asíncrona.
        /// </summary>
        /// <returns></returns>
        public async Task<List<TramiteContrato>> GetAllTipoTramiteAsync()
        {
            return await _context.TipoTramiteContratos
                .AsNoTracking()
                .ToListAsync();
        }

        /// <summary>
        /// Obtiene de forma asíncrona un TramiteContrato por su identificador.
        /// </summary>
        /// <remarks>La consulta se realiza con AsNoTracking para no registrar la entidad en el Change
        /// Tracker.</remarks>
        /// <param name="id">Identificador del trámite a recuperar.</param>
        /// <returns>Tarea que representa la operación asíncrona que devuelve el TramiteContrato con el identificador
        /// especificado, o null si no existe.</returns>
        public async Task<TramiteContrato> GetTipoTramiteByIdAsync(int id)
        {
            return (await _context.TipoTramiteContratos
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == id))!;
        }
    }
}
