using Mantenimiento.Core.Application.DTOs.AnalistaSupervisor;
using Mantenimiento.Core.Domain.Entities;

namespace Mantenimiento.Core.Application.Mappings
{
    public static class AnalistaSupervisorMapper
    {
        public static AnalistaSupervisorConsultaDto ToDto(AnalistaSupervisorConsulta entity) => new()
        {
            DespachoAnalista = entity.Despacho_Analista,
            NombreAnalista = entity.Nombre_Analista,
            EstadoAnalista = entity.Estado_Analista,
            DespachoSupervisor = entity.Despacho_Supervisor,
            NombreSupervisor = entity.Nombre_Supervisor
        };

        public static SupervisorOptionDto ToDto(SupervisorOption entity) => new()
        {
            DespachoSupervisor = entity.Despacho_Supervisor,
            NombreSupervisor = entity.Nombre_Supervisor
        };
    }
}