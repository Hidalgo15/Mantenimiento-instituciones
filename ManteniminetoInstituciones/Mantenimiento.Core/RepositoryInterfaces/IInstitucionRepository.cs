using Mantenimiento.Core.Domain.Entities;

namespace Mantenimiento.Core.Domain.RepositoryInterfaces
{
    public interface IInstitucionRepository
    {
        Task<List<Institucion>> ObtenerInstitucionesAsync();
    }
}
