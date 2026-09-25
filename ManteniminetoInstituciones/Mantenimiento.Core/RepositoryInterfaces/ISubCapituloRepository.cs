
using Mantenimiento.Core.Domain.Entities;

namespace Mantenimiento.Core.Domain.RepositoryInterfaces
{
    public interface ISubCapituloRepository
    {
    Task<SubCapitulo> GetByIdAsync(int id, int id_capitulo, string buscar);
    Task<List<SubCapitulo>> ObtenerSubCapitulosAsync(string? buscar);
    Task<int> InsertarConSpAsync(SubCapitulo subCapitulo);
    Task ActualizarConSpAsync(SubCapitulo subCapitulo);
    Task EliminarConSpAsync(int id);
    }
}
