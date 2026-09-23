using Mantenimiento.Core.Application.DTOs.Capitulo;
using Mantenimiento.Core.Application.InterfaceServices;
using Mantenimiento.Core.Domain.Entities;
using Mantenimiento.Core.Domain.RepositoryInterfaces;

namespace Mantenimiento.Core.Application.Services
{
    public class CapituloService : ICapituloService
    {
        private readonly ICapituloRepository _repo;

        public CapituloService(ICapituloRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<CapituloDto>> ObtenerCapitulosAsync(string? buscar = null)
        {
            var lista = await _repo.ObtenerCapitulosAsync(buscar);
            return lista.Select(MapToDto).ToList();
        }

        public async Task<CapituloDto> ObtenerPorIdAsync(int id)
        {
            var entidad = await _repo.GetByIdAsync(id);
            return MapToDto(entidad);
        }

        public async Task CrearCapituloAsync(CrearCapituloDto dto)
        {
            ValidarCapitulo(dto.CodigoCapitulo, dto.NombreCapitulo);

            var entidad = new Capitulo
            {
                CodigoCapitulo = dto.CodigoCapitulo.Trim(),
                NombreCapitulo = dto.NombreCapitulo.Trim()
            };

            await _repo.InsertarConSpAsync(entidad);
        }

        public async Task ActualizarCapituloAsync(ActualizarCapituloDto dto)
        {
            ValidarCapitulo(dto.CodigoCapitulo, dto.NombreCapitulo);

            var entidad = new Capitulo
            {
                Id = dto.Id,
                CodigoCapitulo = dto.CodigoCapitulo.Trim(),
                NombreCapitulo = dto.NombreCapitulo.Trim()
            };

            await _repo.ActualizarConSpAsync(entidad);
        }

        public async Task EliminarCapituloAsync(int id)
        {
            await _repo.EliminarConSpAsync(id);
        }

        private static void ValidarCapitulo(string codigo, string nombre)
        {
            if (string.IsNullOrWhiteSpace(codigo))
                throw new ArgumentException("El código del capítulo es requerido.");

            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("El nombre del capítulo es requerido.");

            if (codigo.Length > 10)
                throw new ArgumentException("El código del capítulo no puede exceder 10 caracteres.");
        }

        private static CapituloDto MapToDto(Capitulo c) => new()
        {
            Id = c.Id,
            CodigoCapitulo = c.CodigoCapitulo,
            NombreCapitulo = c.NombreCapitulo
        };
    }
}