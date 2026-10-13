

using Mantenimiento.Core.Application.DTOs.TipoTramite;
using Mantenimiento.Core.Application.InterfaceServices;
using Mantenimiento.Core.Application.Mappings;
using Mantenimiento.Core.Domain.RepositoryInterfaces;

namespace Mantenimiento.Core.Application.Services
{
    public class TipoTramiteService : ITipoTramiteService   
    {
        private readonly ITipoTramiteRepository _tipoTramiteRepo;

        public TipoTramiteService(ITipoTramiteRepository tipoTramiteRepo)
        {
            _tipoTramiteRepo = tipoTramiteRepo;
        }

        public async Task<List<TipoTramiteDto>> ObtenerTiposTramiteAsync()
        {
            var entidades = await _tipoTramiteRepo.GetAllTipoTramiteAsync();

            return entidades.Select(TipoTramiteMapper.ToDto).ToList();
        }

        public async Task<TipoTramiteDto?> ObtenerPorIdAsync(int id)
        {
            var entidad = await _tipoTramiteRepo.GetTipoTramiteByIdAsync(id);

            if (entidad == null)
                return null;

            return TipoTramiteMapper.ToDto(entidad);
        }
    }
}
