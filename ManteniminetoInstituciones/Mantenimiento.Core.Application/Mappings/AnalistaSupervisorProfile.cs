using AutoMapper;
using Mantenimiento.Core.Application.DTOs.AnalistaSupervisor;
using Mantenimiento.Core.Domain.Entities;

namespace Mantenimiento.Core.Application.Mappings
{
    public class AnalistaSupervisorProfile : Profile
    {
        public AnalistaSupervisorProfile()
        {
            // Mapeo para la vista principal
            CreateMap<AnalistaSupervisorConsulta, AnalistaSupervisorConsultaDto>()
                .ForMember(dest => dest.DespachoAnalista, opt => opt.MapFrom(src => src.Despacho_Analista))
                .ForMember(dest => dest.NombreAnalista, opt => opt.MapFrom(src => src.Nombre_Analista))
                .ForMember(dest => dest.EstadoAnalista, opt => opt.MapFrom(src => src.Estado_Analista))
                .ForMember(dest => dest.DespachoSupervisor, opt => opt.MapFrom(src => src.Despacho_Supervisor))
                .ForMember(dest => dest.NombreSupervisor, opt => opt.MapFrom(src => src.Nombre_Supervisor));

            // Mapeo para el desplegable de supervisores
            CreateMap<SupervisorOption, SupervisorOptionDto>()
                .ForMember(dest => dest.DespachoSupervisor, opt => opt.MapFrom(src => src.Despacho_Supervisor))
                .ForMember(dest => dest.NombreSupervisor, opt => opt.MapFrom(src => src.Nombre_Supervisor));
        }
    }
}