using Mantenimiento.Core.Application.DTOs.Daf;
using Mantenimiento.Core.Application.InterfaceServices;
using Mantenimiento.Core.Domain.RepositoryInterfaces;

namespace Mantenimiento.Core.Application.Services
{
    public class DafService : IDafService
    {
        private readonly IDafRepository _dafRepository;

        public DafService(IDafRepository dafRepository)
        {
            _dafRepository = dafRepository;
        }

        public async Task<IEnumerable<DafDto>> ObtenerAsync(int? id = null, int? idSubCapitulo = null, string? codigoDaf = null)
        {
            var lista = await _dafRepository.ObtenerAsync(id, idSubCapitulo, codigoDaf);

            return lista.Select(d => new DafDto
            {
                Id = d.Id,
                IdSubCapitulo = d.IdSubCapitulo,
                CodigoDaf = d.CodigoDaf,
                Daf = d.NombreDaf
            });
        }
    }
}