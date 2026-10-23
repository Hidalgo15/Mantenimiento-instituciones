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
    }
}