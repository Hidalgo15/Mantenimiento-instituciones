using Mantenimiento.Core.Domain.Entities;
using Mantenimiento.Core.Domain.Pagination;

namespace Mantenimiento.Core.Domain.RepositoryInterfaces
{
    public interface IRequisitoRepository
    {
        Task<List<Requisito>> ObtenerRequisitosAsync(string? busqueda);
        Task<PagedResult<Requisito>> ObtenerPaginadoAsync(string? busqueda, int pagina, int tamanoPagina);
        Task<ResultadoOperacion> CrearAsync(string requisitoDescripcion);
        Task<ResultadoOperacion> ActualizarAsync(int requisitosID, string requisitoDescripcion);
        Task<ResultadoOperacion> EliminarAsync(int requisitosID);
    }
}
