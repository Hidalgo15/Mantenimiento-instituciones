using Mantenimiento.Core.Domain.Entities;
using Mantenimiento.Core.Domain.RepositoryInterfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Mantenimiento.Infraestructure.Persistence.Repositories
{
    public class DafRepository : IDafRepository
    {
        private readonly ApplicationDbContext _context;

        public DafRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Actualiza un registro de Daf en la base de datos utilizando un procedimiento almacenado.
        /// </summary>
        /// <param name="daf"></param>
        /// <returns></returns>
        public async Task<Daf> ActualizarAsync(Daf daf)
        {
            var pId = new SqlParameter("@id", SqlDbType.Int)
            {
                Value = daf.Id
            };

            var pIdSubCapitulo = new SqlParameter("@id_sub_capitulo", SqlDbType.Int)
            {
                Value = daf.IdSubCapitulo
            };

            var pCodigoDaf = new SqlParameter("@codigo_daf", SqlDbType.VarChar, 10)
            {
                Value = string.IsNullOrWhiteSpace(daf.CodigoDaf) ? (object)DBNull.Value : daf.CodigoDaf
            };

            var pNombreDaf = new SqlParameter("@daf", SqlDbType.VarChar, 200)
            {
                Value = string.IsNullOrWhiteSpace(daf.NombreDaf) ? (object)DBNull.Value : daf.NombreDaf
            };

            // Pasando los SqlParameter directamente como en los demás métodos
            var result = await _context.Dafs
                .FromSqlRaw("EXEC dbo.sp_daf_Actualizar @id, @id_sub_capitulo, @codigo_daf, @daf", pId, pIdSubCapitulo, pCodigoDaf, pNombreDaf)
                .AsNoTracking()
                .ToListAsync();

            return result.FirstOrDefault();
        }

        /// <summary>
        /// Crea un registro Daf en la base de datos ejecutando el procedimiento almacenado dbo.sp_daf_Crear y asigna el
        /// identificador generado al objeto proporcionado.
        /// </summary>
        /// <remarks>Ejecuta ExecuteSqlRawAsync con SqlParameter, usando un parámetro de salida para
        /// recuperar el Id generado. Pueden propagarse excepciones de acceso a datos.</remarks>
        /// <param name="daf">Daf con los datos a insertar; IdSubCapitulo se enviará como NULL si es menor o igual a cero.</param>
        /// <returns>El objeto Daf actualizado con el Id asignado por la base de datos.</returns>
        public async Task<Daf> CrearAsync(Daf daf)
        {
            var pIdSubCapitulo = new SqlParameter("@id_sub_capitulo", SqlDbType.Int)
            {
                Value = daf.IdSubCapitulo > 0 ? daf.IdSubCapitulo : DBNull.Value
            };
            var pCodigoDaf = new SqlParameter("@codigo_daf", SqlDbType.VarChar, 10) { Value = daf.CodigoDaf };
            var pNombreDaf = new SqlParameter("@daf", SqlDbType.VarChar, 200) { Value = daf.NombreDaf };
            var pIdGenerado = new SqlParameter
            {
                ParameterName = "@id_generado",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.Output
            };

            // Usando marcadores posicionales
            await _context.Database.ExecuteSqlRawAsync(
                "EXEC dbo.sp_daf_Crear {0}, {1}, {2}, {3} OUT",
                pIdSubCapitulo, pCodigoDaf, pNombreDaf, pIdGenerado
            );

            if (pIdGenerado.Value != DBNull.Value)
            {
                daf.Id = (int)pIdGenerado.Value;
            }

            return daf;
        }

        /// <summary>
        /// Elimina de forma asíncrona el registro con el identificador especificado ejecutando el procedimiento
        /// almacenado sp_daf_Eliminar.
        /// </summary>
        /// <remarks>Ejecuta SQL crudo mediante ExecuteSqlRawAsync en el DbContext; puede lanzar
        /// excepciones de la base de datos y no comprueba el número de filas afectadas.</remarks>
        /// <param name="id">Identificador del registro a eliminar.</param>
        /// <returns>True si la operación se completa correctamente.</returns>
        public async Task<bool> EliminarAsync(int id)
        {
            var pId = new SqlParameter("@id", id);
            await _context.Database.ExecuteSqlRawAsync("EXEC sp_daf_Eliminar @id", pId);
            return true;
        }

        /// <summary>
        /// Obtiene de forma asíncrona los registros Daf filtrados por id, idSubCapitulo y código DAF mediante el procedimiento almacenado
        /// dbo.sp_daf_Obtener.
        /// </summary>
        /// <remarks>Se ejecuta el procedimiento almacenado dbo.sp_daf_Obtener pasando DBNull.Value para
        /// parámetros no especificados. Los resultados se consultan sin seguimiento (AsNoTracking).</remarks>
        /// <param name="id">Identificador del registro a obtener; si es null o cero se omite el filtro.</param>
        /// <param name="idSubCapitulo">Identificador del subcapítulo; si es null o cero se omite el filtro.</param>
        /// <param name="codigoDaf">Código DAF para filtrar los resultados; si es null o una cadena vacía/señales de espacio se omite el filtro.</param>
        /// <returns>Una tarea que representa la operación asíncrona. El resultado contiene una secuencia de objetos Daf que
        /// cumplen el filtro.</returns>
        public async Task<IEnumerable<Daf>> ObtenerAsync(int? id = null, int? idSubCapitulo = null, string? codigoDaf = null)
        {
            var pId = new SqlParameter("@id", SqlDbType.Int) { Value = id.HasValue ? id.Value : DBNull.Value };
            var pIdSubCapitulo = new SqlParameter("@id_sub_capitulo", SqlDbType.Int) { Value = idSubCapitulo.HasValue ? idSubCapitulo.Value : DBNull.Value };
            var pCodigoDaf = new SqlParameter("@codigo_daf", string.IsNullOrWhiteSpace(codigoDaf) ? DBNull.Value : codigoDaf);

            return await _context.Dafs
                .FromSqlRaw("EXEC dbo.sp_daf_Obtener @id, @id_sub_capitulo, @codigo_daf", pId, pIdSubCapitulo, pCodigoDaf)
                .AsNoTracking()
                .ToListAsync();
        }

        /// <summary>
        /// Obtiene de forma asíncrona los registros Daf filtrados por código DAF mediante el procedimiento almacenado
        /// dbo.sp_daf_Obtener.
        /// </summary>
        /// <remarks>Se ejecuta el procedimiento almacenado dbo.sp_daf_Obtener pasando DBNull.Value para
        /// parámetros no especificados. Los resultados se consultan sin seguimiento (AsNoTracking).</remarks>
        /// <param name="codigoDaf">Código DAF para filtrar los resultados; si es null o una cadena vacía/señales de espacio se omite el filtro.</param>
        /// <returns>Una tarea que representa la operación asíncrona. El resultado contiene una secuencia de objetos Daf que
        /// cumplen el filtro.</returns>
        public async Task<IEnumerable<Daf>> ObtenerPorCodigoDafAsync(string? codigoDaf)
        {
            var pId = new SqlParameter("@id", SqlDbType.Int) { Value = DBNull.Value };
            var pIdSubCapitulo = new SqlParameter("@id_sub_capitulo", SqlDbType.Int) { Value = DBNull.Value };

            var pCodigoDaf = new SqlParameter("@codigo_daf", string.IsNullOrWhiteSpace(codigoDaf) ? DBNull.Value : codigoDaf);

            return await _context.Dafs
                .FromSqlRaw("EXEC dbo.sp_daf_Obtener @id,@id_sub_capitulo, @codigo_daf", pId, pIdSubCapitulo, pCodigoDaf)
                .AsNoTracking()
                .ToListAsync();
        }

        /// <summary>
        /// Obtiene un registro Daf por su identificador, subcapítulo y código DAF mediante el procedimiento almacenado
        /// dbo.sp_daf_Obtener.
        /// </summary>
        /// <param name="id">Identificador del registro a obtener.</param>
        /// <param name="idSubCapitulo">Identificador del subcapítulo; si es null o cero se omite el filtro.</param>
        /// <param name="codigoDaf">Código DAF para filtrar los resultados; si es null o una cadena vacía/señales de espacio se omite el filtro.</param>
        /// <returns>Una tarea que representa la operación asíncrona. El resultado contiene el objeto Daf que cumple el filtro.</returns>
        /// <exception cref="KeyNotFoundException"></exception>
        public async Task<Daf> ObtenerPorIdAsync(int id, int? idSubCapitulo = null, string? codigoDaf = null)
        {
            var pId = new SqlParameter("@id", SqlDbType.Int) { Value = id };
            var pIdSubCapitulo = new SqlParameter("@id_sub_capitulo", SqlDbType.Int)
            {
                Value = (idSubCapitulo.HasValue && idSubCapitulo.Value > 0) ? (object)idSubCapitulo.Value : DBNull.Value
            };
            var pCodigoDaf = new SqlParameter("@codigo_daf", SqlDbType.VarChar, 10)
            {
                Value = string.IsNullOrWhiteSpace(codigoDaf) ? (object)DBNull.Value : codigoDaf
            };

            var result = await _context.Dafs
                .FromSqlRaw("EXEC dbo.sp_daf_Obtener @id, @id_sub_capitulo, @codigo_daf", pId, pIdSubCapitulo, pCodigoDaf)
                .AsNoTracking()
                .ToListAsync();

            var subCapitulo = result.FirstOrDefault();
            if (subCapitulo == null)
            {
                throw new KeyNotFoundException($"No se encontró el subcapítulo con el ID {id}.");
            }

            return subCapitulo;
        }

        /// <summary>
        /// Obtiene los registros Daf asociados al subcapítulo especificado.
        /// </summary>
        /// <remarks>Ejecuta el procedimiento almacenado dbo.sp_daf_Obtener y aplica AsNoTracking en la
        /// consulta.</remarks>
        /// <param name="idSubCapitulo">Identificador del subcapítulo; si es null se omite el filtro por subcapítulo.</param>
        /// <returns>Una tarea asincrónica cuyo resultado es una colección de objetos Daf que coinciden con el subcapítulo
        /// especificado.</returns>
        public async Task<IEnumerable<Daf>> ObtenerPorSubCapituloAsync(int? idSubCapitulo)
        {
            var pId = new SqlParameter("@id", SqlDbType.Int) { Value = DBNull.Value };
            var pIdSubCapitulo = new SqlParameter("@id_sub_capitulo", SqlDbType.Int) { Value = idSubCapitulo.HasValue ? idSubCapitulo.Value : DBNull.Value };
            var pCodigoDaf = new SqlParameter("@codigo_daf", DBNull.Value);

            return await _context.Dafs
                .FromSqlRaw("EXEC dbo.sp_daf_Obtener @id, @id_sub_capitulo, @codigo_daf", pId, pIdSubCapitulo, pCodigoDaf)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}