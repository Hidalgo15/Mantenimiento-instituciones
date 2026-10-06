using Mantenimiento.Core.Application.DTOs.UnidadEjecutora;
using Mantenimiento.Core.Application.InterfaceServices;
using Mantenimiento.Core.Domain.RepositoryInterfaces;

namespace Mantenimiento.Core.Application.Services
{
    public class CategoriaTramiteService : ICategoriaTramiteService
    {
        private readonly ICategoriaTramiteRepository _repo;

        public CategoriaTramiteService(ICategoriaTramiteRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<CategoriaTramiteDto>> ObtenerTodasAsync()
        {
            var entidades = await _repo.ObtenerTodasAsync();

            // Mapeo de Entidad -> DTO en el Service
            return entidades.Select(e => new CategoriaTramiteDto
            {
                CodigoCategoria = e.CodigoCategoria,
                Descripcion = e.Descripcion
            });
        }
    }
}