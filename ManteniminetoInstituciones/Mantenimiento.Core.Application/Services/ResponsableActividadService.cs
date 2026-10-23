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

        /// <summary>
        /// Obtiene la lista de catálogos de moldes.
        /// </summary>
        /// <returns></returns>
        public async Task<List<MoldeCatalogoDto>> ObtenerCatalogosMoldesAsync()
        {
            var data = await _repository.ObtenerCatalogosMoldesAsync();
            return data.Select(ResponsableActividadMapper.ToDto).ToList();
        }

        /// <summary>
        /// Obtiene la lista de subtipos de moldes por código de molde.
        /// </summary>
        /// <param name="codigoTre"></param>
        /// <returns></returns>
        public async Task<List<SubtipoMoldeDto>> ObtenerSubtiposPorMoldeAsync(int codigoTre)
        {
            var data = await _repository.ObtenerSubtiposPorMoldeAsync(codigoTre);
            return data.Select(ResponsableActividadMapper.ToDto).ToList();
        }

        /// <summary>
        /// Obtiene la lista de usuarios por códigos de subtipos.
        /// </summary>
        /// <param name="codigosSubtipos"></param>
        /// <returns></returns>
        public async Task<List<UsuarioSubtipoDto>> ObtenerUsuariosPorMoldeAsync(int codigoTre)
        {
            var data = await _repository.ObtenerUsuariosPorMoldeAsync(codigoTre);
            return data.Select(ResponsableActividadMapper.ToDto).ToList();
        }

        /// <summary>
        /// Obtiene la lista de actividades por códigos de subtipos.
        /// </summary>
        /// <param name="codigosSubtipos"></param>
        /// <returns></returns>
        public async Task<List<ActividadSubtipoDto>> ObtenerActividadesPorSubtiposAsync(string codigosSubtipos)
        {
            var data = await _repository.ObtenerActividadesPorSubtiposAsync(codigosSubtipos);
            return data.Select(ResponsableActividadMapper.ToDto).ToList();
        }

        /// <summary>
        /// Obtiene la lista de actividades asignadas a un usuario por su código de despacho.
        /// </summary>
        /// <param name="codigoDespa"></param>
        /// <returns></returns>
        public async Task<List<ResponsableActividadDto>> ObtenerActividadesUsuarioAsync(string codigoDespa)
        {
            var data = await _repository.ObtenerActividadesUsuarioAsync(codigoDespa);
            return data.Select(ResponsableActividadMapper.ToDto).ToList();
        }

        /// <summary>
        /// Gestiona las actividades asignadas a un usuario, agregando o eliminando según la lista de códigos de actividades proporcionada.
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<ResultadoOperacionDto> GestionarActividadesAsync(GestionarResponsableActividadDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.CodigoDespa))
                throw new ArgumentException("El código de despacho del usuario es obligatorio.");

            var res = await _repository.GestionarActividadesAsync(dto.CodigoDespa, dto.ListaCodigosActividades);
            return ResultadoOperacionMapper.ToDto(res);
        }
    }
}