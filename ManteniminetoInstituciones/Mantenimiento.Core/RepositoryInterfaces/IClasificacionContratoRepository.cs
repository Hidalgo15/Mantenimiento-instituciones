using Mantenimiento.Core.Domain.Entities.ClasificacionContrato;
using Mantenimiento.Core.Domain.Pagination;

namespace Mantenimiento.Core.Domain.RepositoryInterfaces
{
    public interface IClasificacionContratoRepository
    {
        Task<List<ClasificacionContrato>> ObtenerAsync(int? codigoClasificacion, string? contratoTipo, string? tipoOperacion, string? contratoProcesoSeleccion);
        Task<PagedResult<ClasificacionContrato>> ObtenerPaginadoAsync(int? codigoClasificacion, string? contratoTipo, string? tipoOperacion, string? contratoProcesoSeleccion, int pagina, int tamanoPagina);
        Task<int> CrearAsync(string? contratoTipo, string? tipoOperacion, string? contratoProcesoSeleccion);
        Task ActualizarAsync(int codigoClasificacion, string? contratoTipo, string? tipoOperacion, string? contratoProcesoSeleccion);
        Task EliminarAsync(int codigoClasificacion);
        Task<List<ProcesoSeleccionCatalogo>> ObtenerCatalogoProcesoSeleccionAsync();
        Task<List<ClasificacionOpcion>> ObtenerOpcionesAsync(string tipoOpcion);
    }
}