using Mantenimiento.Core.Application.DTOs.Impedimentos;
using Mantenimiento.Core.Domain.Pagination;

namespace Mantenimiento.Core.Application.InterfaceServices
{
    public interface IImpedimentoService
    {
        Task<PagedResult<ImpedimentoDto>> ObtenerPaginadoAsync(string? filtroTexto, int pagina = 1, int tamanoPagina = 10);
        Task<ImpedimentoDto?> ObtenerPorIdAsync(int id);
        Task<int> CrearAsync(CrearImpedimentoDto dto);
        Task ActualizarAsync(ActualizarImpedimentoDto dto);
        Task EliminarAsync(int id);
    }
}
