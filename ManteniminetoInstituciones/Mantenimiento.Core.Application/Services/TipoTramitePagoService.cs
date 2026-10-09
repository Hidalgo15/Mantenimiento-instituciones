using Mantenimiento.Core.Application.DTOs.TipoTramite;
using Mantenimiento.Core.Application.InterfaceServices;
using Mantenimiento.Core.Domain.Entities;
using Mantenimiento.Core.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            return lista.Select(MapToDto).ToList();
        }

        public async Task<TipoTramiteDto?> ObtenerPorIdAsync(int id)
        {
            var entidad = await _repo.ObtenerPorIdAsync(id);
            return entidad != null ? MapToDto(entidad) : null;
        }

        private static TipoTramiteDto MapToDto(TipoTramitePago t) => new()
        {
            Id = t.Id,
            Tipo = t.Tipo != null ? t.Tipo.Trim() : string.Empty
        };
    }
}
