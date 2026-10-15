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

        /// <summary>
        /// Obtiene de forma asíncrona las UnidadesEjecutoras que coinciden con los filtros opcionales mediante el
        /// procedimiento almacenado dbo.sp_unidad_ejecutora_Obtener.
        /// </summary>
        /// <remarks>La consulta se ejecuta en modo lectura (AsNoTracking) y el filtrado se realiza por el
        /// procedimiento almacenado mencionado.</remarks>
        /// <param name="id">Identificador de la unidad ejecutora; si es nulo, no se aplica filtro por id.</param>
        /// <param name="codigoUnidadEjecutora">Código de la unidad ejecutora; si es nulo, vacío o contiene solo espacios, no se aplica filtro por código.</param>
        /// <param name="idDaf">Identificador del DAF asociado; si es nulo, no se aplica filtro por DAF.</param>
        /// <returns>Una tarea que representa la operación asíncrona cuyo resultado es una colección de UnidadEjecutora que
        /// cumplen los filtros.</returns>
        public async Task<IEnumerable<UnidadEjecutora>> ObtenerAsync(int? id = null, string? codigoUnidadEjecutora = null, int? idDaf = null)
        {
            var pId = new SqlParameter("@id", SqlDbType.Int) { Value = id.HasValue ? id.Value : DBNull.Value };
            var pCodigo = new SqlParameter("@codigo_unidad_ejecutora", string.IsNullOrWhiteSpace(codigoUnidadEjecutora) ? DBNull.Value : codigoUnidadEjecutora);
            var pIdDaf = new SqlParameter("@id_daf", SqlDbType.Int) { Value = idDaf.HasValue ? idDaf.Value : DBNull.Value };

            return await _context.UnidadesEjecutoras
                .FromSqlRaw("EXEC dbo.sp_unidad_ejecutora_Obtener @id = @id, @codigo_unidad_ejecutora = @codigo_unidad_ejecutora, @id_daf = @id_daf", pId, pCodigo, pIdDaf)
                .AsNoTracking()
                .ToListAsync();
        }

        /// <summary>
        /// Crea una unidad ejecutora en la base de datos mediante el procedimiento almacenado
        /// dbo.sp_unidad_ejecutora_Crear y devuelve el identificador generado.
        /// </summary>
        /// <remarks>La operación se ejecuta de forma asíncrona mediante ExecuteSqlRawAsync y obtiene el
        /// id generado desde el parámetro de salida @id_generado. Las propiedades nulas se envían como DBNull.Value y
        /// el parámetro usuario se recorta antes de usarse.</remarks>
        /// <param name="entidad">Unidad ejecutora con los datos a persistir (IdPadre, IdDaf, CodigoUnidadEjecutora, NombreUnidadEjecutora,
        /// Rnc, Estado, PortalCompra, IdCategoria).</param>
        /// <param name="usuario">Usuario que realiza la operación; si es nulo o vacío se sustituye por 'SISTEMA'.</param>
        /// <returns>Identificador generado (int) de la unidad ejecutora creada.</returns>
        public async Task<int> CrearConSpAsync(UnidadEjecutora entidad, string usuario)
        {
            var pIdPadre = new SqlParameter("@id_padre", SqlDbType.Int) { Value = entidad.IdPadre.HasValue ? entidad.IdPadre.Value : DBNull.Value };
            var pIdDaf = new SqlParameter("@id_daf", SqlDbType.Int) { Value = entidad.IdDaf };
            var pCodigo = new SqlParameter("@codigo_unidad_ejecutora", entidad.CodigoUnidadEjecutora ?? (object)DBNull.Value);
            var pNombre = new SqlParameter("@unidad_ejecutora", entidad.NombreUnidadEjecutora ?? (object)DBNull.Value);
            var pRnc = new SqlParameter("@rnc", string.IsNullOrWhiteSpace(entidad.Rnc) ? DBNull.Value : entidad.Rnc);

            var pEstado = new SqlParameter("@estado", SqlDbType.Int) { Value = entidad.Estado };
            var pPortalCompra = new SqlParameter("@portal_compra", SqlDbType.Int) { Value = entidad.PortalCompra };

            var usuarioValidado = string.IsNullOrWhiteSpace(usuario) ? "SISTEMA" : usuario.Trim();
            var pUsuario = new SqlParameter("@usuario", usuarioValidado);

            var pIdCategoria = new SqlParameter("@id_categoria", SqlDbType.Int) { Value = entidad.IdCategoria.HasValue ? entidad.IdCategoria.Value : DBNull.Value };

            var pIdGenerado = new SqlParameter
            {
                ParameterName = "@id_generado",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.Output
            };

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC dbo.sp_unidad_ejecutora_Crear @id_padre, @id_daf, @codigo_unidad_ejecutora, @unidad_ejecutora, @rnc, @estado, @portal_compra, @usuario, @id_categoria, @id_generado OUTPUT",
                pIdPadre, pIdDaf, pCodigo, pNombre, pRnc, pEstado, pPortalCompra, pUsuario, pIdCategoria, pIdGenerado);

            return (int)pIdGenerado.Value;
        }

        /// <summary>
        /// Actualiza una unidad ejecutora en la base de datos mediante el procedimiento almacenado
        /// dbo.sp_unidad_ejecutora_Actualizar de forma asíncrona.
        /// </summary>
        /// <remarks>Ejecuta el procedimiento almacenado dbo.sp_unidad_ejecutora_Actualizar mapeando
        /// parámetros y enviando valores nulos como DBNull.Value. Puede propagarse excepciones de acceso a datos si la
        /// ejecución falla.</remarks>
        /// <param name="entidad">UnidadEjecutora con los valores a persistir: Id, IdPadre, IdDaf, CodigoUnidadEjecutora,
        /// NombreUnidadEjecutora, Rnc, Estado, PortalCompra e IdCategoria.</param>
        /// <param name="usuario">Identificador o nombre del usuario que realiza la operación; puede ser null.</param>
        /// <returns>Una tarea que representa la operación asíncrona.</returns>
        public async Task ActualizarConSpAsync(UnidadEjecutora entidad, string usuario)
        {
            var pId = new SqlParameter("@id", entidad.Id);
            var pIdPadre = new SqlParameter("@id_padre", SqlDbType.Int) { Value = entidad.IdPadre.HasValue ? entidad.IdPadre.Value : DBNull.Value };
            var pIdDaf = new SqlParameter("@id_daf", SqlDbType.Int) { Value = entidad.IdDaf };
            var pCodigo = new SqlParameter("@codigo_unidad_ejecutora", entidad.CodigoUnidadEjecutora ?? (object)DBNull.Value);
            var pNombre = new SqlParameter("@unidad_ejecutora", entidad.NombreUnidadEjecutora ?? (object)DBNull.Value);
            var pRnc = new SqlParameter("@rnc", string.IsNullOrWhiteSpace(entidad.Rnc) ? DBNull.Value : entidad.Rnc);

            var pEstado = new SqlParameter("@estado", SqlDbType.Int) { Value = entidad.Estado };
            var pPortalCompra = new SqlParameter("@portal_compra", SqlDbType.Int) { Value = entidad.PortalCompra };

            var pUsuario = new SqlParameter("@usuario", usuario ?? (object)DBNull.Value);
            var pIdCategoria = new SqlParameter("@id_categoria", SqlDbType.Int) { Value = entidad.IdCategoria.HasValue ? entidad.IdCategoria.Value : DBNull.Value };

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC dbo.sp_unidad_ejecutora_Actualizar @id, @id_padre, @id_daf, @codigo_unidad_ejecutora, @unidad_ejecutora, @rnc, @estado, @portal_compra, @usuario, @id_categoria",
                pId, pIdPadre, pIdDaf, pCodigo, pNombre, pRnc, pEstado, pPortalCompra, pUsuario, pIdCategoria);
        }

        /// <summary>
        /// Elimina una unidad ejecutora llamando al procedimiento almacenado dbo.sp_unidad_ejecutora_Eliminar.
        /// </summary>
        /// <remarks>La llamada ejecuta el procedimiento almacenado en la base de datos mediante
        /// ExecuteSqlRawAsync y pasa los parámetros @id, @borrado_logico y @usuario.</remarks>
        /// <param name="id">Identificador de la unidad ejecutora a eliminar.</param>
        /// <param name="borradoLogico">True para realizar un borrado lógico; false para borrado físico.</param>
        /// <param name="usuario">Nombre del usuario que realiza la operación; si es null se envía DBNull.Value al procedimiento.</param>
        /// <returns>Tarea que representa la operación asíncrona.</returns>
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