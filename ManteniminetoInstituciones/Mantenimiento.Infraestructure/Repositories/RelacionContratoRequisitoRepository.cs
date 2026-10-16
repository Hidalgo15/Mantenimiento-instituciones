using Mantenimiento.Core.Domain.Entities;
using Mantenimiento.Core.Domain.Entities.RelacionContratoRequisito;
using Mantenimiento.Core.Domain.RepositoryInterfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Mantenimiento.Infraestructure.Persistence.Repositories
{
    public class RelacionContratoRequisitoRepository : IRelacionContratoRequisitoRepository
    {
        private readonly ApplicationDbContext _context;

        public RelacionContratoRequisitoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtiene la lista de relaciones entre contratos y requisitos según los parámetros proporcionados.
        /// </summary>
        /// <param name="codigoClasificacion"></param>
        /// <param name="codigoRequisito"></param>
        /// <returns></returns>
        public async Task<List<RelacionContratoRequisito>> ObtenerAsync(int? codigoClasificacion, int? codigoRequisito)
        {
            var pClasificacion = new SqlParameter("@CodigoClasificacion", (object?)codigoClasificacion ?? DBNull.Value);
            var pRequisito = new SqlParameter("@CodigoRequisito", (object?)codigoRequisito ?? DBNull.Value);

            return await _context.Set<RelacionContratoRequisito>()
                .FromSqlRaw("EXEC [dbo].[sp_Obtener_Relacion_Contrato_Requisito] @CodigoClasificacion, @CodigoRequisito", pClasificacion, pRequisito)
                .AsNoTracking()
                .ToListAsync();
        }

        /// <summary>
        /// Asigna de forma asíncrona la relación entre una clasificación y un requisito mediante el procedimiento
        /// almacenado sp_Asignar_Contrato_Requisito.
        /// </summary>
        /// <remarks>Ejecuta el procedimiento almacenado mediante DbContext.FromSqlRaw, aplica
        /// AsNoTracking y materializa el primer resultado devuelto.</remarks>
        /// <param name="codigoClasificacion">Código de la clasificación a asociar.</param>
        /// <param name="codigoRequisito">Código del requisito a asociar.</param>
        /// <returns>Objeto Task cuyo resultado es un ResultadoOperacion que indica el éxito o fallo de la operación; si el
        /// procedimiento no devuelve resultados, se retorna un ResultadoOperacion con CodigoResultado = 0 y Mensaje =
        /// Error desconocido al asignar la relación.</returns>
        public async Task<ResultadoOperacion> AsignarAsync(int codigoClasificacion, int codigoRequisito)
        {
            var pClasificacion = new SqlParameter("@CodigoClasificacion", codigoClasificacion);
            var pRequisito = new SqlParameter("@CodigoRequisito", codigoRequisito);

            var resultado = await _context.Set<ResultadoOperacion>()
                .FromSqlRaw("EXEC [dbo].[sp_Asignar_Contrato_Requisito] @CodigoClasificacion, @CodigoRequisito", pClasificacion, pRequisito)
                .AsNoTracking()
                .ToListAsync();

            return resultado.FirstOrDefault() ?? new ResultadoOperacion { CodigoResultado = 0, Mensaje = "Error desconocido al asignar la relación." };
        }

        /// <summary>
        /// Ejecuta de forma asíncrona el procedimiento almacenado sp_Actualizar_Relacion_Contrato_Requisito para
        /// actualizar la relación entre contrato y requisito y devuelve el resultado de la operación.
        /// </summary>
        /// <remarks>El procedimiento se ejecuta mediante FromSqlRaw con parámetros y AsNoTracking; se
        /// extrae el primer registro devuelto por la consulta.</remarks>
        /// <param name="id">Identificador de la relación a actualizar.</param>
        /// <param name="codigoClasificacion">Código de clasificación asociado a la relación.</param>
        /// <param name="codigoRequisito">Código del requisito asociado a la relación.</param>
        /// <returns>ResultadoOperacion que representa el resultado de la actualización; si no se obtiene resultado devuelve un
        /// ResultadoOperacion con CodigoResultado = 0 y mensaje de error desconocido.</returns>
        public async Task<ResultadoOperacion> ActualizarAsync(int id, int codigoClasificacion, int codigoRequisito)
        {
            var pId = new SqlParameter("@Id", id);
            var pClasificacion = new SqlParameter("@CodigoClasificacion", codigoClasificacion);
            var pRequisito = new SqlParameter("@CodigoRequisito", codigoRequisito);

            var resultado = await _context.Set<ResultadoOperacion>()
                .FromSqlRaw("EXEC [dbo].[sp_Actualizar_Relacion_Contrato_Requisito] @Id, @CodigoClasificacion, @CodigoRequisito", pId, pClasificacion, pRequisito)
                .AsNoTracking()
                .ToListAsync();

            return resultado.FirstOrDefault() ?? new ResultadoOperacion { CodigoResultado = 0, Mensaje = "Error desconocido al actualizar la relación." };
        }

        /// <summary>
        /// Elimina la relación entre contrato y requisito mediante el procedimiento almacenado
        /// dbo.sp_Eliminar_Relacion_Contrato_Requisito.
        /// </summary>
        /// <remarks>La ejecución se realiza mediante FromSqlRaw con el parámetro @Id y AsNoTracking; se
        /// devuelve el primer registro devuelto por el procedimiento almacenado.</remarks>
        /// <param name="id">Identificador de la relación a eliminar.</param>
        /// <returns>ResultadoOperacion con el resultado de la operación; si no se obtiene resultado devuelve un
        /// ResultadoOperacion con CodigoResultado = 0 y un mensaje de error.</returns>
        public async Task<ResultadoOperacion> EliminarAsync(int id)
        {
            var pId = new SqlParameter("@Id", id);

            var resultado = await _context.Set<ResultadoOperacion>()
                .FromSqlRaw("EXEC [dbo].[sp_Eliminar_Relacion_Contrato_Requisito] @Id", pId)
                .AsNoTracking()
                .ToListAsync();

            return resultado.FirstOrDefault() ?? new ResultadoOperacion { CodigoResultado = 0, Mensaje = "Error desconocido al eliminar la relación." };
        }

        /// <summary>
        /// Obtiene el catálogo de clasificaciones de contratos ejecutando el procedimiento almacenado
        /// 'sp_Obtener_Catalogo_Clasificacion_Contratos' y devuelve los resultados materializados en una lista de
        /// CatalogoClasificacionContrato.
        /// </summary>
        /// <remarks>Los registros se recuperan mediante FromSqlRaw y se consultan con AsNoTracking. La
        /// lógica de filtrado por tipo de contrato o tipo de operación está presente en el código pero actualmente está
        /// comentada y no se aplica.</remarks>
        /// <returns>Una tarea que representa la operación asincrónica. El resultado contiene la lista de
        /// CatalogoClasificacionContrato obtenida de la base de datos.</returns>
        public async Task<List<CatalogoClasificacionContrato>> ObtenerCatalogoClasificacionAsync()
        {
            var lista = await _context.Set<CatalogoClasificacionContrato>()
                .FromSqlRaw("EXEC [dbo].[sp_Obtener_Catalogo_Clasificacion_Contratos]")
                .AsNoTracking()
                .ToListAsync();

            // Filtro para omitir registros que no tengan tipo de contrato u operación válidos
            //return lista.Where(c => !string.IsNullOrWhiteSpace(c.ContratoTipo) && !string.IsNullOrWhiteSpace(c.TipoOperacion)).ToList();
            return lista;
        }

        /// <summary>
        /// Obtiene de forma asíncrona el catálogo de requisitos de contratos ejecutando el procedimiento almacenado
        /// sp_Obtener_Catalogo_Requisitos_Contratos y devuelve una lista de CatalogoRequisitoContrato.
        /// </summary>
        /// <remarks>Los datos se obtienen mediante FromSqlRaw y AsNoTracking; las entidades devueltas no
        /// están rastreadas por el contexto. Requiere que el procedimiento almacenado
        /// sp_Obtener_Catalogo_Requisitos_Contratos exista y tenga los permisos necesarios.</remarks>
        /// <returns>Tarea cuyo resultado es una lista de CatalogoRequisitoContrato recuperados desde la base de datos sin
        /// seguimiento por el DbContext.</returns>
        public async Task<List<CatalogoRequisitoContrato>> ObtenerCatalogoRequisitosAsync()
        {
            return await _context.Set<CatalogoRequisitoContrato>()
                .FromSqlRaw("EXEC [dbo].[sp_Obtener_Catalogo_Requisitos_Contratos]")
                .AsNoTracking()
                .ToListAsync();
        }
    }
}