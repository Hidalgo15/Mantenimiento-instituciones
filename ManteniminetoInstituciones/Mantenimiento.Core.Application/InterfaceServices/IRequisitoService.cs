using Mantenimiento.Core.Application.DTOs.Requisito;
using Mantenimiento.Core.Application.DTOs.Resultado;
using Mantenimiento.Core.Domain.Pagination;

namespace Mantenimiento.Core.Application.InterfaceServices
{
    public interface IRequisitoService
    {
        Task<List<RequisitoDto>> ObtenerRequisitosAsync(string? busqueda);
        Task<PagedResult<RequisitoDto>> ObtenerPaginadoAsync(string? busqueda, int pagina = 1, int tamanoPagina = 10);
        Task<ResultadoOperacionDto> CrearAsync(CrearRequisitoDto dto);
        Task<ResultadoOperacionDto> ActualizarAsync(ActualizarRequisitoDto dto);
        Task<ResultadoOperacionDto> EliminarAsync(int requisitosID);
    }
}
