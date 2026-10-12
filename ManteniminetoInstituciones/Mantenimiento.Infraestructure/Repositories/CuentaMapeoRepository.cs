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
    public class CuentaMapeoRepository : ICuentaMapeoRepository
    {

        private readonly ApplicationDbContext _context;

        public CuentaMapeoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

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

        public async Task<List<CuentaCt>> ObtenerCuentasBaseCatalogAsync()
        {
            return await _context.TipoCuentaCt
                .AsNoTracking()
                .OrderBy(c => c.CodigoCuenta)
                .ToListAsync();
        }

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

        public async Task CrearCuentaAsync(int idCuentaBase, string? descripcion, int tipoTramite)
        {
            var pIdBase = new SqlParameter("@id_cuenta", idCuentaBase);
            var pDesc = new SqlParameter("@descripcion_cuenta", (object?)descripcion ?? DBNull.Value);
            var pTipo = new SqlParameter("@tipo_tramite", tipoTramite);

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC dbo.sp_Cuentas_Insertar @id_cuenta, @descripcion_cuenta, @tipo_tramite",
                pIdBase, pDesc, pTipo);
        }

        public async Task ActualizarCuentaAsync(int idCuenta, string descripcion, int tipoTramite)
        {
            var pId = new SqlParameter("@id_cuenta", idCuenta);
            var pDesc = new SqlParameter("@descripcion_cuenta", descripcion);
            var pTipo = new SqlParameter("@tipo_tramite", tipoTramite);

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC dbo.sp_Cuentas_Actualizar @id_cuenta, @descripcion_cuenta, @tipo_tramite",
                pId, pDesc, pTipo);
        }

        public async Task EliminarCuentaAsync(int idCuenta)
        {
            var pId = new SqlParameter("@id_cuenta", idCuenta);
            await _context.Database.ExecuteSqlRawAsync(
                "EXEC dbo.sp_Cuentas_Eliminar @id_cuenta", pId);
        }
    }
}
