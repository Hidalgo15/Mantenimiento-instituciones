using Mantenimiento.Core.Domain.Entities;

namespace Mantenimiento.Core.Domain.RepositoryInterfaces
{
    public interface IDafRepository
    {
        Task<IEnumerable<Daf>> ObtenerAsync(int? id = null, int? idSubCapitulo = null, string? codigoDaf = null);
        Task<Daf> ObtenerPorIdAsync(int id , int? IdSubCapitulo = null, string? codigoDaf = null);
        Task<IEnumerable<Daf>> ObtenerPorSubCapituloAsync(int? idSubCapitulo);
        Task<IEnumerable<Daf>> ObtenerPorCodigoDafAsync(string? codigoDaf);
        Task<Daf> CrearAsync(Daf daf);
        Task<Daf> ActualizarAsync(Daf daf);
        Task<bool> EliminarAsync(int id);

    }
}
