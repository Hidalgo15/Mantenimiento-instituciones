

using Mantenimiento.Core.Domain.Entities;

namespace Mantenimiento.Core.Domain.RepositoryInterfaces
{
    public interface IFondoEspecialesRepository
    {
        Task<List<FondosEspeciales>> GetAllFondosEspecialesAsync();
        Task<FondosEspeciales> GetFondoEspecialByIdAsync(int id);
        // Agregar int idInstitucion a la interfaz
        Task<FondosEspeciales> CreateFondosEspecialesAsync(FondosEspeciales fondoEspecial, int idInstitucion);
        Task<FondosEspeciales> UpdateFondosEspecialesAsync(FondosEspeciales fondoEspecial, int idInstitucion);
        Task DeleteFondosEspecialesAsync(int id);
    }
}
