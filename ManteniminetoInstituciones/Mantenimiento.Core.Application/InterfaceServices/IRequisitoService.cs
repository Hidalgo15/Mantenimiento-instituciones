using Mantenimiento.Core.Application.DTOs.Requisito;
using Mantenimiento.Core.Application.DTOs.Resultado;

namespace Mantenimiento.Core.Application.InterfaceServices
{
    public interface IRequisitoService
    {
        Task<List<RequisitoDto>> ObtenerRequisitosAsync(string? busqueda);
        Task<ResultadoOperacionDto> CrearAsync(CrearRequisitoDto dto);
        Task<ResultadoOperacionDto> ActualizarAsync(ActualizarRequisitoDto dto);
        Task<ResultadoOperacionDto> EliminarAsync(int requisitosID);
    }
}
