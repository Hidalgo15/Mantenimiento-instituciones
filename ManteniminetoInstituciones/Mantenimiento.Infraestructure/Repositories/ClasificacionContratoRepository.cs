using Mantenimiento.Core.Domain.Entities.ClasificacionContrato;
using Mantenimiento.Core.Domain.RepositoryInterfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Mantenimiento.Infraestructure.Persistence.Repositories
{
    public class ClasificacionContratoRepository : IClasificacionContratoRepository
    {
        private readonly ApplicationDbContext _context;

        public ClasificacionContratoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtiene una lista de clasificaciones de contratos según los parámetros proporcionados.
        /// </summary>
        /// <param name="codigoClasificacion"></param>
        /// <param name="contratoTipo"></param>
        /// <param name="tipoOperacion"></param>
        /// <param name="contratoProcesoSeleccion"></param>
        /// <returns></returns>
        public async Task<List<ClasificacionContrato>> ObtenerAsync(int? codigoClasificacion, string? contratoTipo, string? tipoOperacion, string? contratoProcesoSeleccion)
        {
            var pId = new SqlParameter("@codigo_clasificacion", (object?)codigoClasificacion ?? DBNull.Value);
            var pTipo = new SqlParameter("@contrato_tipo", (object?)contratoTipo ?? DBNull.Value);
            var pOperacion = new SqlParameter("@tipo_operacion", (object?)tipoOperacion ?? DBNull.Value);
            var pProceso = new SqlParameter("@contrato_proceso_seleccion", (object?)contratoProcesoSeleccion ?? DBNull.Value);

            return await _context.Set<ClasificacionContrato>()
                .FromSqlRaw("EXEC [dbo].[sp_clasificacion_contratos_Obtener] @codigo_clasificacion, @contrato_tipo, @tipo_operacion, @contrato_proceso_seleccion", pId, pTipo, pOperacion, pProceso)
                .AsNoTracking()
                .ToListAsync();
        }

        /// <summary>
        /// Crea una clasificación de contrato llamando al procedimiento almacenado 'sp_clasificacion_contratos_Crear' y
        /// devuelve el código generado.
        /// </summary>
        /// <remarks>Ejecuta la instrucción SQL de forma asíncrona y obtiene el resultado a partir del
        /// parámetro de salida '@codigo_generado'.</remarks>
        /// <param name="contratoTipo">Tipo de contrato; puede ser null para enviar DBNull al procedimiento almacenado.</param>
        /// <param name="tipoOperacion">Tipo de operación; puede ser null para enviar DBNull al procedimiento almacenado.</param>
        /// <param name="contratoProcesoSeleccion">Proceso de selección del contrato; puede ser null para enviar DBNull al procedimiento almacenado.</param>
        /// <returns>Código generado por el procedimiento almacenado (entero).</returns>
        public async Task<int> CrearAsync(string? contratoTipo, string? tipoOperacion, string? contratoProcesoSeleccion)
        {
            var pTipo = new SqlParameter("@contrato_tipo", (object?)contratoTipo ?? DBNull.Value);
            var pOperacion = new SqlParameter("@tipo_operacion", (object?)tipoOperacion ?? DBNull.Value);
            var pProceso = new SqlParameter("@contrato_proceso_seleccion", (object?)contratoProcesoSeleccion ?? DBNull.Value);

            var pCodigoGenerado = new SqlParameter("@codigo_generado", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC [dbo].[sp_clasificacion_contratos_Crear] @contrato_tipo, @tipo_operacion, @contrato_proceso_seleccion, @codigo_generado OUTPUT",
                pTipo, pOperacion, pProceso, pCodigoGenerado);

            return Convert.ToInt32(pCodigoGenerado.Value);
        }

        /// <summary>
        /// Actualiza la clasificación de contratos en la base de datos ejecutando el procedimiento almacenado
        /// correspondiente.
        /// </summary>
        /// <remarks>Ejecuta el procedimiento almacenado [dbo].[sp_clasificacion_contratos_Actualizar]
        /// mediante ExecuteSqlRawAsync en el DbContext.</remarks>
        /// <param name="codigoClasificacion">Identificador de la clasificación a actualizar.</param>
        /// <param name="contratoTipo">Tipo de contrato; null se convierte en DBNull para omitir el valor.</param>
        /// <param name="tipoOperacion">Tipo de operación asociado al contrato; null se convierte en DBNull para omitir el valor.</param>
        /// <param name="contratoProcesoSeleccion">Proceso de selección del contrato; null se convierte en DBNull para omitir el valor.</param>
        /// <returns>Tarea que representa la operación asincrónica de actualización.</returns>
        public async Task ActualizarAsync(int codigoClasificacion, string? contratoTipo, string? tipoOperacion, string? contratoProcesoSeleccion)
        {
            var pId = new SqlParameter("@codigo_clasificacion", codigoClasificacion);
            var pTipo = new SqlParameter("@contrato_tipo", (object?)contratoTipo ?? DBNull.Value);
            var pOperacion = new SqlParameter("@tipo_operacion", (object?)tipoOperacion ?? DBNull.Value);
            var pProceso = new SqlParameter("@contrato_proceso_seleccion", (object?)contratoProcesoSeleccion ?? DBNull.Value);

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC [dbo].[sp_clasificacion_contratos_Actualizar] @codigo_clasificacion, @contrato_tipo, @tipo_operacion, @contrato_proceso_seleccion",
                pId, pTipo, pOperacion, pProceso);
        }

        /// <summary>
        /// Elimina la clasificación de contratos con el código especificado ejecutando el procedimiento almacenado
        /// [dbo].[sp_clasificacion_contratos_Eliminar].
        /// </summary>
        /// <remarks>Ejecuta SQL sin seguimiento de cambios mediante ExecuteSqlRawAsync; requiere permisos
        /// adecuados y puede producir excepciones de base de datos en caso de error.</remarks>
        /// <param name="codigoClasificacion">Código identificador de la clasificación de contrato a eliminar.</param>
        /// <returns>Tarea que representa la operación asincrónica.</returns>
        public async Task EliminarAsync(int codigoClasificacion)
        {
            var pId = new SqlParameter("@codigo_clasificacion", codigoClasificacion);

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC [dbo].[sp_clasificacion_contratos_Eliminar] @codigo_clasificacion", pId);
        }

        /// <summary>
        /// Obtiene de forma asíncrona el catálogo de procesos de selección desde la base de datos ejecutando el
        /// procedimiento almacenado 'sp_Obtener_Catalogo_Proceso_Seleccion'.
        /// </summary>
        /// <remarks>La consulta se ejecuta mediante FromSqlRaw y las entidades se devuelven sin
        /// seguimiento (AsNoTracking).</remarks>
        /// <returns>Una tarea que representa la operación asíncrona. El resultado es una lista de ProcesoSeleccionCatalogo con
        /// los elementos del catálogo.</returns>
        public async Task<List<ProcesoSeleccionCatalogo>> ObtenerCatalogoProcesoSeleccionAsync()
        {
            return await _context.Set<ProcesoSeleccionCatalogo>()
                .FromSqlRaw("EXEC [dbo].[sp_Obtener_Catalogo_Proceso_Seleccion]")
                .AsNoTracking()
                .ToListAsync();
        }

        /// <summary>
        /// Recupera de forma asíncrona las opciones de clasificación para el tipo especificado.
        /// </summary>
        /// <remarks>Ejecuta el procedimiento almacenado sp_clasificacion_contratos_ObtenerOpciones con el
        /// parámetro @tipo_opcion y devuelve las entidades sin seguimiento (AsNoTracking).</remarks>
        /// <param name="tipoOpcion">Cadena que indica el tipo de opción usado para filtrar las opciones de clasificación.</param>
        /// <returns>Tarea que representa la operación asíncrona y contiene la lista de ClasificacionOpcion correspondiente al
        /// tipo especificado.</returns>
        public async Task<List<ClasificacionOpcion>> ObtenerOpcionesAsync(string tipoOpcion)
        {
            var pTipo = new SqlParameter("@tipo_opcion", tipoOpcion);

            return await _context.Set<ClasificacionOpcion>()
                .FromSqlRaw("EXEC [dbo].[sp_clasificacion_contratos_ObtenerOpciones] @tipo_opcion", pTipo)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}