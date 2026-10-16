using Mantenimiento.Core.Domain.Entities;
using Mantenimiento.Core.Domain.RepositoryInterfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Mantenimiento.Infraestructure.Persistence.Repositories
{
    public class CuentaMapeoRepository : ICuentaMapeoRepository
    {

        private readonly ApplicationDbContext _context;

        public CuentaMapeoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtiene de forma asíncrona las cuentas que coinciden con los criterios especificados.
        /// </summary>
        /// <remarks>Ejecuta el procedimiento almacenado dbo.sp_Cuentas_Obtener con @id_cuenta = NULL y
        /// los parámetros @tipo_tramite y @busqueda; la consulta se realiza sin seguimiento (AsNoTracking).</remarks>
        /// <param name="tipoTramite">Filtro opcional por tipo de trámite; si es null se omite el filtro.</param>
        /// <param name="busqueda">Término de búsqueda opcional para filtrar cuentas; si es null se omite el filtro.</param>
        /// <returns>Tarea que representa la operación asíncrona cuyo resultado es una lista de CuentaDbo que cumplen los
        /// criterios.</returns>
        public async Task<List<CuentaDbo>> ObtenerCuentasAsync(int? tipoTramite = null, string? busqueda = null)
        {
            var pId = new SqlParameter("@id_cuenta", SqlDbType.Int) { Value = DBNull.Value };
            var pTipo = new SqlParameter("@tipo_tramite", SqlDbType.Int) { Value = (object?)tipoTramite ?? DBNull.Value };
            var pBusqueda = new SqlParameter("@busqueda", SqlDbType.NVarChar, 255) { Value = (object?)busqueda ?? DBNull.Value };

            return await _context.TipoCuentaDbo
                .FromSqlRaw("EXEC dbo.sp_Cuentas_Obtener @id_cuenta = @id_cuenta, @tipo_tramite = @tipo_tramite, @busqueda = @busqueda", pId, pTipo, pBusqueda)
                .AsNoTracking()
                .ToListAsync();
        }

        /// <summary>
        /// Obtiene de forma asincrónica las cuentas base del catálogo desde el contexto de datos, sin seguimiento y
        /// ordenadas por CodigoCuenta.
        /// </summary>
        /// <remarks>La consulta usa AsNoTracking() y ordena los resultados por CodigoCuenta.</remarks>
        /// <returns>Una tarea que representa la operación asincrónica que devuelve una lista de CuentaCt.</returns>
        public async Task<List<CuentaCt>> ObtenerCuentasBaseCatalogAsync()
        {
            return await _context.TipoCuentaCt
                .AsNoTracking()
                .OrderBy(c => c.CodigoCuenta)
                .ToListAsync();
        }

        /// <summary>
        /// Obtiene de forma asíncrona la cuenta con el identificador especificado, o null si no existe.
        /// </summary>
        /// <remarks>Ejecuta el procedimiento almacenado dbo.sp_Cuentas_Obtener con los parámetros
        /// correspondientes y devuelve el primer registro; la consulta se realiza con AsNoTracking.</remarks>
        /// <param name="idCuenta">Identificador de la cuenta a recuperar.</param>
        /// <returns>Una tarea que representa la operación asíncrona. El resultado contiene la instancia de CuentaDbo si se
        /// encuentra; en caso contrario, null.</returns>
        public async Task<CuentaDbo?> ObtenerPorIdAsync(int idCuenta)
        {
            var pId = new SqlParameter("@id_cuenta", SqlDbType.Int) { Value = idCuenta };
            var pTipo = new SqlParameter("@tipo_tramite", SqlDbType.Int) { Value = DBNull.Value };
            var pBusqueda = new SqlParameter("@busqueda", SqlDbType.NVarChar, 255) { Value = DBNull.Value };

            var resultado = await _context.TipoCuentaDbo
                .FromSqlRaw("EXEC dbo.sp_Cuentas_Obtener @id_cuenta = @id_cuenta, @tipo_tramite = @tipo_tramite, @busqueda = @busqueda", pId, pTipo, pBusqueda)
                .AsNoTracking()
                .ToListAsync();

            return resultado.FirstOrDefault();
        }

        /// <summary>
        /// Crea una nueva cuenta en la base de datos ejecutando el procedimiento almacenado dbo.sp_Cuentas_Insertar con los parámetros proporcionados.
        /// </summary>
        /// <param name="idCuentaBase"></param>
        /// <param name="descripcion"></param>
        /// <param name="tipoTramite"></param>
        /// <returns></returns>
        public async Task CrearCuentaAsync(int idCuentaBase, string? descripcion, int tipoTramite)
        {
            var pIdBase = new SqlParameter("@id_cuenta", idCuentaBase);
            var pDesc = new SqlParameter("@descripcion_cuenta", (object?)descripcion ?? DBNull.Value);
            var pTipo = new SqlParameter("@tipo_tramite", tipoTramite);

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC dbo.sp_Cuentas_Insertar @id_cuenta, @descripcion_cuenta, @tipo_tramite",
                pIdBase, pDesc, pTipo);
        }

        /// <summary>
        /// Actualiza los datos de una cuenta ejecutando el procedimiento almacenado dbo.sp_Cuentas_Actualizar.
        /// </summary>
        /// <remarks>Ejecuta SQL crudo a través de ExecuteSqlRawAsync pasando parámetros; puede lanzar
        /// excepciones de acceso a datos (por ejemplo, SqlException o DbUpdateException) que deben manejarse en el
        /// llamador.</remarks>
        /// <param name="idCuenta">Identificador de la cuenta a actualizar.</param>
        /// <param name="descripcion">Nueva descripción de la cuenta.</param>
        /// <param name="tipoTramite">Tipo de trámite asociado a la cuenta.</param>
        /// <returns>Una tarea que representa la operación asincrónica de actualización.</returns>
        public async Task ActualizarCuentaAsync(int idCuenta, string descripcion, int tipoTramite)
        {
            var pId = new SqlParameter("@id_cuenta", idCuenta);
            var pDesc = new SqlParameter("@descripcion_cuenta", descripcion);
            var pTipo = new SqlParameter("@tipo_tramite", tipoTramite);

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC dbo.sp_Cuentas_Actualizar @id_cuenta, @descripcion_cuenta, @tipo_tramite",
                pId, pDesc, pTipo);
        }

        /// <summary>
        /// Elimina la cuenta especificada ejecutando el procedimiento almacenado dbo.sp_Cuentas_Eliminar.
        /// </summary>
        /// <remarks>Ejecuta un procedimiento almacenado en la base de datos; puede lanzar excepciones en
        /// caso de error de ejecución SQL.</remarks>
        /// <param name="idCuenta">Identificador de la cuenta que se eliminará.</param>
        /// <returns>Tarea que representa la operación asincrónica que se completa cuando la eliminación finaliza.</returns>
        public async Task EliminarCuentaAsync(int idCuenta)
        {
            var pId = new SqlParameter("@id_cuenta", idCuenta);
            await _context.Database.ExecuteSqlRawAsync(
                "EXEC dbo.sp_Cuentas_Eliminar @id_cuenta", pId);
        }
    }
}
