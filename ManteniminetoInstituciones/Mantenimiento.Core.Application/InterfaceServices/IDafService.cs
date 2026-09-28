using Mantenimiento.Core.Application.DTOs.Daf;

namespace Mantenimiento.Core.Application.InterfaceServices
{
    public interface IDafService
    {
        Task<IEnumerable<DafDto>> ObtenerAsync(int? id = null, int? idSubCapitulo = null, string? codigoDaf = null);
        Task <DafDto> ObtenerPorIdAsync(int id, int? IdSubCapitulo = null, string? codigoDaf = null);
        Task<IEnumerable<DafDto>> ObtenerPorCodigoAsync(string codigoDaf, int? IdSubCapitulo = null, int? id = null);
        Task<IEnumerable<DafDto>> ObtenerPorSubCapitulo (int idSubCapitulo, int? id = null, string? codigoDaf = null);
        Task<DafDto> CrearAsync(CrearDafDTO dafDto);
        Task<DafDto> ActualizarAsync(ActualizarDafDTO dafDto);
        Task EliminarAsync(int id);

    }
}
