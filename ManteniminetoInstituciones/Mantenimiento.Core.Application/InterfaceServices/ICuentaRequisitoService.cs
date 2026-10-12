using Mantenimiento.Core.Application.DTOs.CuentaRequisito;

namespace Mantenimiento.Core.Application.InterfaceServices
{
    public interface ICuentaRequisitoService
    {
        Task<List<CuentaRequisitoDto>> ObtenerRelacionAsync(double? idCuenta, int? idRequisito, bool? estado);
        Task<ResultadoOperacionDto> AsignarAsync(CrearCuentaRequisitoDto dto);
        Task<ResultadoOperacionDto> ActualizarAsync(ActualizarCuentaRequisitoDto dto);
        Task<ResultadoOperacionDto> EliminarAsync(int cuentaRequisitosID);
        Task<List<CuentaCatalogoDto>> ObtenerCatalogoCuentasAsync();
        Task<List<RequisitoCatalogoDto>> ObtenerCatalogoRequisitosAsync();
    }
}
