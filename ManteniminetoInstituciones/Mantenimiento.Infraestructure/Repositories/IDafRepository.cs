using Mantenimiento.Core.Application.DTOs.Daf;

namespace Mantenimiento.Infraestructure.Persistence.Repositories
{
    public interface IDafRepository
    {
        Task<IEnumerable<DafDto>> ObtenerAsync(int? id = null, int? idSubCapitulo = null, string? codigoDaf = null);
    }
}
