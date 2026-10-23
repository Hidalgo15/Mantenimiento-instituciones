using Mantenimiento.Core.Domain.Entities;
using Mantenimiento.Core.Domain.Entities.ResponsableActividad;
using Mantenimiento.Core.Domain.RepositoryInterfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Mantenimiento.Infraestructure.Persistence.Repositories
{
    public class ResponsableActividadRepository : IResponsableActividadRepository
    {
        private readonly ApplicationDbContext _context;

        public ResponsableActividadRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtiene la lista de catálogos de moldes desde la base de datos mediante un procedimiento almacenado.
        /// </summary>
        /// <returns></returns>
        public async Task<List<MoldeCatalogo>> ObtenerCatalogosMoldesAsync()
        {
            return await _context.Set<MoldeCatalogo>()
                .FromSqlRaw("EXEC [SIGOB].[dbo].[sp_Obtener_Catalogo_Moldes]")
                .AsNoTracking()
                .ToListAsync();
        }

        /// <summary>
        /// Obtiene la lista de subtipos de moldes asociados a un código de molde específico desde la base de datos mediante un procedimiento almacenado.
        /// </summary>
        /// <param name="codigoTre"></param>
        /// <returns></returns>
        public async Task<List<SubtipoMolde>> ObtenerSubtiposPorMoldeAsync(int codigoTre)
        {
            var pTre = new SqlParameter("@CodigoTre", codigoTre);
            return await _context.Set<SubtipoMolde>()
                .FromSqlRaw("EXEC [SIGOB].[dbo].[sp_Obtener_Subtipos_Por_Molde] @CodigoTre", pTre)
                .AsNoTracking()
                .ToListAsync();
        }

        /// <summary>
        /// Obtiene la lista de usuarios asociados a los subtipos de moldes especificados desde la base de datos mediante un procedimiento almacenado.
        /// </summary>
        /// <param name="codigosSubtipos"></param>
        /// <returns></returns>
        public async Task<List<UsuarioSubtipo>> ObtenerUsuariosPorSubtiposAsync(string codigosSubtipos)
        {
            var pSubtipos = new SqlParameter("@CodigosSubtipos", codigosSubtipos);
            return await _context.Set<UsuarioSubtipo>()
                .FromSqlRaw("EXEC [SIGOB].[dbo].[sp_Obtener_Usuarios_Por_Subtipos] @CodigosSubtipos", pSubtipos)
                .AsNoTracking()
                .ToListAsync();
        }

        /// <summary>
        /// Obtiene la lista de actividades asociadas a los subtipos de moldes especificados desde la base de datos mediante un procedimiento almacenado.
        /// </summary>
        /// <param name="codigosSubtipos"></param>
        /// <returns></returns>
        public async Task<List<ActividadSubtipo>> ObtenerActividadesPorSubtiposAsync(string codigosSubtipos)
        {
            var pSubtipos = new SqlParameter("@CodigosSubtipos", codigosSubtipos);
            return await _context.Set<ActividadSubtipo>()
                .FromSqlRaw("EXEC [SIGOB].[dbo].[sp_Obtener_Actividades_Por_Subtipos] @CodigosSubtipos", pSubtipos)
                .AsNoTracking()
                .ToListAsync();
        }

        /// <summary>
        /// Obtiene la lista de actividades asociadas a un usuario específico desde la base de datos mediante un procedimiento almacenado.
        /// </summary>
        /// <param name="codigoDespa"></param>
        /// <returns></returns>
        public async Task<List<ResponsableActividad>> ObtenerActividadesUsuarioAsync(string codigoDespa)
        {
            var pDespa = new SqlParameter("@CodigoDespa", codigoDespa);
            return await _context.Set<ResponsableActividad>()
                .FromSqlRaw("EXEC [SIGOB].[dbo].[sp_Obtener_Actividades_Usuario] @CodigoDespa", pDespa)
                .AsNoTracking()
                .ToListAsync();
        }

        /// <summary>
        /// Gestiona las actividades asociadas a un usuario específico mediante un procedimiento almacenado en la base de datos.
        /// </summary>
        /// <param name="codigoDespa"></param>
        /// <param name="listaCodigosActividades"></param>
        /// <returns></returns>
        public async Task<ResultadoOperacion> GestionarActividadesAsync(string codigoDespa, string? listaCodigosActividades)
        {
            var pDespa = new SqlParameter("@CodigoDespa", codigoDespa);
            var pLista = new SqlParameter("@ListaCodigosActividades", (object?)listaCodigosActividades ?? DBNull.Value);

            var resultado = await _context.Set<ResultadoOperacion>()
                .FromSqlRaw("EXEC [SIGOB].[dbo].[sp_Gestionar_Responsable_Actividades] @CodigoDespa, @ListaCodigosActividades", pDespa, pLista)
                .AsNoTracking()
                .ToListAsync();

            return resultado.FirstOrDefault() ?? new ResultadoOperacion { CodigoResultado = 0, Mensaje = "Error desconocido al actualizar actividades." };
        }
    }
}