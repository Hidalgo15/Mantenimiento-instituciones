using Mantenimiento.Core.Domain.Entities;
using Mantenimiento.Core.Domain.RepositoryInterfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Mantenimiento.Infraestructure.Persistence.Repositories
{
    public class CapituloRepository : ICapituloRepository
    {
        private readonly ApplicationDbContext _context;

        public CapituloRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtiene un capítulo por su ID utilizando un procedimiento almacenado.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        public async Task<Capitulo> GetByIdAsync(int id)
        {
            var paramId = new SqlParameter("@id", id);
            var result = await _context.Capitulos
                .FromSqlRaw("EXEC dbo.sp_capitulo_Obtener @id", paramId)
                .AsNoTracking()
                .ToListAsync();

            var capitulo = result.FirstOrDefault();
            if (capitulo == null)
            {
                throw new KeyNotFoundException($"No se encontró el capítulo con el ID {id}.");
            }

            return capitulo;
        }

        /// <summary>
        /// Obtiene una lista de capítulos utilizando un procedimiento almacenado, 
        /// con la opción de filtrar por un término de búsqueda.
        /// </summary>
        /// <param name="buscar"></param>
        /// <returns></returns>
        public async Task<List<Capitulo>> ObtenerCapitulosAsync()
        {
            return await _context.Capitulos
                .FromSqlRaw("EXEC dbo.sp_capitulo_Obtener")
                .AsNoTracking()
                .ToListAsync();
        }


        /// <summary>
        /// Inserta un nuevo capítulo utilizando un procedimiento almacenado 
        /// y devuelve el ID generado.
        /// </summary>
        /// <param name="capitulo"></param>
        /// <returns></returns>
        public async Task<int> InsertarConSpAsync(Capitulo capitulo)
        {
            var pCodigo = new SqlParameter("@codigo_capitulo", capitulo.CodigoCapitulo ?? (object)DBNull.Value);
            var pNombre = new SqlParameter("@capitulo", capitulo.NombreCapitulo ?? (object)DBNull.Value);
            var pIdGenerado = new SqlParameter
            {
                ParameterName = "@id_generado",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.Output
            };

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC dbo.sp_capitulo_Insertar @codigo_capitulo, @capitulo, @id_generado OUTPUT",
                pCodigo, pNombre, pIdGenerado);

            return (int)pIdGenerado.Value;
        }

        /// <summary>
        /// Actualiza un capítulo existente utilizando un procedimiento almacenado.
        /// </summary>
        /// <param name="capitulo"></param>
        /// <returns></returns>
        public async Task ActualizarConSpAsync(Capitulo capitulo)
        {
            var pId = new SqlParameter("@id", capitulo.Id);
            var pCodigo = new SqlParameter("@codigo_capitulo", capitulo.CodigoCapitulo ?? (object)DBNull.Value);
            var pNombre = new SqlParameter("@capitulo", capitulo.NombreCapitulo ?? (object)DBNull.Value);

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC dbo.sp_capitulo_Actualizar @id, @codigo_capitulo, @capitulo",
                pId, pCodigo, pNombre);
        }

        /// <summary>
        /// Elimina un capítulo existente utilizando un procedimiento almacenado.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task EliminarConSpAsync(int id)
        {
            var pId = new SqlParameter("@id", id);
            await _context.Database.ExecuteSqlRawAsync("EXEC dbo.sp_capitulo_Eliminar @id", pId);
        }
    }
}