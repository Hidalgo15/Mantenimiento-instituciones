using Mantenimiento.Core.Application.DTOs.RequisitoContrato;
using Mantenimiento.Core.Domain.Pagination;

namespace Mantenimiento.Core.Application.InterfaceServices
{
    public interface IRequisitoContratoService
    {
        Task<List<RequisitoContratoDto>> ObtenerRequisitosAsync(string? busqueda = null);
        Task<PagedResult<RequisitoContratoDto>> ObtenerPaginadoAsync(string? busqueda = null, int pagina = 1, int tamanoPagina = 10);
        Task<RequisitoContratoDto> ObtenerPorIdAsync(int codigoRequisito);
        Task<int> CrearRequisitoContratoAsync(CrearRequisitoContratoDto dto);
        Task ActualizarRequisitoContratoAsync(ActualizarRequisitoContratoDto dto);
        Task EliminarRequisitoContratoAsync(int codigoRequisito);
    }
}
