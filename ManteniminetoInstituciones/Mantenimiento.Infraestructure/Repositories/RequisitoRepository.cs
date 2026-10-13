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

        public async Task<List<Requisito>> ObtenerRequisitosAsync(string? busqueda)
        {
            var pBusqueda = new SqlParameter("@Busqueda", (object?)busqueda ?? DBNull.Value);

            return await _context.Set<Requisito>()
                .FromSqlRaw("EXEC [ct].[sp_Obtener_Requisitos] @Busqueda", pBusqueda)
                .AsNoTracking()
                .ToListAsync();
        }

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