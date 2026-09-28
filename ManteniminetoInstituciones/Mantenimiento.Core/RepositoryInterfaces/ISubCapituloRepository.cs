
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
    Task<List<SubCapitulo>> ObtenerSubCapitulosPorCapituloAsync(int capituloId);
    Task<List<SubCapitulo>> ObtenerSubCapitulosPorCodigoCapituloAsync(string codigoCapitulo);
    Task<List<SubCapitulo>> ObtenerSubCapitulosPorNombreCapituloAsync(string nombreCapitulo);
    Task<List<SubCapitulo>> ObtenerSubCapitulosPorCodigoSubCapituloAsync(string codigoSubCapitulo);
    Task<List<SubCapitulo>> ObtenerSubCapitulosPorNombreSubCapituloAsync(string nombreSubCapitulo);
    }
}
