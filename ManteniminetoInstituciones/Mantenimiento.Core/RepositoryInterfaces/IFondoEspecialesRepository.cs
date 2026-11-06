

using Mantenimiento.Core.Domain.Entities;
using Mantenimiento.Core.Domain.Pagination;

namespace Mantenimiento.Core.Domain.RepositoryInterfaces
{
    public interface IFondoEspecialesRepository
    {
        Task<PagedResult<FondosEspeciales>> GetFondosEspecialesPaginadosAsync(
            string? estructura,
            string? tipoTramite,
            int pageNumber,
            int pageSize);
        Task<FondosEspeciales?> GetFondoEspecialByIdAsync(int id);
        // Agregar int idInstitucion a la interfaz
        Task<FondosEspeciales> CreateFondosEspecialesAsync(FondosEspeciales fondoEspecial, int idInstitucion);
        Task<FondosEspeciales> UpdateFondosEspecialesAsync(FondosEspeciales fondoEspecial, int idInstitucion);
        Task DeleteFondosEspecialesAsync(int id);
    }
}
