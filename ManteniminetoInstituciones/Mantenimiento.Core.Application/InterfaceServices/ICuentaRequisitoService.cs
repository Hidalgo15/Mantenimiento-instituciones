using Mantenimiento.Core.Application.DTOs.CuentaRequisito;
using Mantenimiento.Core.Application.DTOs.Resultado;
using Mantenimiento.Core.Domain.Pagination;

namespace Mantenimiento.Core.Application.InterfaceServices
{
    public interface ICuentaRequisitoService
    {
        Task<List<CuentaRequisitoDto>> ObtenerRelacionAsync(double? idCuenta, int? idRequisito, bool? estado);
        Task<PagedResult<CuentaRequisitoDto>> ObtenerPaginadoAsync(double? idCuenta, int? idRequisito, bool? estado, int pagina = 1, int tamanoPagina = 10);
        Task<ResultadoOperacionDto> AsignarAsync(CrearCuentaRequisitoDto dto);
        Task<ResultadoOperacionDto> ActualizarAsync(ActualizarCuentaRequisitoDto dto);
        Task<ResultadoOperacionDto> EliminarAsync(int cuentaRequisitosID);
        Task<List<CuentaCatalogoDto>> ObtenerCatalogoCuentasAsync();
        Task<List<RequisitoCatalogoDto>> ObtenerCatalogoRequisitosAsync();
    }
}
