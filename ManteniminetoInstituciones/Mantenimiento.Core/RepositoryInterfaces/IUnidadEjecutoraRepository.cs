using Mantenimiento.Core.Domain.Entities;

namespace Mantenimiento.Core.Domain.RepositoryInterfaces
{
    public interface IUnidadEjecutoraRepository
    {
        Task<IEnumerable<UnidadEjecutora>> ObtenerAsync(int? id = null, string? codigoUnidadEjecutora = null, int? idDaf = null);
        Task<int> CrearConSpAsync(UnidadEjecutora entidad, string usuario);
        Task ActualizarConSpAsync(UnidadEjecutora entidad, string usuario);
        Task EliminarConSpAsync(int id, bool borradoLogico, string usuario);
    }
}
