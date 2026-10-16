

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

        /// <summary>
        /// Obtiene de forma asíncrona la lista de tipos de trámite mapeados a DTOs.
        /// </summary>
        /// <remarks>Los elementos se recuperan desde el repositorio y se convierten a DTO mediante
        /// TipoTramiteMapper.</remarks>
        /// <returns>Una tarea que representa la operación asíncrona cuyo resultado es una lista de TipoTramiteDto.</returns>
        public async Task<List<TipoTramiteDto>> ObtenerTiposTramiteAsync()
        {
            var entidades = await _tipoTramiteRepo.GetAllTipoTramiteAsync();

            return entidades.Select(TipoTramiteMapper.ToDto).ToList();
        }

        /// <summary>
        /// Obtiene de forma asíncrona un TipoTramiteDto para el identificador especificado.
        /// </summary>
        /// <remarks>Recupera la entidad desde el repositorio y la convierte a DTO mediante
        /// TipoTramiteMapper. Devuelve null si no se encuentra.</remarks>
        /// <param name="id">Identificador del tipo de trámite.</param>
        /// <returns>Un TipoTramiteDto si existe; en caso contrario, null.</returns>
        public async Task<TipoTramiteDto?> ObtenerPorIdAsync(int id)
        {
            var entidad = await _tipoTramiteRepo.GetTipoTramiteByIdAsync(id);

            if (entidad == null)
                return null;

            return TipoTramiteMapper.ToDto(entidad);
        }
    }
}
