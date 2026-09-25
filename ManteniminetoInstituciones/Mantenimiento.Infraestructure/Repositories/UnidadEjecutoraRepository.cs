using Mantenimiento.Core.Domain.Entities;
using Mantenimiento.Core.Domain.RepositoryInterfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Mantenimiento.Infraestructure.Persistence.Repositories
{
    public class UnidadEjecutoraRepository : IUnidadEjecutoraRepository
    {
        private readonly ApplicationDbContext _context;

        public UnidadEjecutoraRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UnidadEjecutora>> ObtenerAsync(int? id = null, string? codigoUnidadEjecutora = null, int? idDaf = null)
        {
            var pId = new SqlParameter("@id", id.HasValue ? id.Value : DBNull.Value);
            var pCodigo = new SqlParameter("@codigo_unidad_ejecutora", string.IsNullOrWhiteSpace(codigoUnidadEjecutora) ? DBNull.Value : codigoUnidadEjecutora);
            var pIdDaf = new SqlParameter("@id_daf", idDaf.HasValue ? idDaf.Value : DBNull.Value);

            return await _context.UnidadesEjecutoras
                .FromSqlRaw("EXEC dbo.sp_unidad_ejecutora_Obtener @id = @id, @codigo_unidad_ejecutora = @codigo_unidad_ejecutora, @id_daf = @id_daf", pId, pCodigo, pIdDaf)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<int> CrearConSpAsync(UnidadEjecutora entidad, string usuario)
        {
            var pIdPadre = new SqlParameter("@id_padre", entidad.IdPadre.HasValue ? entidad.IdPadre.Value : DBNull.Value);
            var pIdDaf = new SqlParameter("@id_daf", entidad.IdDaf);
            var pCodigo = new SqlParameter("@codigo_unidad_ejecutora", entidad.CodigoUnidadEjecutora ?? (object)DBNull.Value);
            var pNombre = new SqlParameter("@unidad_ejecutora", entidad.NombreUnidadEjecutora ?? (object)DBNull.Value);
            var pRnc = new SqlParameter("@rnc", string.IsNullOrWhiteSpace(entidad.Rnc) ? DBNull.Value : entidad.Rnc);

            var pEstado = new SqlParameter("@estado", SqlDbType.Int) { Value = entidad.Estado };
            var pPortalCompra = new SqlParameter("@portal_compra", SqlDbType.Int) { Value = entidad.PortalCompra };

            var pUsuario = new SqlParameter("@usuario", usuario ?? (object)DBNull.Value);
            var pIdGenerado = new SqlParameter
            {
                ParameterName = "@id_generado",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.Output
            };

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC dbo.sp_unidad_ejecutora_Crear @id_padre, @id_daf, @codigo_unidad_ejecutora, @unidad_ejecutora, @rnc, @estado, @portal_compra, @usuario, @id_generado OUTPUT",
                pIdPadre, pIdDaf, pCodigo, pNombre, pRnc, pEstado, pPortalCompra, pUsuario, pIdGenerado);

            return (int)pIdGenerado.Value;
        }

        public async Task ActualizarConSpAsync(UnidadEjecutora entidad, string usuario)
        {
            var pId = new SqlParameter("@id", entidad.Id);
            var pIdPadre = new SqlParameter("@id_padre", entidad.IdPadre.HasValue ? entidad.IdPadre.Value : DBNull.Value);
            var pIdDaf = new SqlParameter("@id_daf", entidad.IdDaf);
            var pCodigo = new SqlParameter("@codigo_unidad_ejecutora", entidad.CodigoUnidadEjecutora ?? (object)DBNull.Value);
            var pNombre = new SqlParameter("@unidad_ejecutora", entidad.NombreUnidadEjecutora ?? (object)DBNull.Value);
            var pRnc = new SqlParameter("@rnc", string.IsNullOrWhiteSpace(entidad.Rnc) ? DBNull.Value : entidad.Rnc);

            var pEstado = new SqlParameter("@estado", SqlDbType.Int) { Value = entidad.Estado };
            var pPortalCompra = new SqlParameter("@portal_compra", SqlDbType.Int) { Value = entidad.PortalCompra };

            var pUsuario = new SqlParameter("@usuario", usuario ?? (object)DBNull.Value);

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC dbo.sp_unidad_ejecutora_Actualizar @id, @id_padre, @id_daf, @codigo_unidad_ejecutora, @unidad_ejecutora, @rnc, @estado, @portal_compra, @usuario",
                pId, pIdPadre, pIdDaf, pCodigo, pNombre, pRnc, pEstado, pPortalCompra, pUsuario);
        }

        public async Task EliminarConSpAsync(int id, bool borradoLogico, string usuario)
        {
            var pId = new SqlParameter("@id", id);
            var pBorrado = new SqlParameter("@borrado_logico", borradoLogico);
            var pUsuario = new SqlParameter("@usuario", usuario ?? (object)DBNull.Value);

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC dbo.sp_unidad_ejecutora_Eliminar @id, @borrado_logico, @usuario",
                pId, pBorrado, pUsuario);
        }
    }
}