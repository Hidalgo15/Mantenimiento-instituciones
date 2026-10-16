using Mantenimiento.Core.Domain.Entities;
using Mantenimiento.Core.Domain.Entities.RelacionContratoRequisito;

namespace Mantenimiento.Core.Domain.RepositoryInterfaces
{
    public interface IRelacionContratoRequisitoRepository
    {
        Task<List<RelacionContratoRequisito>> ObtenerAsync(int? codigoClasificacion, int? codigoRequisito);
        Task<ResultadoOperacion> AsignarAsync(int codigoClasificacion, int codigoRequisito);
        Task<ResultadoOperacion> ActualizarAsync(int id, int codigoClasificacion, int codigoRequisito);
        Task<ResultadoOperacion> EliminarAsync(int id);
        Task<List<CatalogoClasificacionContrato>> ObtenerCatalogoClasificacionAsync();
        Task<List<CatalogoRequisitoContrato>> ObtenerCatalogoRequisitosAsync();
    }
}
