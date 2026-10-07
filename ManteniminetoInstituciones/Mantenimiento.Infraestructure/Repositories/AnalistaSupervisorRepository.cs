using Mantenimiento.Core.Domain.Entities;
using Mantenimiento.Core.Domain.RepositoryInterfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Mantenimiento.Infraestructure.Persistence.Repositories
{
    public class AnalistaSupervisorRepository : IAnalistaSupervisorRepository
    {
        private readonly ApplicationDbContext _context;

        public AnalistaSupervisorRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtiene la lista de analistas y supervisores según los filtros proporcionados.
        /// </summary>
        /// <param name="nombreAnalista"></param>
        /// <param name="estadoAnalista"></param>
        /// <param name="despachoSupervisor"></param>
        /// <returns></returns>
        public async Task<List<AnalistaSupervisorConsulta>> ObtenerMantenimientoAsync(string? nombreAnalista, string? estadoAnalista, string? despachoSupervisor)
        {
            var pNombre = new SqlParameter("@NombreAnalista", (object?)nombreAnalista ?? DBNull.Value);
            var pEstado = new SqlParameter("@EstadoAnalista", (object?)estadoAnalista ?? DBNull.Value);
            var pSupervisor = new SqlParameter("@DespachoSupervisor", (object?)despachoSupervisor ?? DBNull.Value);

            return await _context.AnalistaSupervisorConsultas
                .FromSqlRaw("EXEC [dbo].[sp_Obtener_Mantenimiento_Analistas_Supervisores] @NombreAnalista, @EstadoAnalista, @DespachoSupervisor",
                    pNombre, pEstado, pSupervisor)
                .AsNoTracking()
                .ToListAsync();
        }

        /// <summary>
        /// Obtiene la lista de supervisores disponibles para asignación.
        /// </summary>
        /// <returns></returns>
        public async Task<List<SupervisorOption>> ObtenerSupervisoresDisponiblesAsync()
        {
            return await _context.SupervisorOptions
                .FromSqlRaw("EXEC [dbo].[sp_Obtener_Supervisores_Disponibles]")
                .AsNoTracking()
                .ToListAsync();
        }

        /// <summary>
        /// Asigna un supervisor a un analista mediante el procedimiento almacenado correspondiente.
        /// </summary>
        /// <param name="despachoAnalista"></param>
        /// <param name="despachoSupervisor"></param>
        /// <returns></returns>
        public async Task AsignarSupervisorAsync(string despachoAnalista, string despachoSupervisor)
        {
            var pAnalista = new SqlParameter("@DespachoAnalista", despachoAnalista);
            var pSupervisor = new SqlParameter("@DespachoSupervisor", despachoSupervisor);

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC [dbo].[sp_Asignar_Supervisor_Analista] @DespachoAnalista, @DespachoSupervisor",
                pAnalista, pSupervisor);
        }

        /// <summary>
        /// Reasigna un supervisor a un analista mediante el procedimiento almacenado correspondiente.
        /// </summary>
        /// <param name="despachoAnalista"></param>
        /// <param name="nuevoDespachoSupervisor"></param>
        /// <returns></returns>
        public async Task ReasignarSupervisorAsync(string despachoAnalista, string nuevoDespachoSupervisor)
        {
            var pAnalista = new SqlParameter("@DespachoAnalista", despachoAnalista);
            var pNuevoSupervisor = new SqlParameter("@NuevoDespachoSupervisor", nuevoDespachoSupervisor);

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC [dbo].[sp_Reasignar_Supervisor_Analista] @DespachoAnalista, @NuevoDespachoSupervisor",
                pAnalista, pNuevoSupervisor);
        }

        /// <summary>
        /// Elimina la asignación de un supervisor a un analista mediante el procedimiento almacenado correspondiente.
        /// </summary>
        /// <param name="despachoAnalista"></param>
        /// <returns></returns>
        public async Task EliminarAsignacionAsync(string despachoAnalista)
        {
            var pAnalista = new SqlParameter("@DespachoAnalista", despachoAnalista);

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC [dbo].[sp_Eliminar_Asignacion_Analista] @DespachoAnalista",
                pAnalista);
        }
    }
}