

using Mantenimiento.Core.Application.DTOs.TipoTramite;
using Mantenimiento.Core.Application.InterfaceServices;
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

            return entidades.Select(e => new TipoTramiteDto
            {
                Id = e.Id,
                Tipo = e.TipoTramite
            }).ToList();
        }

        public async Task<TipoTramiteDto?> ObtenerPorIdAsync(int id)
        {
            var entidad = await _tipoTramiteRepo.GetTipoTramiteByIdAsync(id);

            if (entidad == null)
                return null;

            return new TipoTramiteDto
            {
                Id = entidad.Id,
                Tipo = entidad.TipoTramite
            };
        }
    }
}
