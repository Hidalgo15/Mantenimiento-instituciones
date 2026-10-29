using Mantenimiento.Core.Application.DTOs.ResponsableActividad;
using Mantenimiento.Core.Application.DTOs.Resultado;
using Mantenimiento.Core.Domain.Entities;

namespace Mantenimiento.Core.Application.InterfaceServices
{
    public interface IResponsableActividadService
    {
        Task<List<MoldeCatalogoDto>> ObtenerCatalogosMoldesAsync();
        Task<List<SubtipoMoldeDto>> ObtenerSubtiposPorMoldeAsync(int codigoTre);
        Task<List<UsuarioSubtipoDto>> ObtenerUsuariosPorMoldeAsync(int codigoTre);
        Task<List<ActividadSubtipoDto>> ObtenerActividadesPorSubtiposAsync(string codigosSubtipos);
        Task<List<ResponsableActividadDto>> ObtenerActividadesUsuarioAsync(string codigoDespa);
        Task<ResultadoOperacionDto> GestionarActividadesAsync(GestionarResponsableActividadDto dto);
        Task<List<UsuarioActividadAsignacionDto>> ObtenerUsuariosPorActividadAsync(int codigoActividad, int codigoTre, string? filtroCargo, string? filtroArea, string? busquedaGeneral);
        Task<ResultadoOperacionDto> GestionarActividadUsuariosAsync(GestionarActividadUsuariosDto dto);
        Task<List<FlujoMoldeDto>> ObtenerFlujosPorMoldeAsync(int codigoTre);
        Task<List<ActividadFlujoDto>> ObtenerActividadesPorFlujoAsync(int codigoTre, string nombreFlujo);
    }
}