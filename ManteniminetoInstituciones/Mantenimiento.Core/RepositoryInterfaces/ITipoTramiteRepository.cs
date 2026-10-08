

using Mantenimiento.Core.Domain.Entities;

namespace Mantenimiento.Core.Domain.RepositoryInterfaces
{
    public interface ITipoTramiteRepository
    {
        Task <List<TipoTramiteContrato>> GetAllTipoTramiteAsync();
        Task<TipoTramiteContrato> GetTipoTramiteByIdAsync(int id);
    }
}
