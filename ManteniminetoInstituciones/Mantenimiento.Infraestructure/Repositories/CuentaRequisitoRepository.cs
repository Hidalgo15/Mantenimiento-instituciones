using Mantenimiento.Core.Domain.Entities;
using Mantenimiento.Core.Domain.RepositoryInterfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Mantenimiento.Infraestructure.Persistence.Repositories
{
    public class CuentaRequisitoRepository : ICuentaRequisitoRepository
    {
        private readonly ApplicationDbContext _context;

        public CuentaRequisitoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtiene las relaciones entre cuentas y requisitos según los filtros proporcionados.
        /// </summary>
        /// <remarks>Ejecuta el procedimiento almacenado [ct].[sp_Obtener_Relacion_Cuenta_Requisitos] con
        /// los parámetros @IdCuenta, @IdRequisito y @Estado y devuelve el resultado sin seguimiento
        /// (AsNoTracking).</remarks>
        /// <param name="idCuenta">Identificador de la cuenta; null para no filtrar por cuenta.</param>
        /// <param name="idRequisito">Identificador del requisito; null para no filtrar por requisito.</param>
        /// <param name="estado">Estado de la relación; null para no filtrar por estado.</param>
        /// <returns>Lista de CuentaRequisitoConsulta que cumplen los criterios especificados.</returns>
        public async Task<List<CuentaRequisitoConsulta>> ObtenerRelacionAsync(double? idCuenta, int? idRequisito, bool? estado)
        {
            var pIdCuenta = new SqlParameter("@IdCuenta", (object?)idCuenta ?? DBNull.Value);
            var pIdRequisito = new SqlParameter("@IdRequisito", (object?)idRequisito ?? DBNull.Value);
            var pEstado = new SqlParameter("@Estado", (object?)estado ?? DBNull.Value);

            return await _context.Set<CuentaRequisitoConsulta>()
                .FromSqlRaw("EXEC [ct].[sp_Obtener_Relacion_Cuenta_Requisitos] @IdCuenta, @IdRequisito, @Estado", pIdCuenta, pIdRequisito, pEstado)
                .AsNoTracking()
                .ToListAsync();
        }

        /// <summary>
        /// Asigna un requisito a una cuenta de forma asíncrona mediante el procedimiento almacenado
        /// ct.sp_Asignar_Cuenta_Requisito.
        /// </summary>
        /// <remarks>Devuelve un ResultadoOperacion por defecto (CódigoResultado = 0, Mensaje = "Error
        /// desconocido.") si la consulta no retorna resultados.</remarks>
        /// <param name="idCuenta">Identificador de la cuenta.</param>
        /// <param name="idRequisito">Identificador del requisito.</param>
        /// <param name="estado">Estado de asignación; true para asignar, false para desasignar.</param>
        /// <returns>Un ResultadoOperacion con el código y el mensaje que describen el resultado de la operación.</returns>
        public async Task<ResultadoOperacion> AsignarAsync(double idCuenta, int idRequisito, bool estado)
        {
            var pIdCuenta = new SqlParameter("@IdCuenta", idCuenta);
            var pIdRequisito = new SqlParameter("@IdRequisito", idRequisito);
            var pEstado = new SqlParameter("@Estado", estado);

            var resultado = await _context.Set<ResultadoOperacion>()
                .FromSqlRaw("EXEC [ct].[sp_Asignar_Cuenta_Requisito] @IdCuenta, @IdRequisito, @Estado", pIdCuenta, pIdRequisito, pEstado)
                .AsNoTracking()
                .ToListAsync();

            return resultado.FirstOrDefault() ?? new ResultadoOperacion { CodigoResultado = 0, Mensaje = "Error desconocido." };
        }

        /// <summary>
        /// Actualiza la relación entre una cuenta y un requisito mediante un procedimiento almacenado y devuelve el
        /// resultado de la operación.
        /// </summary>
        /// <remarks>Ejecuta el procedimiento almacenado [ct].[sp_Actualizar_Relacion_Cuenta_Requisitos]
        /// con los parámetros proporcionados mediante FromSqlRaw y devuelve el primer resultado sin seguimiento
        /// (AsNoTracking).</remarks>
        /// <param name="cuentaRequisitosID">Identificador de la relación cuenta-requisito.</param>
        /// <param name="idCuenta">Identificador de la cuenta.</param>
        /// <param name="idRequisito">Identificador del requisito.</param>
        /// <param name="estado">Valor booleano que indica el estado de la relación.</param>
        /// <returns>Objeto ResultadoOperacion con el código y mensaje de la operación. Si no se obtiene resultado, devuelve un
        /// ResultadoOperacion con CodigoResultado = 0 y Mensaje = "Error desconocido.".</returns>
        public async Task<ResultadoOperacion> ActualizarAsync(int cuentaRequisitosID, double idCuenta, int idRequisito, bool estado)
        {
            var pId = new SqlParameter("@CuentaRequisitosID", cuentaRequisitosID);
            var pIdCuenta = new SqlParameter("@IdCuenta", idCuenta);
            var pIdRequisito = new SqlParameter("@IdRequisito", idRequisito);
            var pEstado = new SqlParameter("@Estado", estado);

            var resultado = await _context.Set<ResultadoOperacion>()
                .FromSqlRaw("EXEC [ct].[sp_Actualizar_Relacion_Cuenta_Requisitos] @CuentaRequisitosID, @IdCuenta, @IdRequisito, @Estado", pId, pIdCuenta, pIdRequisito, pEstado)
                .AsNoTracking()
                .ToListAsync();

            return resultado.FirstOrDefault() ?? new ResultadoOperacion { CodigoResultado = 0, Mensaje = "Error desconocido." };
        }

        /// <summary>
        /// Elimina la relación CuentaRequisitos en la base de datos mediante un procedimiento almacenado y devuelve el
        /// resultado de la operación.
        /// </summary>
        /// <remarks>Ejecuta el procedimiento almacenado [ct].[sp_Eliminar_Relacion_Cuenta_Requisitos] con
        /// el parámetro proporcionado; la consulta se realiza sin seguimiento y devuelve el primer resultado o un valor
        /// por defecto.</remarks>
        /// <param name="cuentaRequisitosID">Identificador de la relación CuentaRequisitos a eliminar.</param>
        /// <returns>ResultadoOperacion que indica el resultado de la eliminación; si no se obtiene respuesta, devuelve un
        /// ResultadoOperacion con CodigoResultado 0 y Mensaje 'Error desconocido'.</returns>
        public async Task<ResultadoOperacion> EliminarAsync(int cuentaRequisitosID)
        {
            var pId = new SqlParameter("@CuentaRequisitosID", cuentaRequisitosID);

            var resultado = await _context.Set<ResultadoOperacion>()
                .FromSqlRaw("EXEC [ct].[sp_Eliminar_Relacion_Cuenta_Requisitos] @CuentaRequisitosID", pId)
                .AsNoTracking()
                .ToListAsync();

            return resultado.FirstOrDefault() ?? new ResultadoOperacion { CodigoResultado = 0, Mensaje = "Error desconocido." };
        }

        /// <summary>
        /// Recupera de forma asíncrona el catálogo de cuentas ejecutando el procedimiento almacenado
        /// [ct].[sp_Obtener_Catalogo_ctCuentas].
        /// </summary>
        /// <remarks>Los registros se obtienen mediante FromSqlRaw ejecutando el procedimiento almacenado
        /// y se materializan con AsNoTracking, por lo que las entidades no quedan rastreadas por el
        /// DbContext.</remarks>
        /// <returns>Una tarea que representa la operación asíncrona. El resultado contiene una lista de CuentaCatalogo con las
        /// entidades obtenidas sin seguimiento.</returns>
        public async Task<List<CuentaCatalogo>> ObtenerCatalogoCuentasAsync()
        {
            return await _context.Set<CuentaCatalogo>()
                .FromSqlRaw("EXEC [ct].[sp_Obtener_Catalogo_ctCuentas]")
                .AsNoTracking()
                .ToListAsync();
        }

        /// <summary>
        /// Recupera de forma asíncrona el catálogo de requisitos ejecutando el procedimiento almacenado
        /// [ct].[sp_Obtener_Catalogo_ctRequisitos].
        /// </summary>
        /// <remarks>La consulta se ejecuta mediante FromSqlRaw y se materializa con AsNoTracking para
        /// lecturas sin seguimiento. Ejecuta el procedimiento almacenado en la base de datos y devuelve todos los
        /// registros del catálogo.</remarks>
        /// <returns>Una tarea que representa la operación asíncrona que devuelve una lista de RequisitoCatalogo.</returns>
        public async Task<List<RequisitoCatalogo>> ObtenerCatalogoRequisitosAsync()
        {
            return await _context.Set<RequisitoCatalogo>()
                .FromSqlRaw("EXEC [ct].[sp_Obtener_Catalogo_ctRequisitos]")
                .AsNoTracking()
                .ToListAsync();
        }
    }
}