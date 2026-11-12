using Mantenimiento.Core.Domain.Entities;
using Mantenimiento.Core.Domain.Pagination;

namespace Mantenimiento.Core.Domain.RepositoryInterfaces
{
    public interface IProveedorInhabilitadoRepository
    {
        Task<PagedResult<ProveedorInhabilitado>> ObtenerPaginadoAsync(string? filtroTexto, int pagina, int tamanoPagina);
        Task<(List<string> Generos, List<string> EstadosRpe)> ObtenerOpcionesAsync();
        Task<ProveedorInhabilitado?> ObtenerPorIdAsync(int id);
        Task<int> CrearAsync(ProveedorInhabilitado proveedor, string usuario);
        Task ActualizarAsync(ProveedorInhabilitado proveedor, string usuario);
        Task EliminarAsync(int id, string usuario);
    }
}
