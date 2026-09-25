using Mantenimiento.Core.Domain.Entities;

namespace Mantenimiento.Core.Domain.RepositoryInterfaces
{
    public interface IDafRepository
    {
        Task<IEnumerable<Daf>> ObtenerAsync(int? id = null, int? idSubCapitulo = null, string? codigoDaf = null);
    }
}
