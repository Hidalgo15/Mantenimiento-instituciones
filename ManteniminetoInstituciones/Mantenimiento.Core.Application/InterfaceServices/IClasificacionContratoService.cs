using Mantenimiento.Core.Application.DTOs.ClasificacionContrato;

namespace Mantenimiento.Core.Application.InterfaceServices
{
    public interface IClasificacionContratoService
    {
        Task<List<ClasificacionContratoDto>> ObtenerAsync(int? codigoClasificacion, string? contratoTipo, string? tipoOperacion, string? contratoProcesoSeleccion);
        Task<ClasificacionContratoDto?> ObtenerPorIdAsync(int codigoClasificacion);
        Task<int> CrearAsync(CrearClasificacionContratoDto dto);
        Task ActualizarAsync(ActualizarClasificacionContratoDto dto);
        Task EliminarAsync(int codigoClasificacion);
        Task<List<ProcesoSeleccionCatalogoDto>> ObtenerCatalogoProcesoSeleccionAsync();
        Task<List<ClasificacionOpcionDto>> ObtenerOpcionesAsync(string tipoOpcion);
    }
}