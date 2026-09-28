using Mantenimiento.Core.Application.DTOs.Daf;
using Mantenimiento.Core.Application.DTOs.SubCapitulo;
using Mantenimiento.Core.Application.InterfaceServices;
using Mantenimiento.Core.Domain.Entities;
using Mantenimiento.Core.Domain.RepositoryInterfaces;

namespace Mantenimiento.Core.Application.Services
{
    public class DafService : IDafService
    {
        private readonly IDafRepository _dafRepository;

        public DafService(IDafRepository dafRepository)
        {
            _dafRepository = dafRepository;
        }

        public async Task<IEnumerable<DafDto>> ObtenerAsync(int? id = null, int? idSubCapitulo = null, string? codigoDaf = null)
        {
            var lista = await _dafRepository.ObtenerAsync(id, idSubCapitulo, codigoDaf);

            return lista.Select(d => new DafDto
            {
                Id = d.Id,
                IdSubCapitulo = d.IdSubCapitulo,
                CodigoDaf = d.CodigoDaf,
                Daf = d.NombreDaf
            });
        }

        public async Task<IEnumerable<DafDto>> ObtenerPorCodigoAsync(string codigoDaf, int? IdSubCapitulo = null, int? id = null)
        {
            var lista = await _dafRepository.ObtenerPorCodigoDafAsync(codigoDaf);
            return lista.Select(MapToDto).ToList();
        }

        public async Task<DafDto> ObtenerPorIdAsync(int id, int? IdSubCapitulo = null, string? codigoDaf = null)
        {
            var daf = await _dafRepository.ObtenerPorIdAsync(id);
            return MapToDto(daf);
        }

        public async Task<IEnumerable<DafDto>> ObtenerPorSubCapitulo(int idSubCapitulo, int? id = null, string? codigoDaf = null)
        {
            var lista = await _dafRepository.ObtenerPorSubCapituloAsync(idSubCapitulo);
            return lista.Select(MapToDto).ToList();
        }

        public async Task<DafDto> CrearAsync(CrearDafDTO dafDto)
        {
            ValidarDaf(dafDto.CodigoDaf, dafDto.Daf);

            var entidad = new Daf
            {
                IdSubCapitulo = dafDto.IdSubCapitulo,
                CodigoDaf = dafDto.CodigoDaf,
                NombreDaf = dafDto.Daf
            };

            await _dafRepository.CrearAsync(entidad);
            return MapToDto(entidad);
        }


        public async Task<DafDto> ActualizarAsync(ActualizarDafDTO dafDto)
        {
            ValidarDaf(dafDto.CodigoDaf, dafDto.Daf);

            var entidad = new Daf
            {
                Id = dafDto.Id,
                IdSubCapitulo = dafDto.IdSubCapitulo,
                CodigoDaf = dafDto.CodigoDaf,
                NombreDaf = dafDto.Daf
            };

            await _dafRepository.ActualizarAsync(entidad);
            return MapToDto(entidad);
        }

        public async Task EliminarAsync(int id)
        {
            await _dafRepository.EliminarAsync(id);
        }


        private static void ValidarDaf(string? codigo, string daf)
        {
            if (string.IsNullOrWhiteSpace(codigo))
                throw new ArgumentException("El código del daf es requerido.");

            if (string.IsNullOrWhiteSpace(daf))
                throw new ArgumentException("El nombre del daf es requerido.");

            if (codigo.ToString().Length > 10)
                throw new ArgumentException("El código del daf no puede exceder 10 dígitos.");
        }

        private static DafDto MapToDto(Daf d) => new()
        {
            Id = d.Id,
            IdSubCapitulo = d.IdSubCapitulo,
            CodigoDaf = d.CodigoDaf,
            Daf = d.NombreDaf
        };
    }
}