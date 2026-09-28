using Mantenimiento.Core.Application.DTOs.UnidadEjecutora;
using Mantenimiento.Core.Application.InterfaceServices;
using Mantenimiento.Core.Domain.Entities;
using Mantenimiento.Core.Domain.RepositoryInterfaces;

namespace Mantenimiento.Core.Application.Services
{
    public class UnidadEjecutoraService : IUnidadEjecutoraService
    {
        private readonly IUnidadEjecutoraRepository _repo;

        public UnidadEjecutoraService(IUnidadEjecutoraRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<UnidadEjecutoraDto>> ObtenerAsync(int? id = null, string? codigoUnidadEjecutora = null, int? idDaf = null)
        {
            var lista = await _repo.ObtenerAsync(id, codigoUnidadEjecutora, idDaf);
            return lista.Select(MapToDto);
        }

        public async Task<UnidadEjecutoraDto?> ObtenerPorIdAsync(int id)
        {
            var res = await _repo.ObtenerAsync(id: id);
            var entidad = res.FirstOrDefault();
            return entidad != null ? MapToDto(entidad) : null;
        }

        public async Task CrearAsync(CrearUnidadEjecutoraDto dto)
        {
            Validar(dto.IdDaf, dto.CodigoUnidadEjecutora, dto.NombreUnidadEjecutora, dto.Rnc);

            var entidad = new UnidadEjecutora
            {
                IdPadre = dto.IdPadre,
                IdDaf = dto.IdDaf,
                CodigoUnidadEjecutora = dto.CodigoUnidadEjecutora.Trim(),
                NombreUnidadEjecutora = dto.NombreUnidadEjecutora.Trim(),
                Rnc = dto.Rnc?.Trim(),
                Estado = dto.Estado,
                PortalCompra = dto.PortalCompra ? 1 : 0
            };

            await _repo.CrearConSpAsync(entidad, dto.Usuario);
        }

        public async Task ActualizarAsync(ActualizarUnidadEjecutoraDto dto)
        {
            Validar(dto.IdDaf, dto.CodigoUnidadEjecutora, dto.NombreUnidadEjecutora, dto.Rnc);

            var entidad = new UnidadEjecutora
            {
                Id = dto.Id,
                IdPadre = dto.IdPadre,
                IdDaf = dto.IdDaf,
                CodigoUnidadEjecutora = dto.CodigoUnidadEjecutora.Trim(),
                NombreUnidadEjecutora = dto.NombreUnidadEjecutora.Trim(),
                Rnc = dto.Rnc?.Trim(),
                Estado = dto.Estado,
                PortalCompra = dto.PortalCompra ? 1 : 0
            };

            await _repo.ActualizarConSpAsync(entidad, dto.Usuario);
        }

        public async Task EliminarAsync(int id, string usuario)
        {
            await _repo.EliminarConSpAsync(id, borradoLogico: true, usuario: usuario);
        }

        private static void Validar(int idDaf, string codigo, string nombre, string? rnc)
        {
            if (idDaf <= 0)
                throw new ArgumentException("Debe seleccionar una DAF válida.");

            if (string.IsNullOrWhiteSpace(codigo))
                throw new ArgumentException("El código de unidad ejecutora es obligatorio.");

            if (codigo.Trim().Length > 10)
                throw new ArgumentException("El código de unidad ejecutora no puede tener más de 10 caracteres.");

            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("El nombre de la unidad ejecutora es obligatorio.");

            if (nombre.Trim().Length > 250)
                throw new ArgumentException("El nombre de la unidad ejecutora no puede tener más de 250 caracteres.");

            if (!string.IsNullOrWhiteSpace(rnc) && rnc.Trim().Length > 20)
                throw new ArgumentException("El RNC no puede superar los 20 caracteres.");
        }

        private static UnidadEjecutoraDto MapToDto(UnidadEjecutora u) => new()
        {
            Id = u.Id,
            IdPadre = u.IdPadre,
            IdDaf = u.IdDaf,
            CodigoDaf = u.CodigoDaf ?? string.Empty,
            NombreDaf = u.NombreDaf ?? string.Empty,
            CodigoUnidadEjecutora = u.CodigoUnidadEjecutora ?? string.Empty,
            NombreUnidadEjecutora = u.NombreUnidadEjecutora ?? string.Empty,
            Rnc = u.Rnc,
            Estado = u.Estado,
            PortalCompra = u.PortalCompra
        };
    }
}