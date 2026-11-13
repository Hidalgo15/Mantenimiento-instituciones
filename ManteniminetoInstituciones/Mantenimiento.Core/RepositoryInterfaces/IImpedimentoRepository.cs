using Mantenimiento.Core.Domain.Entities;
using Mantenimiento.Core.Domain.Pagination;

namespace Mantenimiento.Core.Domain.RepositoryInterfaces
{
    public interface IImpedimentoRepository
    {
        Task<List<Impedimento>> ObtenerAsync(string? filtroTexto);
        Task<PagedResult<Impedimento>> ObtenerPaginadoAsync(string? filtroTexto, int pagina, int tamanoPagina);
        Task<Impedimento?> ObtenerPorIdAsync(int id);
        Task<int> CrearAsync(Impedimento impedimento);
        Task ActualizarAsync(Impedimento impedimento);
        Task EliminarAsync(int id);
    }
}
