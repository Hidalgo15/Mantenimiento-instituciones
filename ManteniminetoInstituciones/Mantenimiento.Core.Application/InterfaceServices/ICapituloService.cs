using Mantenimiento.Core.Application.DTOs.Capitulo;

namespace Mantenimiento.Core.Application.InterfaceServices
{
    public interface ICapituloService
    {
        Task<List<CapituloDto>> ObtenerCapitulosAsync(string? codigo = null);
        Task<CapituloDto> ObtenerPorIdAsync(int id);
        Task CrearCapituloAsync(CrearCapituloDto dto);
        Task ActualizarCapituloAsync(ActualizarCapituloDto dto);
        Task EliminarCapituloAsync(int id);
    }
}
