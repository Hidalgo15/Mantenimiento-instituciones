

using Mantenimiento.Core.Application.DTOs.Fondos;
using Mantenimiento.Core.Domain.Pagination;

namespace Mantenimiento.Core.Application.InterfaceServices
{
    public interface IFondoEspecialesService
    {
        Task ActualizarFondoEspecialAsync(FondoDto dto);
        Task CrearFondoEspecialAsync(CrearFondoDto dto);
        Task EliminarFondoEspecialAsync(int id);
        Task<PagedResult<FondoDto>> ObtenerFondosEspecialesAsync(
            string? estructura = null,
            string? tipoTramite = null,
            int pageNumber = 1,
            int pageSize = 10);
        Task<FondoDto?> ObtenerPorIdAsync(int id);
    }
}
