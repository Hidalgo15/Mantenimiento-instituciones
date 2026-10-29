using Mantenimiento.Core.Application.DTOs.ResponsableActividad;
using Mantenimiento.Core.Application.DTOs.Resultado;
using Mantenimiento.Core.Application.InterfaceServices;
using Mantenimiento.Core.Application.Mappings;
using Mantenimiento.Core.Domain.Entities;
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

        /// <summary>
        /// Obtiene la lista de usuarios evaluando si tienen asignada una actividad específica, aplicando filtros.
        /// </summary>
        public async Task<List<UsuarioActividadAsignacionDto>> ObtenerUsuariosPorActividadAsync(int codigoActividad, int codigoTre, string? filtroCargo, string? filtroArea, string? busquedaGeneral)
        {
            var data = await _repository.ObtenerUsuariosPorActividadAsync(codigoActividad, codigoTre, filtroCargo, filtroArea, busquedaGeneral);
            return data.Select(ResponsableActividadMapper.ToDto).ToList();
        }

        /// <summary>
        /// Gestiona la asignación de múltiples usuarios a una actividad específica (Nuevo Flujo Inverso).
        /// </summary>
        public async Task<ResultadoOperacionDto> GestionarActividadUsuariosAsync(GestionarActividadUsuariosDto dto)
        {
            if (dto.CodigoActividad <= 0)
                throw new ArgumentException("El código de actividad es obligatorio.");

            var res = await _repository.GestionarActividadUsuariosAsync(dto.CodigoActividad, dto.ListaCodigosDespa);
            return ResultadoOperacionMapper.ToDto(res);
        }

        public async Task<List<FlujoMoldeDto>> ObtenerFlujosPorMoldeAsync(int codigoTre)
        {
            var data = await _repository.ObtenerFlujosPorMoldeAsync(codigoTre);
            return data.Select(ResponsableActividadMapper.ToDto).ToList();
        }

        public async Task<List<ActividadFlujoDto>> ObtenerActividadesPorFlujoAsync(int codigoTre, string nombreFlujo)
        {
            var data = await _repository.ObtenerActividadesPorFlujoAsync(codigoTre, nombreFlujo);
            return data.Select(ResponsableActividadMapper.ToDto).ToList();
        }

        public async Task<List<UsuarioActividadAsignacionDto>> ObtenerUsuariosPorFlujoAsync(int codigoTre, string nombreFlujo, string? busquedaGeneral = null)
        {
            // 1. Obtenemos todas las actividades que pertenecen a este flujo
            var actividadesFlujo = await _repository.ObtenerActividadesPorFlujoAsync(codigoTre, nombreFlujo);
            var codigosActividadesFlujo = actividadesFlujo.Select(a => a.CodigoActividad).ToList();

            if (!codigosActividadesFlujo.Any()) return new List<UsuarioActividadAsignacionDto>();

            // 2. Tomamos la primera actividad solo para obtener la lista base de usuarios del molde (como tenías antes)
            var primerActividadId = codigosActividadesFlujo.First();
            var usuariosBase = await _repository.ObtenerUsuariosPorActividadAsync(primerActividadId, codigoTre, null, null, busquedaGeneral);

            var listaResultado = new List<UsuarioActividadAsignacionDto>();

            // 3. Para cada usuario, verificamos de manera real si tiene asignada CUALQUIERA de las actividades del flujo
            foreach (var usuario in usuariosBase)
            {
                var dto = ResponsableActividadMapper.ToDto(usuario);

                // Consultamos las actividades reales que tiene este usuario en BD
                var actividadesUsuario = await _repository.ObtenerActividadesUsuarioAsync(usuario.CodigoDespa);
                var codigosActividadesUsuario = actividadesUsuario.Select(a => a.CodigoActividad).ToHashSet();

                // Si el usuario tiene al menos una actividad de este flujo asignada, lo marcamos como asignado para el grid
                dto.EstaAsignado = codigosActividadesFlujo.Any(id => codigosActividadesUsuario.Contains(id));

                // Aplicamos filtro de búsqueda general si viene especificado
                if (string.IsNullOrWhiteSpace(busquedaGeneral) ||
                    dto.Nombre.Contains(busquedaGeneral, StringComparison.OrdinalIgnoreCase) ||
                    dto.CodigoDespa.Contains(busquedaGeneral, StringComparison.OrdinalIgnoreCase))
                {
                    listaResultado.Add(dto);
                }
            }

            return listaResultado;
        }

    }
}