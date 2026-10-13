
using Mantenimiento.Core.Application.DTOs.Capitulo;
using Mantenimiento.Core.Application.DTOs.SubCapitulo;
using Mantenimiento.Core.Application.InterfaceServices;
using Mantenimiento.Core.Application.Mappings;
using Mantenimiento.Core.Domain.RepositoryInterfaces;

namespace Mantenimiento.Core.Application.Services
{
    public class SubCapituloService : ISubCapituloService
    {
        private readonly ISubCapituloRepository _repo;
        private readonly IDafRepository _dafRepo; // <-- Añadimos el repositorio de DAF
        public SubCapituloService(ISubCapituloRepository repo, IDafRepository dafRepository )
        {
            _repo = repo;
            _dafRepo = dafRepository;
        }

        public async Task<List<SubCapituloDto>> ObtenerSubCapitulosAsync(string? codigo = null)
        {
            var lista = await _repo.ObtenerSubCapitulosAsync(codigo);
            return lista.Select(SubCapituloMapper.ToDto).ToList();
        }

        public async Task<SubCapituloDto> ObtenerPorIdAsync(int id, int id_capitulo, string buscar)
        {
            var entidad = await _repo.GetByIdAsync(id, id_capitulo, buscar);
            return SubCapituloMapper.ToDto(entidad);
        }

        public async Task CrearSubCapituloAsync(CrearSubCapituloDto dto)
        {
            ValidarSubCapitulo(dto.CodigoSubCapitulo, dto.subcapitulo);

            await _repo.InsertarConSpAsync(SubCapituloMapper.ToEntity(dto));
        }

        public async Task ActualizarSubCapituloAsync(ActualizarSubCapituloDto dto)
        {
            ValidarSubCapitulo(dto.CodigoSubCapitulo, dto.subcapitulo);

            await _repo.ActualizarConSpAsync(SubCapituloMapper.ToEntity(dto));
        }

        public async Task EliminarSubCapituloAsync(int id)
        {
            var dafsAsociadas = await _dafRepo.ObtenerPorSubCapituloAsync(id);
            if (dafsAsociadas != null && dafsAsociadas.Any())
            {
                throw new InvalidOperationException("No se puede eliminar este Sub-capítulo porque tiene unidades DAF asociadas.");
            }

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

        public async Task<List<SubCapituloDto>> ObtenerSubCapitulosPorCapituloAsync(int capituloId)
        {
            var lista = await _repo.ObtenerSubCapitulosPorCapituloAsync(capituloId);
            return lista.Select(SubCapituloMapper.ToDto).ToList();
        }

        public async Task<List<SubCapituloDto>> ObtenerSubCapitulosPorCodigoCapituloAsync(string codigoCapitulo)
        {
            var lista = await _repo.ObtenerSubCapitulosPorCodigoCapituloAsync(codigoCapitulo);
            return lista.Select(SubCapituloMapper.ToDto).ToList();
        }

        public async Task<List<SubCapituloDto>> ObtenerSubCapitulosPorNombreCapituloAsync(string nombreCapitulo)
        {
            var lista = await _repo.ObtenerSubCapitulosPorNombreCapituloAsync(nombreCapitulo);
            return lista.Select(SubCapituloMapper.ToDto).ToList();
        }

        public async Task<List<SubCapituloDto>> ObtenerSubCapitulosPorCodigoSubCapituloAsync(string codigoSubCapitulo)
        {
            var lista = await _repo.ObtenerSubCapitulosPorCodigoSubCapituloAsync(codigoSubCapitulo);
            return lista.Select(SubCapituloMapper.ToDto).ToList();
        }

        public async Task<List<SubCapituloDto>> ObtenerSubCapitulosPorNombreSubCapituloAsync(string nombreSubCapitulo)
        {
            var lista = await _repo.ObtenerSubCapitulosPorNombreSubCapituloAsync(nombreSubCapitulo);
            return lista.Select(SubCapituloMapper.ToDto).ToList();
        }
    }
}
