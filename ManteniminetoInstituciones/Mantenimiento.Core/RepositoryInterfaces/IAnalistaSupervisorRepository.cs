using Mantenimiento.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mantenimiento.Core.Domain.RepositoryInterfaces
{
    public interface IAnalistaSupervisorRepository
    {
        Task<List<AnalistaSupervisorConsulta>> ObtenerMantenimientoAsync(string? nombreAnalista, string? estadoAnalista, string? despachoSupervisor);
        Task<List<SupervisorOption>> ObtenerSupervisoresDisponiblesAsync();
        Task AsignarSupervisorAsync(string despachoAnalista, string despachoSupervisor);
        Task ReasignarSupervisorAsync(string despachoAnalista, string nuevoDespachoSupervisor);
        Task EliminarAsignacionAsync(string despachoAnalista);
    }
}
