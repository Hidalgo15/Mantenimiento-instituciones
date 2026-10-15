using Mantenimiento.Core.Domain.Entities;
using Mantenimiento.Core.Domain.RepositoryInterfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Mantenimiento.Infraestructure.Persistence.Repositories
{
    public class TipoTramitePagoRepository : ITipoTramitePagoRepository
    {
        private readonly ApplicationDbContext _context;

        public TipoTramitePagoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtiene todos los registros de TipoTramitePago desde la base de datos utilizando un procedimiento almacenado.
        /// </summary>
        /// <returns></returns>
        public async Task<List<TipoTramitePago>> ObtenerTodosAsync()
        {
            return await _context.TipoTramitePago
                .FromSqlRaw("EXEC dbo.sp_TipoTramitePago_Obtener")
                .AsNoTracking()
                .ToListAsync();
        }

        /// <summary>
        /// Obtiene de forma asíncrona un TipoTramitePago por su identificador.
        /// </summary>
        /// <remarks>Ejecuta el procedimiento almacenado dbo.sp_TipoTramitePago_Obtener mediante
        /// FromSqlRaw y devuelve el primer resultado sin seguimiento (AsNoTracking).</remarks>
        /// <param name="id">Identificador del TipoTramitePago a recuperar.</param>
        /// <returns>Tarea que devuelve el TipoTramitePago correspondiente al identificador, o null si no se encuentra.</returns>
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
