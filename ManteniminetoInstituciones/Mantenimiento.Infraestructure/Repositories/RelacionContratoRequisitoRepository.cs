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

        public async Task<List<RelacionContratoRequisito>> ObtenerAsync(int? codigoClasificacion, int? codigoRequisito)
        {
            var pClasificacion = new SqlParameter("@CodigoClasificacion", (object?)codigoClasificacion ?? DBNull.Value);
            var pRequisito = new SqlParameter("@CodigoRequisito", (object?)codigoRequisito ?? DBNull.Value);

            return await _context.Set<RelacionContratoRequisito>()
                .FromSqlRaw("EXEC [dbo].[sp_Obtener_Relacion_Contrato_Requisito] @CodigoClasificacion, @CodigoRequisito", pClasificacion, pRequisito)
                .AsNoTracking()
                .ToListAsync();
        }

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

        public async Task<ResultadoOperacion> EliminarAsync(int id)
        {
            var pId = new SqlParameter("@Id", id);

            var resultado = await _context.Set<ResultadoOperacion>()
                .FromSqlRaw("EXEC [dbo].[sp_Eliminar_Relacion_Contrato_Requisito] @Id", pId)
                .AsNoTracking()
                .ToListAsync();

            return resultado.FirstOrDefault() ?? new ResultadoOperacion { CodigoResultado = 0, Mensaje = "Error desconocido al eliminar la relación." };
        }

        public async Task<List<CatalogoClasificacionContrato>> ObtenerCatalogoClasificacionAsync()
        {
            var lista = await _context.Set<CatalogoClasificacionContrato>()
                .FromSqlRaw("EXEC [dbo].[sp_Obtener_Catalogo_Clasificacion_Contratos]")
                .AsNoTracking()
                .ToListAsync();

            // Filtramos para omitir registros que no tengan tipo de contrato u operación válidos
            //return lista.Where(c => !string.IsNullOrWhiteSpace(c.ContratoTipo) && !string.IsNullOrWhiteSpace(c.TipoOperacion)).ToList();
            return lista;
        }

        public async Task<List<CatalogoRequisitoContrato>> ObtenerCatalogoRequisitosAsync()
        {
            return await _context.Set<CatalogoRequisitoContrato>()
                .FromSqlRaw("EXEC [dbo].[sp_Obtener_Catalogo_Requisitos_Contratos]")
                .AsNoTracking()
                .ToListAsync();
        }
    }
}