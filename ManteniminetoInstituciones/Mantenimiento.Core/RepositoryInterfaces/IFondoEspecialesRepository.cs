

using Mantenimiento.Core.Domain.Entities;

namespace Mantenimiento.Core.Domain.RepositoryInterfaces
{
    public interface IFondoEspecialesRepository
    {
        Task<List<FondosEspeciales>> GetAllFondosEspecialesAsync();
        Task<FondosEspeciales> GetFondoEspecialByIdAsync(int id);
        Task <FondosEspeciales> UpdateFondosEspecialesAsync(FondosEspeciales fondoEspecial);
        Task <FondosEspeciales> CreateFondosEspecialesAsync(FondosEspeciales fondoEspecial);
        Task DeleteFondosEspecialesAsync(int id);
    }
}
