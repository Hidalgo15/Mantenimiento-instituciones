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

        /// <summary>
        /// Obtiene todos los tipos de trámite de pago y los convierte a DTOs.
        /// </summary>
        /// <returns></returns>
        public async Task<List<TipoTramiteDto>> ObtenerTodosAsync()
        {
            var lista = await _repo.ObtenerTodosAsync();
            return lista.Select(TipoTramiteMapper.ToDto).ToList();
        }

        /// <summary>
        /// Obtiene de forma asíncrona un TipoTramite por su identificador.
        /// </summary>
        /// <remarks>Consulta el repositorio y convierte la entidad a DTO mediante
        /// TipoTramiteMapper.</remarks>
        /// <param name="id">Identificador del tipo de trámite.</param>
        /// <returns>Un TipoTramiteDto si existe; en caso contrario, null.</returns>
        public async Task<TipoTramiteDto?> ObtenerPorIdAsync(int id)
        {
            var entidad = await _repo.ObtenerPorIdAsync(id);
            return entidad != null ? TipoTramiteMapper.ToDto(entidad) : null;
        }
    }
}
