

using Mantenimiento.Core.Application.DTOs.Fondos;

namespace Mantenimiento.Core.Application.InterfaceServices
{
    public interface IFondoEspecialesService
    {
        Task ActualizarFondoEspecialAsync(FondoDto dto);
        Task CrearFondoEspecialAsync(CrearFondoDto dto);
        Task EliminarFondoEspecialAsync(int id);
        Task<List<FondoDto>> ObtenerFondosEspecialesAsync();
        Task<FondoDto> ObtenerPorIdAsync(int id);
    }
}
