using Mantenimiento.Core.Domain.Entities;

namespace Mantenimiento.Core.Domain.RepositoryInterfaces
{
    public interface ICategoriaTramiteRepository
    {
        Task<IEnumerable<CategoriaTramite>> ObtenerTodasAsync();
    }
}