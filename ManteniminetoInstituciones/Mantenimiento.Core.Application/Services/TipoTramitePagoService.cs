using Mantenimiento.Core.Application.DTOs.TipoTramite;
using Mantenimiento.Core.Application.InterfaceServices;
using Mantenimiento.Core.Application.Mappings;
using Mantenimiento.Core.Domain.RepositoryInterfaces;

namespace Mantenimiento.Core.Application.Services
{
    public class TipoTramitePagoService : ITipoTramitePagoService
    {
        private readonly ITipoTramitePagoRepository _repo;

        public TipoTramitePagoService(ITipoTramitePagoRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<TipoTramiteDto>> ObtenerTodosAsync()
        {
            var lista = await _repo.ObtenerTodosAsync();
            return lista.Select(TipoTramiteMapper.ToDto).ToList();
        }

        public async Task<TipoTramiteDto?> ObtenerPorIdAsync(int id)
        {
            var entidad = await _repo.ObtenerPorIdAsync(id);
            return entidad != null ? TipoTramiteMapper.ToDto(entidad) : null;
        }
    }
}
