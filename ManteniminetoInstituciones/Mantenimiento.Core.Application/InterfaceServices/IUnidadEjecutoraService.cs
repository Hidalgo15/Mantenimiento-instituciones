using Mantenimiento.Core.Application.DTOs.UnidadEjecutora;

namespace Mantenimiento.Core.Application.InterfaceServices
{
    public interface IUnidadEjecutoraService
    {
        Task<IEnumerable<UnidadEjecutoraDto>> ObtenerAsync(int? id = null, string? codigoUnidadEjecutora = null, int? idDaf = null);
        Task<UnidadEjecutoraDto?> ObtenerPorIdAsync(int id);
        Task CrearAsync(CrearUnidadEjecutoraDto dto);
        Task ActualizarAsync(ActualizarUnidadEjecutoraDto dto);
        Task EliminarAsync(int id, string usuario);
    }
}
