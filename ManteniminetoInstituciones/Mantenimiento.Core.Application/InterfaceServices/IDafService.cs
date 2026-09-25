using Mantenimiento.Core.Application.DTOs.Daf;

namespace Mantenimiento.Core.Application.InterfaceServices
{
    public interface IDafService
    {
        Task<IEnumerable<DafDto>> ObtenerAsync(int? id = null, int? idSubCapitulo = null, string? codigoDaf = null);
    }
}
