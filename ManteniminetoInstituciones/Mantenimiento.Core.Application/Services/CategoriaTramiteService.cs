using Mantenimiento.Core.Application.DTOs.UnidadEjecutora;
using Mantenimiento.Core.Application.InterfaceServices;
using Mantenimiento.Core.Application.Mappings;
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

        /// <summary>
        /// obtiene todas las categorias de tramite
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<CategoriaTramiteDto>> ObtenerTodasAsync()
        {
            var entidades = await _repo.ObtenerTodasAsync();
            return entidades.Select(CategoriaTramiteMapper.ToDto);
        }
    }
}