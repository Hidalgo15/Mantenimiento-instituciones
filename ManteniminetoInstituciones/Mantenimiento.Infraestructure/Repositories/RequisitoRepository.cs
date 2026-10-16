using Mantenimiento.Core.Domain.Entities;
using Mantenimiento.Core.Domain.RepositoryInterfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Mantenimiento.Infraestructure.Persistence.Repositories
{
    public class RequisitoRepository : IRequisitoRepository
    {
        private readonly ApplicationDbContext _context;

        public RequisitoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtiene una lista de requisitos desde la base de datos utilizando un procedimiento almacenado.
        /// </summary>
        /// <param name="busqueda"></param>
        /// <returns></returns>
        public async Task<List<Requisito>> ObtenerRequisitosAsync(string? busqueda)
        {
            var pBusqueda = new SqlParameter("@Busqueda", (object?)busqueda ?? DBNull.Value);

            return await _context.Set<Requisito>()
                .FromSqlRaw("EXEC [ct].[sp_Obtener_Requisitos] @Busqueda", pBusqueda)
                .AsNoTracking()
                .ToListAsync();
        }

        /// <summary>
        /// Crea un requisito en la base de datos ejecutando el procedimiento almacenado [ct].[sp_Crear_Requisito] y
        /// devuelve el resultado de la operación.
        /// </summary>
        /// <remarks>Utiliza EF Core FromSqlRaw para ejecutar el procedimiento almacenado y AsNoTracking
        /// para no rastrear la entidad de resultado. Devuelve el mensaje proporcionado por el procedimiento o un
        /// mensaje por defecto si es nulo.</remarks>
        /// <param name="requisitoDescripcion">Descripción del requisito a crear.</param>
        /// <returns>Un ResultadoOperacion con CodigoResultado = 1 si la creación fue satisfactoria (RequisitosID > 0), o
        /// CodigoResultado = 0 y un mensaje de error en caso contrario.</returns>
        public async Task<ResultadoOperacion> CrearAsync(string requisitoDescripcion)
        {
            var pDesc = new SqlParameter("@RequisitoDescripcion", requisitoDescripcion);

            var resultado = await _context.Set<RequisitoCreacionResultado>()
                .FromSqlRaw("EXEC [ct].[sp_Crear_Requisito] @RequisitoDescripcion", pDesc)
                .AsNoTracking()
                .ToListAsync();

            var res = resultado.FirstOrDefault();

            return new ResultadoOperacion
            {
                CodigoResultado = res != null && res.RequisitosID > 0 ? 1 : 0,
                Mensaje = res?.Mensaje ?? "Error desconocido al crear el requisito.",
            };
        }

        /// <summary>
        /// Actualiza un requisito en la base de datos ejecutando el procedimiento almacenado [ct].[sp_Actualizar_Requisito] y
        /// devuelve el resultado de la operación.
        /// </summary>
        /// <remarks>Utiliza EF Core FromSqlRaw para ejecutar el procedimiento almacenado y AsNoTracking
        /// para no rastrear la entidad de resultado.</remarks>
        /// <param name="requisitosID">Identificador del requisito a actualizar.</param>
        /// <param name="requisitoDescripcion">Nueva descripción del requisito.</param>
        /// <returns>Un ResultadoOperacion con CodigoResultado = 1 si la actualización fue satisfactoria, o
        /// CodigoResultado = 0 y un mensaje de error en caso contrario.</returns>
        public async Task<ResultadoOperacion> ActualizarAsync(int requisitosID, string requisitoDescripcion)
        {
            var pId = new SqlParameter("@RequisitosID", requisitosID);
            var pDesc = new SqlParameter("@RequisitoDescripcion", requisitoDescripcion);

            var resultado = await _context.Set<ResultadoOperacion>()
                .FromSqlRaw("EXEC [ct].[sp_Actualizar_Requisito] @RequisitosID, @RequisitoDescripcion", pId, pDesc)
                .AsNoTracking()
                .ToListAsync();

            return resultado.FirstOrDefault() ?? new ResultadoOperacion { CodigoResultado = 0, Mensaje = "Error desconocido al actualizar." };
        }

        /// <summary>
        /// Elimina un requisito por su identificador y devuelve el resultado de la operación.
        /// </summary>
        /// <remarks>Ejecuta el procedimiento almacenado [ct].[sp_Eliminar_Requisito] mediante EF Core y
        /// realiza la consulta sin seguimiento.</remarks>
        /// <param name="requisitosID">Identificador del requisito a eliminar.</param>
        /// <returns>ResultadoOperacion que contiene el código y el mensaje del resultado; si no se obtiene resultado, se
        /// devuelve un objeto con CodigoResultado = 0 y mensaje de error.</returns>
        public async Task<ResultadoOperacion> EliminarAsync(int requisitosID)
        {
            var pId = new SqlParameter("@RequisitosID", requisitosID);

            var resultado = await _context.Set<ResultadoOperacion>()
                .FromSqlRaw("EXEC [ct].[sp_Eliminar_Requisito] @RequisitosID", pId)
                .AsNoTracking()
                .ToListAsync();

            return resultado.FirstOrDefault() ?? new ResultadoOperacion { CodigoResultado = 0, Mensaje = "Error desconocido al eliminar." };
        }
    }
}