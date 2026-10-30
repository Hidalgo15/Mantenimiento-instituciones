using Mantenimiento.Core.Domain.Entities;
using Mantenimiento.Core.Domain.Entities.ResponsableActividad;

namespace Mantenimiento.Core.Domain.RepositoryInterfaces
{
    public interface IResponsableActividadRepository
    {
        Task<List<MoldeCatalogo>> ObtenerCatalogosMoldesAsync();
        Task<List<SubtipoMolde>> ObtenerSubtiposPorMoldeAsync(int codigoTre);
        Task<List<UsuarioSubtipo>> ObtenerUsuariosPorMoldeAsync(int codigoTre);
        Task<List<ActividadSubtipo>> ObtenerActividadesPorSubtiposAsync(string codigosSubtipos);
        Task<List<ResponsableActividad>> ObtenerActividadesUsuarioAsync(string codigoDespa);
        Task<ResultadoOperacion> GestionarActividadesAsync(string codigoDespa, string? listaCodigosActividades);
        Task<List<UsuarioActividadAsignacion>> ObtenerUsuariosPorActividadAsync(int codigoActividad, int codigoTre, string? filtroCargo, string? filtroArea, string? busquedaGeneral);
        Task<ResultadoOperacion> GestionarActividadUsuariosAsync(int codigoActividad, string? listaCodigosDespa);
        Task<List<FlujoMolde>> ObtenerFlujosPorMoldeAsync(int codigoTre);
        Task<List<ActividadFlujo>> ObtenerActividadesPorFlujoAsync(int codigoTre, string nombreFlujo);
        Task<List<UsuarioActividadAsignacion>> ObtenerUsuariosPorFlujoAsync(int codigoTre, string nombreFlujo, string? busquedaGeneral);
    }
}