
using Mantenimiento.Core.Application.DTOs.Capitulo;
using Mantenimiento.Core.Application.DTOs.SubCapitulo;
using Mantenimiento.Core.Application.InterfaceServices;
using Mantenimiento.Core.Domain.Entities;
using Mantenimiento.Core.Domain.RepositoryInterfaces;

namespace Mantenimiento.Core.Application.Services
{
    public class SubCapituloService : ISubCapituloService
    {
        private readonly ISubCapituloRepository _repo;
        public SubCapituloService(ISubCapituloRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<SubCapituloDto>> ObtenerSubCapitulosAsync(string? codigo = null)
        {
            var lista = await _repo.ObtenerSubCapitulosAsync(codigo);
            return lista.Select(MapToDto).ToList();
        }

        public async Task<SubCapituloDto> ObtenerPorIdAsync(int id, int id_capitulo, string buscar)
        {
            var entidad = await _repo.GetByIdAsync(id, id_capitulo, buscar);
            return MapToDto(entidad);
        }

        public async Task CrearSubCapituloAsync(CrearSubCapituloDto dto)
        {
            ValidarSubCapitulo(dto.CodigoSubCapitulo, dto.subcapitulo);

            var entidad = new SubCapitulo
            {
                IdCapitulo = dto.IdCapitulo,
                CodigoSubCapitulo = dto.CodigoSubCapitulo?.ToString(),
                subcapitulo = dto.subcapitulo.Trim()
            };

            await _repo.InsertarConSpAsync(entidad);
        }

        public async Task ActualizarSubCapituloAsync(ActualizarSubCapituloDto dto)
        {
            ValidarSubCapitulo(dto.CodigoSubCapitulo, dto.subcapitulo);

            var entidad = new SubCapitulo
            {
                Id = dto.Id,
                IdCapitulo = dto.IdCapitulo,
                CodigoSubCapitulo = dto.CodigoSubCapitulo?.ToString(),
                subcapitulo = dto.subcapitulo.Trim()
            };

            await _repo.ActualizarConSpAsync(entidad);
        }

        public async Task EliminarSubCapituloAsync(int id)
        {
            await _repo.EliminarConSpAsync(id);
        }

        private static void ValidarSubCapitulo(string? codigo, string subcapitulo)
        {
            if (string.IsNullOrWhiteSpace(codigo))
                throw new ArgumentException("El código del subcapítulo es requerido.");

            if (string.IsNullOrWhiteSpace(subcapitulo))
                throw new ArgumentException("El nombre del subcapítulo es requerido.");

            if (codigo.ToString().Length > 10)
                throw new ArgumentException("El código del subcapítulo no puede exceder 10 dígitos.");
        }

        private static SubCapituloDto MapToDto(SubCapitulo c) => new()
        {
            Id = c.Id,
            IdCapitulo = c.IdCapitulo,
            CodigoSubCapitulo = c.CodigoSubCapitulo?.ToString(),
            subcapitulo = c.subcapitulo
        };


        
      public async Task<List<SubCapituloDto>> ObtenerSubCapitulosPorCapituloAsync(int capituloId)
      {
          var lista = await _repo.ObtenerSubCapitulosPorCapituloAsync(capituloId);
          return lista.Select(MapToDto).ToList();
      }

      public async Task<List<SubCapituloDto>> ObtenerSubCapitulosPorCodigoCapituloAsync(string codigoCapitulo)
      {
          var lista = await _repo.ObtenerSubCapitulosPorCodigoCapituloAsync(codigoCapitulo);
          return lista.Select(MapToDto).ToList();
      }


      public async Task<List<SubCapituloDto>> ObtenerSubCapitulosPorNombreCapituloAsync(string nombreCapitulo)
      {
          var lista = await _repo.ObtenerSubCapitulosPorNombreCapituloAsync(nombreCapitulo);
          return lista.Select(MapToDto).ToList();
      }


      public async Task<List<SubCapituloDto>> ObtenerSubCapitulosPorCodigoSubCapituloAsync(string codigoSubCapitulo)
      {
          var lista = await _repo.ObtenerSubCapitulosPorCodigoSubCapituloAsync(codigoSubCapitulo);
          return lista.Select(MapToDto).ToList();
      }

      
      public Task<List<SubCapituloDto>> ObtenerSubCapitulosPorNombreSubCapituloAsync(string nombreSubCapitulo)
      {
          throw new NotImplementedException();
      } 

    }
}
