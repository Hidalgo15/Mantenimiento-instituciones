using Mantenimiento.Core.Domain.Entities;

namespace Mantenimiento.Core.Domain.RepositoryInterfaces
{
    public interface IRequisitoRepository
    {
        Task<List<Requisito>> ObtenerRequisitosAsync(string? busqueda);
        Task<ResultadoOperacion> CrearAsync(string requisitoDescripcion);
        Task<ResultadoOperacion> ActualizarAsync(int requisitosID, string requisitoDescripcion);
        Task<ResultadoOperacion> EliminarAsync(int requisitosID);
    }
}
