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
    public class CuentaCtRepository : ICuentaCtRepository
    {
        private readonly ApplicationDbContext _context;

        public CuentaCtRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtiene de forma asincrónica las cuentas disponibles, opcionalmente filtradas por una cadena de búsqueda.
        /// </summary>
        /// <remarks>Ejecuta el procedimiento almacenado ct.sp_CuentasCt_Obtener con @CuentaID = NULL y
        /// @busqueda; la consulta se realiza sin seguimiento de cambios (AsNoTracking).</remarks>
        /// <param name="busqueda">Cadena de búsqueda opcional para filtrar las cuentas; si es null se devuelven todas.</param>
        /// <returns>Tarea asincrónica cuyo resultado es la lista de CuentaCt obtenida.</returns>
        public async Task<List<CuentaCt>> ObtenerCuentasAsync(string? busqueda = null)
        {
            var pCuentaId = new SqlParameter("@CuentaID", SqlDbType.Int) { Value = DBNull.Value };
            var pBusqueda = new SqlParameter("@busqueda", SqlDbType.NVarChar, 255) { Value = (object?)busqueda ?? DBNull.Value };

            return await _context.TipoCuentaCt
                .FromSqlRaw("EXEC ct.sp_CuentasCt_Obtener @CuentaID = @CuentaID, @busqueda = @busqueda", pCuentaId, pBusqueda)
                .AsNoTracking()
                .ToListAsync();
        }

        /// <summary>
        /// Obtiene de forma asíncrona una entidad CuentaCt por su identificador.
        /// </summary>
        /// <remarks>Ejecuta el procedimiento almacenado ct.sp_CuentasCt_Obtener mediante FromSqlRaw y
        /// devuelve el primer resultado sin seguimiento (AsNoTracking). El parámetro de búsqueda se envía como
        /// NULL.</remarks>
        /// <param name="cuentaId">Identificador de la cuenta a recuperar.</param>
        /// <returns>Una CuentaCt si existe; en caso contrario, null.</returns>
        public async Task<CuentaCt?> ObtenerPorIdAsync(int cuentaId)
        {
            var pCuentaId = new SqlParameter("@CuentaID", SqlDbType.Int) { Value = cuentaId };
            var pBusqueda = new SqlParameter("@busqueda", SqlDbType.NVarChar, 255) { Value = DBNull.Value };

            var resultado = await _context.TipoCuentaCt
                .FromSqlRaw("EXEC ct.sp_CuentasCt_Obtener @CuentaID = @CuentaID, @busqueda = @busqueda", pCuentaId, pBusqueda)
                .AsNoTracking()
                .ToListAsync();

            return resultado.FirstOrDefault();
        }

        /// <summary>
        /// Inserta una cuenta en la base de datos ejecutando el procedimiento almacenado ct.sp_CuentasCt_Insertar de
        /// forma asíncrona.
        /// </summary>
        /// <remarks>Mapea los argumentos a parámetros SQL y ejecuta ExecuteSqlRawAsync con los parámetros
        /// '@id_cuenta', '@codigo_cuenta' y '@descripcion_cuenta'.</remarks>
        /// <param name="idCuenta">Identificador nullable de la cuenta; si es null se envía DBNull.Value al procedimiento.</param>
        /// <param name="codigoCuenta">Código de la cuenta a insertar.</param>
        /// <param name="descripcionCuenta">Descripción de la cuenta.</param>
        /// <returns>Tarea que representa la operación asíncrona.</returns>
        public async Task CrearCuentaAsync(double? idCuenta, string codigoCuenta, string descripcionCuenta)
        {
            var pIdCuenta = new SqlParameter("@id_cuenta", (object?)idCuenta ?? DBNull.Value);
            var pCodigo = new SqlParameter("@codigo_cuenta", codigoCuenta);
            var pDesc = new SqlParameter("@descripcion_cuenta", descripcionCuenta);

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC ct.sp_CuentasCt_Insertar @id_cuenta, @codigo_cuenta, @descripcion_cuenta",
                pIdCuenta, pCodigo, pDesc);
        }

        /// <summary>
        /// Actualiza una cuenta en la base de datos ejecutando el procedimiento almacenado ct.sp_CuentasCt_Actualizar.
        /// </summary>
        /// <remarks>Los parámetros se envían como SqlParameter y se utiliza DBNull.Value cuando idCuenta
        /// es null; la ejecución se realiza mediante ExecuteSqlRawAsync en el contexto de base de datos.</remarks>
        /// <param name="cuentaId">Identificador de la cuenta en la base de datos.</param>
        /// <param name="idCuenta">Identificador alternativo de la cuenta; null para establecer un valor nulo en la base de datos.</param>
        /// <param name="codigoCuenta">Código de la cuenta.</param>
        /// <param name="descripcionCuenta">Descripción de la cuenta.</param>
        /// <returns>Tarea que representa la operación asíncrona.</returns>
        public async Task ActualizarCuentaAsync(int cuentaId, double? idCuenta, string codigoCuenta, string descripcionCuenta)
        {
            var pCuentaId = new SqlParameter("@CuentaID", cuentaId);
            var pIdCuenta = new SqlParameter("@id_cuenta", (object?)idCuenta ?? DBNull.Value);
            var pCodigo = new SqlParameter("@codigo_cuenta", codigoCuenta);
            var pDesc = new SqlParameter("@descripcion_cuenta", descripcionCuenta);

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC ct.sp_CuentasCt_Actualizar @CuentaID, @id_cuenta, @codigo_cuenta, @descripcion_cuenta",
                pCuentaId, pIdCuenta, pCodigo, pDesc);
        }

        /// <summary>
        /// Elimina una cuenta de la base de datos ejecutando el procedimiento almacenado ct.sp_CuentasCt_Eliminar.
        /// </summary>
        /// <remarks>El parámetro se envía como SqlParameter y se utiliza ExecuteSqlRawAsync en el contexto de base de datos.</remarks>
        /// <param name="cuentaId">Identificador de la cuenta en la base de datos.</param>
        /// <returns>Tarea que representa la operación asíncrona.</returns>
        public async Task EliminarCuentaAsync(int cuentaId)
        {
            var pCuentaId = new SqlParameter("@CuentaID", cuentaId);

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC ct.sp_CuentasCt_Eliminar @CuentaID", pCuentaId);
        }
    }
}
