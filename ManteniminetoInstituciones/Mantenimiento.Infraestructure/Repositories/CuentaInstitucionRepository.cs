using Mantenimiento.Core.Domain.Entities;
using Mantenimiento.Core.Domain.RepositoryInterfaces;
using Mantenimiento.Infraestructure.Persistence.RepositoryServices.GenericRepository;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Mantenimiento.Infraestructure.Persistence.Repositories
{
    public class CuentaInstitucionRepository : GenericRepository<CuentaInstitucion>, ICuentaInstitucionRepository
    {
        public CuentaInstitucionRepository(ApplicationDbContext context) : base(context)
        {
        }

        // 1. Método específico para consultar la vista v_institucion
        public async Task<List<Institucion>> ObtenerInstitucionesAsync()
        {
            return await _context.Instituciones
                .AsNoTracking()
                .ToListAsync();
        }

        // 2. Método específico para ejecutar el Stored Procedure por filtro de estructura
        public async Task<List<CuentaInstitucion>> ObtenerCuentasPorEstructuraAsync(string insCodigo)
        {
            var param = new SqlParameter("@ins_codigo", insCodigo ?? (object)System.DBNull.Value);
            return await _context.CuentasInstitucion
                .FromSqlRaw("EXEC dbo.sp_cta_institucion_ObtenerPorEstructura @ins_codigo", param)
                .AsNoTracking()
                .ToListAsync();
        }

        // ----------------------------------------------------------------------------------
        // NOTA: Si prefieres ejecutar tus Stored Procedures para Insertar/Actualizar/Eliminar 
        // en lugar de las consultas LINQ/EF Core por defecto, puedes SOBREESCRIBIR (override)
        // o redefinir los métodos del GenericRepository:
        // ----------------------------------------------------------------------------------

        public async Task<int> InsertarConSpAsync(CuentaInstitucion cuenta)
        {
            var pCodigo = new SqlParameter("@ins_codigo", cuenta.InsCodigo ?? (object)System.DBNull.Value);
            var pInst = new SqlParameter("@Institucion", cuenta.Institucion ?? (object)System.DBNull.Value);
            var pBanco = new SqlParameter("@cta_cta_banco", cuenta.CtaCtaBanco ?? (object)System.DBNull.Value);
            var pDesc = new SqlParameter("@cta_descripcion", cuenta.CtaDescripcion ?? (object)System.DBNull.Value);
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
            var pCodigo = new SqlParameter("@ins_codigo", cuenta.InsCodigo ?? (object)System.DBNull.Value);
            var pInst = new SqlParameter("@Institucion", cuenta.Institucion ?? (object)System.DBNull.Value);
            var pBanco = new SqlParameter("@cta_cta_banco", cuenta.CtaCtaBanco ?? (object)System.DBNull.Value);
            var pDesc = new SqlParameter("@cta_descripcion", cuenta.CtaDescripcion ?? (object)System.DBNull.Value);

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