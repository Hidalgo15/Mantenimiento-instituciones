using Mantenimiento.Core.Application.DTOs.AnalistaSupervisor;
using Mantenimiento.Core.Domain.Entities;

namespace Mantenimiento.Core.Application.InterfaceServices
{
    public interface IAnalistaSupervisorService
    {
        Task<List<AnalistaSupervisorConsultaDto>> ObtenerMantenimientoAsync(string? nombreAnalista, string? estadoAnalista, string? despachoSupervisor);
        Task<List<SupervisorOptionDto>> ObtenerSupervisoresDisponiblesAsync();
        Task AsignarAsync(AsignarSupervisorDto dto);
        Task ReasignarAsync(AsignarSupervisorDto dto);
        Task DesvincularAsync(DesvincularAnalistaDto dto);
    }
}
