using Mantenimiento.Core.Domain.Entities;
using Mantenimiento.Core.Domain.RepositoryInterfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Mantenimiento.Infraestructure.Persistence.Repositories
{
    public class CuentaInstitucionRepository : ICuentaInstitucionRepository
    {
        private readonly ApplicationDbContext _context;

        public CuentaInstitucionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<CuentaInstitucion> GetByIdAsync(int id)
        {
            var cuenta = await _context.CuentasInstitucion.FindAsync(id);

            return cuenta ?? throw new KeyNotFoundException($"No se encontró la cuenta de institución con el ID {id}.");
        }

        public async Task<List<Institucion>> ObtenerInstitucionesAsync()
        {
            return await _context.Instituciones.AsNoTracking().ToListAsync();
        }

        public async Task<List<CuentaInstitucion>> ObtenerCuentasPorEstructuraAsync(string insCodigo)
        {
            var param = new SqlParameter("@ins_codigo", insCodigo ?? (object)DBNull.Value);
            return await _context.CuentasInstitucion
                .FromSqlRaw("EXEC dbo.sp_cta_institucion_ObtenerPorEstructura @ins_codigo", param)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<bool> ExisteCuentaAsync(string insCodigo, string ctaCtaBanco, int? idExcluir = null)
        {
            var query = _context.CuentasInstitucion
                .Where(c => c.InsCodigo == insCodigo && c.CtaCtaBanco == ctaCtaBanco);

            if (idExcluir.HasValue)
            {
                query = query.Where(c => c.Id != idExcluir.Value);
            }

            return await query.AnyAsync();
        }

        public async Task<int> InsertarConSpAsync(CuentaInstitucion cuenta)
        {
            var pCodigo = new SqlParameter("@ins_codigo", cuenta.InsCodigo ?? (object)DBNull.Value);
            var pInst = new SqlParameter("@Institucion", cuenta.Institucion ?? (object)DBNull.Value);
            var pBanco = new SqlParameter("@cta_cta_banco", cuenta.CtaCtaBanco ?? (object)DBNull.Value);
            var pDesc = new SqlParameter("@cta_descripcion", cuenta.CtaDescripcion ?? (object)DBNull.Value);
            var pNuevoId = new SqlParameter
            {
                ParameterName = "@NuevoID",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.Output
            };

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC dbo.sp_cta_institucion_Insertar @ins_codigo, @Institucion, @cta_cta_banco, @cta_descripcion, @NuevoID OUTPUT",
                pCodigo, pInst, pBanco, pDesc, pNuevoId);

            return (int)pNuevoId.Value;
        }

        public async Task ActualizarConSpAsync(CuentaInstitucion cuenta)
        {
            var pId = new SqlParameter("@ID", cuenta.Id);
            var pCodigo = new SqlParameter("@ins_codigo", cuenta.InsCodigo ?? (object)DBNull.Value);
            var pInst = new SqlParameter("@Institucion", cuenta.Institucion ?? (object)DBNull.Value);
            var pBanco = new SqlParameter("@cta_cta_banco", cuenta.CtaCtaBanco ?? (object)DBNull.Value);
            var pDesc = new SqlParameter("@cta_descripcion", cuenta.CtaDescripcion ?? (object)DBNull.Value);

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC dbo.sp_cta_institucion_Actualizar @ID, @ins_codigo, @Institucion, @cta_cta_banco, @cta_descripcion",
                pId, pCodigo, pInst, pBanco, pDesc);
        }

        public async Task EliminarConSpAsync(int id)
        {
            var pId = new SqlParameter("@ID", id);
            await _context.Database.ExecuteSqlRawAsync("EXEC dbo.sp_cta_institucion_Eliminar @ID", pId);
        }
    }
}