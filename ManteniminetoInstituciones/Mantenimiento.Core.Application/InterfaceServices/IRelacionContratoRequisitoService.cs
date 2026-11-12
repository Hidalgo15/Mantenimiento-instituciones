using Mantenimiento.Core.Application.DTOs.RelacionContratoRequisito;
using Mantenimiento.Core.Application.DTOs.Resultado;
using Mantenimiento.Core.Domain.Pagination;

namespace Mantenimiento.Core.Application.InterfaceServices
{
    public interface IRelacionContratoRequisitoService
    {
        Task<List<RelacionContratoRequisitoDto>> ObtenerAsync(int? codigoClasificacion, int? codigoRequisito);
        Task<PagedResult<RelacionContratoRequisitoDto>> ObtenerPaginadoAsync(int? codigoClasificacion, int? codigoRequisito, int pagina = 1, int tamanoPagina = 10);
        Task<ResultadoOperacionDto> AsignarAsync(CrearRelacionContratoRequisitoDto dto);
        Task<ResultadoOperacionDto> ActualizarAsync(ActualizarRelacionContratoRequisitoDto dto);
        Task<ResultadoOperacionDto> EliminarAsync(int id);
        Task<List<CatalogoClasificacionContratoDto>> ObtenerCatalogoClasificacionAsync();
        Task<List<CatalogoRequisitoContratoDto>> ObtenerCatalogoRequisitosAsync();
    }
}