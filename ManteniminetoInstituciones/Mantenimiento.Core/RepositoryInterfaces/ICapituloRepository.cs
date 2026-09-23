using Mantenimiento.Core.Domain.Entities;

namespace Mantenimiento.Core.Domain.RepositoryInterfaces
{
    public interface ICapituloRepository
    {
        Task<Capitulo> GetByIdAsync(int id);
        Task<List<Capitulo>> ObtenerCapitulosAsync(string? buscar = null);
        Task<int> InsertarConSpAsync(Capitulo capitulo);
        Task ActualizarConSpAsync(Capitulo capitulo);
        Task EliminarConSpAsync(int id);
    }
}
