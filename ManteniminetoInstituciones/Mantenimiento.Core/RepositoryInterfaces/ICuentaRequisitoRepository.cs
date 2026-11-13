using Mantenimiento.Core.Domain.Entities;
using Mantenimiento.Core.Domain.Pagination;

namespace Mantenimiento.Core.Domain.RepositoryInterfaces
{
    public interface ICuentaRequisitoRepository
    {
        Task<List<CuentaRequisitoConsulta>> ObtenerRelacionAsync(double? idCuenta, int? idRequisito, bool? estado);
        Task<PagedResult<CuentaRequisitoConsulta>> ObtenerPaginadoAsync(double? idCuenta, int? idRequisito, bool? estado, int pagina, int tamanoPagina);
        Task<ResultadoOperacion> AsignarAsync(double idCuenta, int idRequisito, bool estado);
        Task<ResultadoOperacion> ActualizarAsync(int cuentaRequisitosID, double idCuenta, int idRequisito, bool estado);
        Task<ResultadoOperacion> EliminarAsync(int cuentaRequisitosID);
        Task<List<CuentaCatalogo>> ObtenerCatalogoCuentasAsync();
        Task<List<RequisitoCatalogo>> ObtenerCatalogoRequisitosAsync();
    }
}
