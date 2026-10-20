using Mantenimiento.Core.Application.DTOs.ResponsableActividad;
using Mantenimiento.Core.Application.DTOs.Resultado;
using Mantenimiento.Core.Application.InterfaceServices;
using Mantenimiento.Core.Application.Mappings;
using Mantenimiento.Core.Domain.RepositoryInterfaces;

namespace Mantenimiento.Core.Application.Services
{
    public class ResponsableActividadService : IResponsableActividadService
    {
        private readonly IResponsableActividadRepository _repository;

        public ResponsableActividadService(IResponsableActividadRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<MoldeCatalogoDto>> ObtenerCatalogosMoldesAsync()
        {
            var data = await _repository.ObtenerCatalogosMoldesAsync();
            return data.Select(ResponsableActividadMapper.ToDto).ToList();
        }

        public async Task<List<SubtipoMoldeDto>> ObtenerSubtiposPorMoldeAsync(int codigoTre)
        {
            var data = await _repository.ObtenerSubtiposPorMoldeAsync(codigoTre);
            return data.Select(ResponsableActividadMapper.ToDto).ToList();
        }

        public async Task<List<UsuarioSubtipoDto>> ObtenerUsuariosPorSubtiposAsync(string codigosSubtipos)
        {
            var data = await _repository.ObtenerUsuariosPorSubtiposAsync(codigosSubtipos);
            return data.Select(ResponsableActividadMapper.ToDto).ToList();
        }

        public async Task<List<ActividadSubtipoDto>> ObtenerActividadesPorSubtiposAsync(string codigosSubtipos)
        {
            var data = await _repository.ObtenerActividadesPorSubtiposAsync(codigosSubtipos);
            return data.Select(ResponsableActividadMapper.ToDto).ToList();
        }

        public async Task<List<ResponsableActividadDto>> ObtenerActividadesUsuarioAsync(string codigoDespa)
        {
            var data = await _repository.ObtenerActividadesUsuarioAsync(codigoDespa);
            return data.Select(ResponsableActividadMapper.ToDto).ToList();
        }

        public async Task<ResultadoOperacionDto> GestionarActividadesAsync(GestionarResponsableActividadDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.CodigoDespa))
                throw new ArgumentException("El código de despacho del usuario es obligatorio.");

            var res = await _repository.GestionarActividadesAsync(dto.CodigoDespa, dto.ListaCodigosActividades);
            return ResultadoOperacionMapper.ToDto(res);
        }
    }
}