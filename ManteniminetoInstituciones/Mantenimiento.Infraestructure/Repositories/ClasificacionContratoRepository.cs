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

        public async Task EliminarAsync(int codigoClasificacion)
        {
            var pId = new SqlParameter("@codigo_clasificacion", codigoClasificacion);

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC [dbo].[sp_clasificacion_contratos_Eliminar] @codigo_clasificacion", pId);
        }

        public async Task<List<ProcesoSeleccionCatalogo>> ObtenerCatalogoProcesoSeleccionAsync()
        {
            return await _context.Set<ProcesoSeleccionCatalogo>()
                .FromSqlRaw("EXEC [dbo].[sp_Obtener_Catalogo_Proceso_Seleccion]")
                .AsNoTracking()
                .ToListAsync();
        }

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