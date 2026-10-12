using Mantenimiento.Core.Domain.Entities;

namespace Mantenimiento.Core.Domain.RepositoryInterfaces
{
    public interface ICuentaRequisitoRepository
    {
        Task<List<CuentaRequisitoConsulta>> ObtenerRelacionAsync(double? idCuenta, int? idRequisito, bool? estado);
        Task<ResultadoOperacion> AsignarAsync(double idCuenta, int idRequisito, bool estado);
        Task<ResultadoOperacion> ActualizarAsync(int cuentaRequisitosID, double idCuenta, int idRequisito, bool estado);
        Task<ResultadoOperacion> EliminarAsync(int cuentaRequisitosID);
        Task<List<CuentaCatalogo>> ObtenerCatalogoCuentasAsync();
        Task<List<RequisitoCatalogo>> ObtenerCatalogoRequisitosAsync();
    }
}
