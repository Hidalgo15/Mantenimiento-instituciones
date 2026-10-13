using AutoMapper;
using Mantenimiento.Core.Application.DTOs.Requisito;
using Mantenimiento.Core.Application.DTOs.Resultado;
using Mantenimiento.Core.Domain.Entities;

namespace Mantenimiento.Core.Application.Mappings
{
    public class RequisitoProfile : Profile
    {
        public RequisitoProfile()
        {
            CreateMap<Requisito, RequisitoDto>().ReverseMap();
            CreateMap<ResultadoOperacion, ResultadoOperacionDto>().ReverseMap();
        }
    }
}