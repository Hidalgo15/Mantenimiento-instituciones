using Mantenimiento.Core.Application.DTOs.Capitulo;
using Mantenimiento.Core.Application.InterfaceServices;
using Mantenimiento.Core.Application.Mappings;
using Mantenimiento.Core.Domain.RepositoryInterfaces;

namespace Mantenimiento.Core.Application.Services
{
    public class CapituloService : ICapituloService
    {
        private readonly ICapituloRepository _repo;
        private readonly ISubCapituloRepository _subCapituloRepo;

        public CapituloService(ICapituloRepository repo, ISubCapituloRepository subCapituloRepo)
        {
            _repo = repo;
            _subCapituloRepo = subCapituloRepo;
        }

        public async Task<List<CapituloDto>> ObtenerCapitulosAsync(string? codigo = null)
        {
            var lista = await _repo.ObtenerCapitulosAsync(codigo);
            return lista.Select(CapituloMapper.ToDto).ToList();
        }

        public async Task<CapituloDto> ObtenerPorIdAsync(int id)
        {
            var entidad = await _repo.GetByIdAsync(id);
            return CapituloMapper.ToDto(entidad);
        }

        public async Task CrearCapituloAsync(CrearCapituloDto dto)
        {
            ValidarCapitulo(dto.CodigoCapitulo, dto.NombreCapitulo);

            await _repo.InsertarConSpAsync(CapituloMapper.ToEntity(dto));
        }

        public async Task ActualizarCapituloAsync(ActualizarCapituloDto dto)
        {
            ValidarCapitulo(dto.CodigoCapitulo, dto.NombreCapitulo);

            await _repo.ActualizarConSpAsync(CapituloMapper.ToEntity(dto));
        }

        public async Task EliminarCapituloAsync(int id)
        {
            var subCapitulos = await _subCapituloRepo.ObtenerSubCapitulosPorCapituloAsync(id);
            if (subCapitulos.Count > 0)
                throw new InvalidOperationException("No se puede eliminar el capítulo porque tiene subcapítulos asociados. Elimine primero sus subcapítulos.");

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
    }
}