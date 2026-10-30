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
        /// Obtiene la lista de usuarios asociados a los moldes especificados desde la base de datos mediante un procedimiento almacenado.
        /// </summary>
        /// <param name="codigoTre"></param>
        /// <returns></returns>
        public async Task<List<UsuarioSubtipo>> ObtenerUsuariosPorMoldeAsync(int codigoTre)
        {
            var pTre = new SqlParameter("@CodigoTre", codigoTre);
            return await _context.Set<UsuarioSubtipo>()
                .FromSqlRaw("EXEC [SIGOB].[dbo].[sp_Obtener_Usuarios_Por_Molde] @CodigoTre", pTre)
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

        /// <summary>
        /// Obtiene la lista de usuarios asociados a una actividad específica desde la base de datos mediante un procedimiento almacenado.
        /// </summary>
        /// <param name="codigoActividad"></param>
        /// <param name="codigoTre"></param>
        /// <param name="filtroCargo"></param>
        /// <param name="filtroArea"></param>
        /// <param name="busquedaGeneral"></param>
        /// <returns></returns>
        public async Task<List<UsuarioActividadAsignacion>> ObtenerUsuariosPorActividadAsync(int codigoActividad, int codigoTre, string? filtroCargo, string? filtroArea, string? busquedaGeneral)
        {
            var pAct = new SqlParameter("@CodigoActividad", codigoActividad);
            var pTre = new SqlParameter("@CodigoTre", codigoTre);
            var pCargo = new SqlParameter("@FiltroCargo", (object?)filtroCargo ?? DBNull.Value);
            var pArea = new SqlParameter("@FiltroArea", (object?)filtroArea ?? DBNull.Value);
            var pBusqueda = new SqlParameter("@BusquedaGeneral", (object?)busquedaGeneral ?? DBNull.Value);

            return await _context.Set<UsuarioActividadAsignacion>()
                .FromSqlRaw("EXEC [SIGOB].[dbo].[sp_Obtener_Usuarios_Por_Actividad] @CodigoActividad, @CodigoTre, @FiltroCargo, @FiltroArea, @BusquedaGeneral", pAct, pTre, pCargo, pArea, pBusqueda)
                .AsNoTracking()
                .ToListAsync();
        }

        /// <summary>
        /// Gestiona la asignación de usuarios a una actividad específica mediante un procedimiento almacenado en la base de datos.
        /// </summary>
        /// <param name="codigoActividad"></param>
        /// <param name="listaCodigosDespa"></param>
        /// <returns></returns>
        public async Task<ResultadoOperacion> GestionarActividadUsuariosAsync(int codigoActividad, string? listaCodigosDespa)
        {
            var pAct = new SqlParameter("@CodigoActividad", codigoActividad);
            var pLista = new SqlParameter("@ListaCodigosDespa", (object?)listaCodigosDespa ?? DBNull.Value);

            var resultado = await _context.Set<ResultadoOperacion>()
                .FromSqlRaw("EXEC [SIGOB].[dbo].[sp_Gestionar_Actividad_Usuarios] @CodigoActividad, @ListaCodigosDespa", pAct, pLista)
                .AsNoTracking()
                .ToListAsync();

            return resultado.FirstOrDefault() ?? new ResultadoOperacion { CodigoResultado = 0, Mensaje = "Error desconocido al actualizar usuarios de la actividad." };
        }

        /// <summary>
        /// Obtiene la lista de flujos de trabajo asociados a un código de molde específico desde la base de datos mediante un procedimiento almacenado.
        /// </summary>
        /// <param name="codigoTre"></param>
        /// <returns></returns>
        public async Task<List<FlujoMolde>> ObtenerFlujosPorMoldeAsync(int codigoTre)
        {
            var pTre = new SqlParameter("@CodigoTre", codigoTre);
            return await _context.Set<FlujoMolde>()
                .FromSqlRaw("EXEC [SIGOB].[dbo].[sp_Obtener_Flujos_Por_Molde] @CodigoTre", pTre)
                .AsNoTracking()
                .ToListAsync();
        }

        /// <summary>
        /// Gestiona la asignación de flujos de trabajo a un molde específico mediante un procedimiento almacenado en la base de datos.
        /// </summary>
        /// <param name="codigoTre"></param>
        /// <param name="nombreFlujo"></param>
        /// <returns></returns>
        public async Task<List<ActividadFlujo>> ObtenerActividadesPorFlujoAsync(int codigoTre, string nombreFlujo)
        {
            var pTre = new SqlParameter("@CodigoTre", codigoTre);
            var pFlujo = new SqlParameter("@NombreFlujo", nombreFlujo);
            return await _context.Set<ActividadFlujo>()
                .FromSqlRaw("EXEC [SIGOB].[dbo].[sp_Obtener_Actividades_Por_Flujo] @CodigoTre, @NombreFlujo", pTre, pFlujo)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}