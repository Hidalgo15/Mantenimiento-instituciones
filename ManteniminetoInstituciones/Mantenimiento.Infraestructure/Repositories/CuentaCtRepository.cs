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

        public async Task<List<CuentaCt>> ObtenerCuentasAsync(string? busqueda = null)
        {
            var pCuentaId = new SqlParameter("@CuentaID", SqlDbType.Int) { Value = DBNull.Value };
            var pBusqueda = new SqlParameter("@busqueda", SqlDbType.NVarChar, 255) { Value = (object?)busqueda ?? DBNull.Value };

            return await _context.TipoCuentaCt
                .FromSqlRaw("EXEC ct.sp_CuentasCt_Obtener @CuentaID = @CuentaID, @busqueda = @busqueda", pCuentaId, pBusqueda)
                .AsNoTracking()
                .ToListAsync();
        }

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

        public async Task CrearCuentaAsync(double? idCuenta, string codigoCuenta, string descripcionCuenta)
        {
            var pIdCuenta = new SqlParameter("@id_cuenta", (object?)idCuenta ?? DBNull.Value);
            var pCodigo = new SqlParameter("@codigo_cuenta", codigoCuenta);
            var pDesc = new SqlParameter("@descripcion_cuenta", descripcionCuenta);

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC ct.sp_CuentasCt_Insertar @id_cuenta, @codigo_cuenta, @descripcion_cuenta",
                pIdCuenta, pCodigo, pDesc);
        }

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

        public async Task EliminarCuentaAsync(int cuentaId)
        {
            var pCuentaId = new SqlParameter("@CuentaID", cuentaId);

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC ct.sp_CuentasCt_Eliminar @CuentaID", pCuentaId);
        }
    }
}
