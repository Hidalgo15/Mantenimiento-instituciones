using Mantenimiento.Core.Domain.Entities;
using Mantenimiento.Core.Domain.RepositoryInterfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Mantenimiento.Infraestructure.Persistence.Repositories
{
    public class SubCapituloRepository : ISubCapituloRepository
    {
        private readonly ApplicationDbContext _context;
        public SubCapituloRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Actualiza un subcapítulo en la base de datos utilizando un procedimiento almacenado.
        /// </summary>
        /// <param name="subCapitulo"></param>
        /// <returns></returns>
        public async Task ActualizarConSpAsync(SubCapitulo subCapitulo)
        {
            var pId = new SqlParameter("@id", subCapitulo.Id);
            var pIdCapitulo = new SqlParameter("@id_capitulo", subCapitulo.IdCapitulo);
            var pCodigo = new SqlParameter("@codigo_sub_capitulo", subCapitulo.CodigoSubCapitulo ?? (object)DBNull.Value);
            var pNombre = new SqlParameter("@sub_capitulo", subCapitulo.subcapitulo ?? (object)DBNull.Value);

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC dbo.sp_subcapitulo_Actualizar @id, @id_capitulo, @codigo_sub_capitulo, @sub_capitulo",
                pId, pIdCapitulo, pCodigo, pNombre);
        }

        /// <summary>
        /// Elimina un subcapítulo de la base de datos utilizando un procedimiento almacenado.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task EliminarConSpAsync(int id)
        {
            var pId = new SqlParameter("@id", id);
            await _context.Database.ExecuteSqlRawAsync("EXEC sp_subcapitulo_Eliminar @id", pId);
        }

        /// <summary>
        /// Obtiene un subcapítulo por su identificador, con opción de filtrar por capítulo y por término de búsqueda.
        /// </summary>
        /// <remarks>Ejecuta el procedimiento almacenado 'sp_subcapitulo_Obtener' mediante FromSqlRaw y
        /// realiza la consulta sin seguimiento (AsNoTracking).</remarks>
        /// <param name="id">Identificador del subcapítulo a recuperar.</param>
        /// <param name="id_capitulo">Identificador del capítulo para filtrar; si es menor o igual a 0 se trata como nulo.</param>
        /// <param name="buscar">Término de búsqueda para filtrar; si es nulo, vacío o contiene solo espacios en blanco se ignora.</param>
        /// <returns>El subcapítulo correspondiente al identificador especificado.</returns>
        /// <exception cref="KeyNotFoundException">Se lanza si no se encuentra un subcapítulo con el identificador proporcionado.</exception>
        public async Task<SubCapitulo> GetByIdAsync(int id, int id_capitulo, string? buscar)
        {
            var pId = new SqlParameter("@id", id);
            var pIdCapitulo = new SqlParameter("@id_capitulo", id_capitulo <= 0 ? (object)DBNull.Value : id_capitulo);
            var pBuscar = new SqlParameter("@buscar", string.IsNullOrWhiteSpace(buscar) ? (object)DBNull.Value : buscar);

            var result = await _context.SubCapitulos
                .FromSqlRaw("EXEC sp_subcapitulo_Obtener @id = @id, @id_capitulo = @id_capitulo, @buscar = @buscar", pId, pIdCapitulo, pBuscar)
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
        /// Inserta un SubCapitulo en la base de datos mediante el procedimiento almacenado dbo.sp_subcapitulo_Crear y
        /// devuelve el identificador generado.
        /// </summary>
        /// <remarks>Ejecuta dbo.sp_subcapitulo_Crear mediante ExecuteSqlRawAsync en el contexto de EF
        /// Core, respetando el orden de parámetros y leyendo @id_generado como parámetro de salida.</remarks>
        /// <param name="subCapitulo">SubCapitulo a insertar; sus propiedades IdCapitulo, CodigoSubCapitulo y subcapitulo se pasan como parámetros
        /// del procedimiento.</param>
        /// <returns>Identificador generado (int) del SubCapitulo insertado.</returns>
        public async Task<int> InsertarConSpAsync(SubCapitulo subCapitulo)
        {
            var pIdCapitulo = new SqlParameter("@id_capitulo", subCapitulo.IdCapitulo);
            var pCodigo = new SqlParameter("@codigo_sub_capitulo", subCapitulo.CodigoSubCapitulo ?? (object)DBNull.Value);
            var pNombre = new SqlParameter("@sub_capitulo", subCapitulo.subcapitulo ?? (object)DBNull.Value);

            var pIdGenerado = new SqlParameter
            {
                ParameterName = "@id_generado",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.Output
            };

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC dbo.sp_subcapitulo_Crear @id_capitulo, @codigo_sub_capitulo, @sub_capitulo, @id_generado OUTPUT",
                pIdCapitulo, pCodigo, pNombre, pIdGenerado);

            return (int)pIdGenerado.Value;
        }

        /// <summary>
        /// Obtiene de forma asíncrona los subcapítulos desde la base de datos mediante el procedimiento almacenado
        /// sp_subcapitulo_Obtener.
        /// </summary>
        /// <remarks>Llama al procedimiento almacenado sp_subcapitulo_Obtener mediante FromSqlRaw pasando
        /// @id y @id_capitulo como NULL y @buscar según el parámetro; utiliza AsNoTracking para consultas de solo
        /// lectura.</remarks>
        /// <param name="buscar">Texto opcional para filtrar resultados; si es null, vacío o contiene solo espacios se ignora el filtro.</param>
        /// <returns>Tarea que representa la operación asíncrona y devuelve una lista de SubCapitulo sin seguimiento
        /// (AsNoTracking).</returns>
        public async Task<List<SubCapitulo>> ObtenerSubCapitulosAsync(string? buscar = null)
        {
            var pId = new SqlParameter("@id", SqlDbType.Int) { Value = DBNull.Value };
            var pIdCapitulo = new SqlParameter("@id_capitulo", SqlDbType.Int) { Value = DBNull.Value };
            var pBuscar = new SqlParameter("@buscar", string.IsNullOrWhiteSpace(buscar) ? (object)DBNull.Value : buscar);

            return await _context.SubCapitulos
                .FromSqlRaw("EXEC sp_subcapitulo_Obtener @id = @id, @id_capitulo = @id_capitulo, @buscar = @buscar", pId, pIdCapitulo, pBuscar)
                .AsNoTracking()
                .ToListAsync();
        }

        /// <summary>
        /// Obtiene de forma asíncrona los subcapítulos asociados al capítulo indicado.
        /// </summary>
        /// <remarks>Ejecuta el procedimiento almacenado 'sp_subcapitulo_Obtener' mediante FromSqlRaw y
        /// devuelve las entidades sin seguimiento (AsNoTracking).</remarks>
        /// <param name="capituloId">Identificador del capítulo cuyos subcapítulos se recuperan.</param>
        /// <returns>Tarea que representa la operación asíncrona; el resultado es una lista de SubCapitulo. La lista estará vacía
        /// si no se encuentran subcapítulos.</returns>
        public async Task<List<SubCapitulo>> ObtenerSubCapitulosPorCapituloAsync(int capituloId)
        {
            var pId = new SqlParameter("@id", SqlDbType.Int) { Value = DBNull.Value };
            var pIdCapitulo = new SqlParameter("@id_capitulo", capituloId);
            var pBuscar = new SqlParameter("@buscar", DBNull.Value);

            return await _context.SubCapitulos
                .FromSqlRaw("EXEC sp_subcapitulo_Obtener @id = @id, @id_capitulo = @id_capitulo, @buscar = @buscar", pId, pIdCapitulo, pBuscar)
                .AsNoTracking()
                .ToListAsync();
        }

        /// <summary>
        /// Obtiene de forma asíncrona los subcapítulos filtrados por el código de capítulo.
        /// </summary>
        /// <remarks>Ejecuta el procedimiento almacenado sp_subcapitulo_Obtener mediante FromSqlRaw,
        /// pasando @id y @id_capitulo como DBNull y @buscar con el valor de codigoCapitulo o DBNull si es nulo/vacío.
        /// La consulta se materializa con ToListAsync.</remarks>
        /// <param name="codigoCapitulo">Código del capítulo usado como criterio de búsqueda; si es null o contiene solo espacios se envía DBNull
        /// para omitir el filtro.</param>
        /// <returns>Tarea que representa la operación asíncrona; el resultado es una lista de SubCapitulo obtenida desde la base
        /// de datos sin seguimiento (AsNoTracking).</returns>
        public async Task<List<SubCapitulo>> ObtenerSubCapitulosPorCodigoCapituloAsync(string codigoCapitulo)
        {
            var pId = new SqlParameter("@id", SqlDbType.Int) { Value = DBNull.Value };
            var pIdCapitulo = new SqlParameter("@id_capitulo", SqlDbType.Int) { Value = DBNull.Value };
            var pBuscar = new SqlParameter("@buscar", string.IsNullOrWhiteSpace(codigoCapitulo) ? (object)DBNull.Value : codigoCapitulo);

            return await _context.SubCapitulos
                .FromSqlRaw("EXEC sp_subcapitulo_Obtener @id = @id, @id_capitulo = @id_capitulo, @buscar = @buscar", pId, pIdCapitulo, pBuscar)
                .AsNoTracking()
                .ToListAsync();
        }

        /// <summary>
        /// Obtiene de forma asincrónica los subcapítulos cuyo nombre de capítulo coincide con el criterio especificado
        /// ejecutando el procedimiento almacenado sp_subcapitulo_Obtener.
        /// </summary>
        /// <remarks>Ejecuta el procedimiento almacenado sp_subcapitulo_Obtener mediante FromSqlRaw,
        /// mapeando parámetros a DBNull.Value cuando corresponde y devolviendo entidades sin seguimiento
        /// (AsNoTracking).</remarks>
        /// <param name="nombreCapitulo">Nombre del capítulo para filtrar la búsqueda; si es nulo o solo contiene espacios se omite el filtro.</param>
        /// <returns>Tarea cuyo resultado es una lista de SubCapitulo obtenidos; puede ser una lista vacía.</returns>
        public async Task<List<SubCapitulo>> ObtenerSubCapitulosPorNombreCapituloAsync(string nombreCapitulo)
        {
            var pId = new SqlParameter("@id", SqlDbType.Int) { Value = DBNull.Value };
            var pIdCapitulo = new SqlParameter("@id_capitulo", SqlDbType.Int) { Value = DBNull.Value };
            var pBuscar = new SqlParameter("@buscar", string.IsNullOrWhiteSpace(nombreCapitulo) ? (object)DBNull.Value : nombreCapitulo);

            return await _context.SubCapitulos
                .FromSqlRaw("EXEC sp_subcapitulo_Obtener @id = @id, @id_capitulo = @id_capitulo, @buscar = @buscar", pId, pIdCapitulo, pBuscar)
                .AsNoTracking()
                .ToListAsync();
        }

        /// <summary>
        /// Obtiene una lista de SubCapitulo que coinciden con el código de subcapítulo proporcionado.
        /// </summary>
        /// <remarks>Ejecuta el procedimiento almacenado 'sp_subcapitulo_Obtener' mediante FromSqlRaw,
        /// pasando parámetros para id e id_capitulo como nulos y usando DBNull.Value para el parámetro de búsqueda
        /// cuando el código es nulo o vacío. La consulta se realiza con AsNoTracking y se materializa con
        /// ToListAsync.</remarks>
        /// <param name="codigoSubCapitulo">Código de subcapítulo usado como criterio de búsqueda. Si es null, vacío o solo espacios, se omite el
        /// filtro.</param>
        /// <returns>Tarea que representa la operación asincrónica. El resultado contiene una lista de SubCapitulo.</returns>
        public async Task<List<SubCapitulo>> ObtenerSubCapitulosPorCodigoSubCapituloAsync(string codigoSubCapitulo)
        {
            var pId = new SqlParameter("@id", SqlDbType.Int) { Value = DBNull.Value };
            var pIdCapitulo = new SqlParameter("@id_capitulo", SqlDbType.Int) { Value = DBNull.Value };
            var pBuscar = new SqlParameter("@buscar", string.IsNullOrWhiteSpace(codigoSubCapitulo) ? (object)DBNull.Value : codigoSubCapitulo);

            return await _context.SubCapitulos
                .FromSqlRaw("EXEC sp_subcapitulo_Obtener @id = @id, @id_capitulo = @id_capitulo, @buscar = @buscar", pId, pIdCapitulo, pBuscar)
                .AsNoTracking()
                .ToListAsync();
        }

        /// <summary>
        /// Obtiene los subcapítulos cuyo nombre coincide parcial o totalmente con el texto proporcionado mediante el
        /// procedimiento almacenado sp_subcapitulo_Obtener.
        /// </summary>
        /// <remarks>Ejecuta el procedimiento almacenado sp_subcapitulo_Obtener mediante FromSqlRaw con
        /// los parámetros @id y @id_capitulo nulos y @buscar igual al valor proporcionado o DBNull.Value; aplica
        /// AsNoTracking y devuelve el resultado de forma asíncrona.</remarks>
        /// <param name="nombreSubCapitulo">Texto completo o parcial para filtrar por nombre; si es null, vacío o contiene solo espacios se ignora el
        /// filtro.</param>
        /// <returns>Lista de SubCapitulo recuperados de la base de datos; puede estar vacía si no hay coincidencias.</returns>
        public async Task<List<SubCapitulo>> ObtenerSubCapitulosPorNombreSubCapituloAsync(string nombreSubCapitulo)
        {
            var pId = new SqlParameter("@id", SqlDbType.Int) { Value = DBNull.Value };
            var pIdCapitulo = new SqlParameter("@id_capitulo", SqlDbType.Int) { Value = DBNull.Value };
            var pBuscar = new SqlParameter("@buscar", string.IsNullOrWhiteSpace(nombreSubCapitulo) ? (object)DBNull.Value : nombreSubCapitulo);

            return await _context.SubCapitulos
                .FromSqlRaw("EXEC sp_subcapitulo_Obtener @id = @id, @id_capitulo = @id_capitulo, @buscar = @buscar", pId, pIdCapitulo, pBuscar)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
