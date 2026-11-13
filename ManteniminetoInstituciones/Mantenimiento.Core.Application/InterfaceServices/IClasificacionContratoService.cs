using Mantenimiento.Core.Application.DTOs.ClasificacionContrato;
using Mantenimiento.Core.Domain.Pagination;

namespace Mantenimiento.Core.Application.InterfaceServices
{
    public interface IClasificacionContratoService
    {
        Task<List<ClasificacionContratoDto>> ObtenerAsync(int? codigoClasificacion, string? contratoTipo, string? tipoOperacion, string? contratoProcesoSeleccion);
        Task<PagedResult<ClasificacionContratoDto>> ObtenerPaginadoAsync(int? codigoClasificacion, string? contratoTipo, string? tipoOperacion, string? contratoProcesoSeleccion, int pagina = 1, int tamanoPagina = 10);
        Task<ClasificacionContratoDto?> ObtenerPorIdAsync(int codigoClasificacion);
        Task<int> CrearAsync(CrearClasificacionContratoDto dto);
        Task ActualizarAsync(ActualizarClasificacionContratoDto dto);
        Task EliminarAsync(int codigoClasificacion);
        Task<List<ProcesoSeleccionCatalogoDto>> ObtenerCatalogoProcesoSeleccionAsync();
        Task<List<ClasificacionOpcionDto>> ObtenerOpcionesAsync(string tipoOpcion);
    }
}