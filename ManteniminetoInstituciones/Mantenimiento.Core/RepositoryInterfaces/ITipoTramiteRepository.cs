

using Mantenimiento.Core.Domain.Entities;

namespace Mantenimiento.Core.Domain.RepositoryInterfaces
{
    public interface ITipoTramiteRepository
    {
        Task <List<TramiteContrato>> GetAllTipoTramiteAsync();
        Task<TramiteContrato> GetTipoTramiteByIdAsync(int id);
    }
}
