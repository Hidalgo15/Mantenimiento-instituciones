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

        /// <summary>
        /// Obtiene de forma asíncrona una lista de capítulos mapeados a CapituloDto, opcionalmente filtrada por código.
        /// </summary>
        /// <remarks>Los elementos se obtienen del repositorio y se mapean mediante
        /// CapituloMapper.</remarks>
        /// <param name="codigo">Código opcional para filtrar los capítulos; si es null se devuelven todos.</param>
        /// <returns>Una tarea que representa la operación asíncrona cuyo resultado es una lista de CapituloDto con los capítulos
        /// encontrados.</returns>
        public async Task<List<CapituloDto>> ObtenerCapitulosAsync(string? codigo = null)
        {
            var lista = await _repo.ObtenerCapitulosAsync(codigo);
            return lista.Select(CapituloMapper.ToDto).ToList();
        }

        /// <summary>
        /// Obtiene de forma asíncrona un CapituloDto a partir del identificador proporcionado.
        /// </summary>
        /// <param name="id">Identificador del capítulo solicitado.</param>
        /// <returns>Un CapituloDto correspondiente al identificador especificado, o null si no se encuentra.</returns>
        public async Task<CapituloDto> ObtenerPorIdAsync(int id)
        {
            var entidad = await _repo.GetByIdAsync(id);
            return CapituloMapper.ToDto(entidad);
        }

        /// <summary>
        /// Crea un capítulo a partir del DTO, valida sus datos e inserta la entidad mediante un procedimiento
        /// almacenado de forma asíncrona.
        /// </summary>
        /// <remarks>Valida el código y el nombre mediante ValidarCapitulo; puede lanzar una excepción si
        /// la validación falla. La entidad se obtiene con CapituloMapper y se persiste usando un procedimiento
        /// almacenado del repositorio.</remarks>
        /// <param name="dto">DTO con los datos del capítulo a crear.</param>
        /// <returns>Tarea que representa la operación asíncrona.</returns>
        public async Task CrearCapituloAsync(CrearCapituloDto dto)
        {
            ValidarCapitulo(dto.CodigoCapitulo, dto.NombreCapitulo);

            await _repo.InsertarConSpAsync(CapituloMapper.ToEntity(dto));
        }

        /// <summary>
        /// Actualiza un capítulo en el repositorio de forma asíncrona mediante un procedimiento almacenado.
        /// </summary>
        /// <remarks>Valida el código y el nombre del capítulo y mapea el DTO a una entidad antes de
        /// invocar el repositorio.</remarks>
        /// <param name="dto">Datos del capítulo a actualizar.</param>
        /// <returns>Tarea que se completa cuando la operación de actualización finaliza.</returns>
        public async Task ActualizarCapituloAsync(ActualizarCapituloDto dto)
        {
            ValidarCapitulo(dto.CodigoCapitulo, dto.NombreCapitulo);

            await _repo.ActualizarConSpAsync(CapituloMapper.ToEntity(dto));
        }

        /// <summary>
        /// Elimina de forma asíncrona el capítulo identificado por id.
        /// </summary>
        /// <param name="id">Identificador del capítulo a eliminar.</param>
        /// <returns>Una tarea que representa la operación asíncrona.</returns>
        /// <exception cref="InvalidOperationException">Se lanza si el capítulo tiene subcapítulos asociados; elimine primero sus subcapítulos.</exception>
        public async Task EliminarCapituloAsync(int id)
        {
            var subCapitulos = await _subCapituloRepo.ObtenerSubCapitulosPorCapituloAsync(id);
            if (subCapitulos.Count > 0)
                throw new InvalidOperationException("No se puede eliminar el capítulo porque tiene subcapítulos asociados. Elimine primero sus subcapítulos.");

            await _repo.EliminarConSpAsync(id);
        }

        /// <summary>
        /// Valida el código y el nombre de un capítulo y lanza ArgumentException si son inválidos.
        /// </summary>
        /// <param name="codigo">Código del capítulo. Requerido; máximo 10 caracteres.</param>
        /// <param name="nombre">Nombre del capítulo. Requerido.</param>
        /// <exception cref="ArgumentException">Se lanza cuando codigo o nombre son nulos, vacíos o contienen solo espacios, o cuando codigo excede 10
        /// caracteres.</exception>
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