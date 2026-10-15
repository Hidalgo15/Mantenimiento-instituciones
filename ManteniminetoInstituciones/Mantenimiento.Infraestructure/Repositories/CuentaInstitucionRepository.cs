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

        /// <summary>
        /// Obtiene una cuenta de institución por su ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        public async Task<CuentaInstitucion> GetByIdAsync(int id)
        {
            var cuenta = await _context.CuentasInstitucion.FindAsync(id);

            return cuenta ?? throw new KeyNotFoundException($"No se encontró la cuenta de institución con el ID {id}.");
        }

        /// <summary>
        /// Obtiene las cuentas de institución asociadas a la estructura especificada.
        /// </summary>
        /// <remarks>Ejecuta el procedimiento almacenado dbo.sp_cta_institucion_ObtenerPorEstructura con
        /// el parámetro @ins_codigo y devuelve entidades en modo sin seguimiento (AsNoTracking).</remarks>
        /// <param name="insCodigo">Código de la institución; si es null se envía DBNull.Value al procedimiento almacenado.</param>
        /// <returns>Una tarea que representa la operación asincrónica. El resultado es una lista de CuentaInstitucion.</returns>
        public async Task<List<CuentaInstitucion>> ObtenerCuentasPorEstructuraAsync(string insCodigo)
        {
            var param = new SqlParameter("@ins_codigo", insCodigo ?? (object)DBNull.Value);
            return await _context.CuentasInstitucion
                .FromSqlRaw("EXEC dbo.sp_cta_institucion_ObtenerPorEstructura @ins_codigo", param)
                .AsNoTracking()
                .ToListAsync();
        }

        /// <summary>
        /// Determina si existe una cuenta con el código de institución y el número de cuenta proporcionados,
        /// opcionalmente excluyendo un registro por Id.
        /// </summary>
        /// <remarks>Ejecuta una consulta asíncrona contra la fuente de datos usando AnyAsync; la
        /// exclusión por Id se aplica antes de evaluar la existencia.</remarks>
        /// <param name="insCodigo">Código de la institución asociado a la cuenta.</param>
        /// <param name="ctaCtaBanco">Número o identificador de la cuenta bancaria.</param>
        /// <param name="idExcluir">Id opcional del registro a excluir de la comprobación; si es null no se excluye ninguno.</param>
        /// <returns>true si existe al menos una cuenta que coincide con los criterios; en caso contrario, false.</returns>
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

        /// <summary>
        /// Inserta una CuentaInstitucion en la base de datos mediante el procedimiento almacenado
        /// dbo.sp_cta_institucion_Insertar y devuelve el identificador generado.
        /// </summary>
        /// <remarks>Operación asíncrona que ejecuta ExecuteSqlRawAsync con parámetros y obtiene el
        /// identificador mediante un parámetro OUTPUT. Puede lanzar excepciones de EF Core o del proveedor SQL en caso
        /// de error.</remarks>
        /// <param name="cuenta">CuentaInstitucion que contiene los datos a insertar; las propiedades nulas se envían como DBNull.</param>
        /// <returns>Entero que representa el identificador recién creado (NuevoID) devuelto por el procedimiento almacenado.</returns>
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


        /// <summary>
        /// Actualiza una CuentaInstitucion mediante el procedimiento almacenado dbo.sp_cta_institucion_Actualizar.
        /// </summary>
        /// <remarks>Ejecuta el procedimiento almacenado dbo.sp_cta_institucion_Actualizar; las
        /// propiedades nulas se envían como DBNull.Value.</remarks>
        /// <param name="cuenta">CuentaInstitucion con los valores a actualizar.</param>
        /// <returns>Tarea que representa la operación asincrónica de actualización.</returns>
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

        /// <summary>
        /// Elimina la entidad con el identificador especificado ejecutando de forma asíncrona el procedimiento
        /// almacenado dbo.sp_cta_institucion_Eliminar.
        /// </summary>
        /// <remarks>Ejecuta SQL crudo mediante ExecuteSqlRawAsync y utiliza SqlParameter para el
        /// parámetro @ID; pueden propagarse excepciones de acceso a datos.</remarks>
        /// <param name="id">Identificador de la entidad a eliminar; se pasa como parámetro @ID al procedimiento almacenado.</param>
        /// <returns>Tarea que representa la operación asíncrona.</returns>
        public async Task EliminarConSpAsync(int id)
        {
            var pId = new SqlParameter("@ID", id);
            await _context.Database.ExecuteSqlRawAsync("EXEC dbo.sp_cta_institucion_Eliminar @ID", pId);
        }
    }
}