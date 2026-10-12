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

        public async Task<ResultadoOperacion> EliminarAsync(int cuentaRequisitosID)
        {
            var pId = new SqlParameter("@CuentaRequisitosID", cuentaRequisitosID);

            var resultado = await _context.Set<ResultadoOperacion>()
                .FromSqlRaw("EXEC [ct].[sp_Eliminar_Relacion_Cuenta_Requisitos] @CuentaRequisitosID", pId)
                .AsNoTracking()
                .ToListAsync();

            return resultado.FirstOrDefault() ?? new ResultadoOperacion { CodigoResultado = 0, Mensaje = "Error desconocido." };
        }

        public async Task<List<CuentaCatalogo>> ObtenerCatalogoCuentasAsync()
        {
            return await _context.Set<CuentaCatalogo>()
                .FromSqlRaw("EXEC [ct].[sp_Obtener_Catalogo_ctCuentas]")
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<RequisitoCatalogo>> ObtenerCatalogoRequisitosAsync()
        {
            return await _context.Set<RequisitoCatalogo>()
                .FromSqlRaw("EXEC [ct].[sp_Obtener_Catalogo_ctRequisitos]")
                .AsNoTracking()
                .ToListAsync();
        }
    }
}