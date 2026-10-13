using AutoMapper;
using Mantenimiento.Core.Application.DTOs.CuentaRequisito;
using Mantenimiento.Core.Application.DTOs.Resultado;
using Mantenimiento.Core.Domain.Entities;

namespace Mantenimiento.Core.Application.Mappings
{
    public class CuentaRequisitoProfile : Profile
    {
        public CuentaRequisitoProfile()
        {
            CreateMap<CuentaRequisito, CuentaRequisitoDto>().ReverseMap();
            CreateMap<ResultadoOperacion, ResultadoOperacionDto>().ReverseMap();
            CreateMap<CuentaCatalogo, CuentaCatalogoDto>().ReverseMap();
            CreateMap<RequisitoCatalogo, RequisitoCatalogoDto>().ReverseMap();
            CreateMap<CuentaRequisitoConsulta, CuentaRequisitoDto>();
        }
    }
}