using Mantenimiento.Core.Domain.Entities.ClasificacionContrato;

namespace Mantenimiento.Core.Domain.RepositoryInterfaces
{
    public interface IClasificacionContratoRepository
    {
        Task<List<ClasificacionContrato>> ObtenerAsync(int? codigoClasificacion, string? contratoTipo, string? tipoOperacion, string? contratoProcesoSeleccion);
        Task<int> CrearAsync(string? contratoTipo, string? tipoOperacion, string? contratoProcesoSeleccion);
        Task ActualizarAsync(int codigoClasificacion, string? contratoTipo, string? tipoOperacion, string? contratoProcesoSeleccion);
        Task EliminarAsync(int codigoClasificacion);
        Task<List<ProcesoSeleccionCatalogo>> ObtenerCatalogoProcesoSeleccionAsync();
        Task<List<ClasificacionOpcion>> ObtenerOpcionesAsync(string tipoOpcion);
    }
}