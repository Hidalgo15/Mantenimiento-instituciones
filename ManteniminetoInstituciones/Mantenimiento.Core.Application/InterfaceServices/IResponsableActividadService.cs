using Mantenimiento.Core.Application.DTOs.ResponsableActividad;
using Mantenimiento.Core.Application.DTOs.Resultado;

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
    }
}