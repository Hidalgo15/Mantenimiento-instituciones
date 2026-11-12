using Mantenimiento.Core.Application.DTOs.ProveedoresInhabilitados;
using Mantenimiento.Core.Domain.Pagination;

namespace Mantenimiento.Core.Application.InterfaceServices
{
    public interface IProveedorInhabilitadoService
    {
        Task<List<ProveedorInhabilitadoDto>> ObtenerAsync(string? filtroTexto);
        Task<PagedResult<ProveedorInhabilitadoDto>> ObtenerPaginadoAsync(string? filtroTexto, int pagina = 1, int tamanoPagina = 10);
        Task<ProveedorInhabilitadoOpcionesDto> ObtenerOpcionesAsync();
        Task<ProveedorInhabilitadoDto?> ObtenerPorIdAsync(int id);
        Task<int> CrearAsync(CrearProveedorInhabilitadoDto dto);
        Task ActualizarAsync(ActualizarProveedorInhabilitadoDto dto);
        Task EliminarAsync(int id, string usuario);
    }
}
