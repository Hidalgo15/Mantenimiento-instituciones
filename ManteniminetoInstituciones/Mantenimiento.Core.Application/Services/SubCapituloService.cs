
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
        private readonly IDafRepository _dafRepo;
        public SubCapituloService(ISubCapituloRepository repo, IDafRepository dafRepository )
        {
            _repo = repo;
            _dafRepo = dafRepository;
        }

        /// <summary>
        /// Obtiene una lista de subcapítulos, opcionalmente filtrada por código.
        /// </summary>
        /// <param name="codigo">Código del subcapítulo para filtrar la lista (opcional).</param>
        /// <returns>Lista de subcapítulos.</returns>
        public async Task<List<SubCapituloDto>> ObtenerSubCapitulosAsync(string? codigo = null)
        {
            var lista = await _repo.ObtenerSubCapitulosAsync(codigo);
            return lista.Select(SubCapituloMapper.ToDto).ToList();
        }

        /// <summary>
        /// Obtiene de forma asíncrona un SubCapituloDto por su identificador y el identificador del capítulo, aplicando
        /// un filtro de búsqueda opcional.
        /// </summary>
        /// <remarks>La entidad se obtiene del repositorio y se mapea a SubCapituloDto. Las excepciones
        /// del repositorio se propagan.</remarks>
        /// <param name="id">Identificador del subcapítulo.</param>
        /// <param name="id_capitulo">Identificador del capítulo al que pertenece el subcapítulo.</param>
        /// <param name="buscar">Filtro de búsqueda opcional; puede ser null o cadena vacía.</param>
        /// <returns>Tarea que representa la operación asíncrona. El resultado contiene el SubCapituloDto si se encuentra; en
        /// caso contrario, null.</returns>
        public async Task<SubCapituloDto> ObtenerPorIdAsync(int id, int id_capitulo, string buscar)
        {
            var entidad = await _repo.GetByIdAsync(id, id_capitulo, buscar);
            return SubCapituloMapper.ToDto(entidad);
        }

        /// <summary>
        /// Crea un subcapítulo a partir del DTO proporcionado, validando sus datos e insertando la entidad de forma
        /// asincrónica.
        /// </summary>
        /// <remarks>Valida el código y el contenido mediante ValidarSubCapitulo y persiste la entidad
        /// llamando al repositorio que ejecuta un procedimiento almacenado. Puede propagar excepciones de validación o
        /// de acceso a datos.</remarks>
        /// <param name="dto">DTO con los datos del subcapítulo, incluyendo CodigoSubCapitulo y subcapitulo.</param>
        /// <returns>Tarea que representa la operación asincrónica de creación.</returns>
        public async Task CrearSubCapituloAsync(CrearSubCapituloDto dto)
        {
            ValidarSubCapitulo(dto.CodigoSubCapitulo, dto.subcapitulo);

            await _repo.InsertarConSpAsync(SubCapituloMapper.ToEntity(dto));
        }

        /// <summary>
        /// Actualiza un subcapítulo validando el código y persistiendo los cambios mediante el repositorio, usando el
        /// mapper para convertir el DTO a entidad.
        /// </summary>
        /// <remarks>Valida el subcapítulo antes de persistir; puede lanzar excepciones por validación o
        /// por errores del repositorio.</remarks>
        /// <param name="dto">DTO que contiene el código del subcapítulo y los datos del subcapítulo a actualizar.</param>
        /// <returns>Tarea que representa la operación asincrónica.</returns>
        public async Task ActualizarSubCapituloAsync(ActualizarSubCapituloDto dto)
        {
            ValidarSubCapitulo(dto.CodigoSubCapitulo, dto.subcapitulo);

            await _repo.ActualizarConSpAsync(SubCapituloMapper.ToEntity(dto));
        }

        /// <summary>
        /// Elimina de forma asincrónica el subcapítulo identificado por su id.
        /// </summary>
        /// <remarks>Verifica la existencia de unidades DAF asociadas antes de solicitar la eliminación al
        /// repositorio; la eliminación se delega al repositorio y puede usar un procedimiento almacenado.</remarks>
        /// <param name="id">Identificador del subcapítulo a eliminar.</param>
        /// <returns>Tarea que representa la operación asincrónica.</returns>
        /// <exception cref="InvalidOperationException">Se lanza si el subcapítulo tiene unidades DAF asociadas.</exception>
        public async Task EliminarSubCapituloAsync(int id)
        {
            var dafsAsociadas = await _dafRepo.ObtenerPorSubCapituloAsync(id);
            if (dafsAsociadas != null && dafsAsociadas.Any())
            {
                throw new InvalidOperationException("No se puede eliminar este Sub-capítulo porque tiene unidades DAF asociadas.");
            }

            await _repo.EliminarConSpAsync(id);
        }

        /// <summary>
        /// Valida el código y el nombre de un subcapítulo y lanza ArgumentException si no cumplen las reglas de
        /// validación.
        /// </summary>
        /// <param name="codigo">Código del subcapítulo. Obligatorio; no puede ser nulo, vacío ni contener solo espacios; máximo 10
        /// caracteres.</param>
        /// <param name="subcapitulo">Nombre del subcapítulo. Obligatorio; no puede ser nulo, vacío ni contener solo espacios.</param>
        /// <exception cref="ArgumentException">Se lanza si el código es nulo, vacío o contiene solo espacios; si el nombre es nulo, vacío o contiene solo
        /// espacios; o si el código excede 10 caracteres.</exception>
        private static void ValidarSubCapitulo(string? codigo, string subcapitulo)
        {
            if (string.IsNullOrWhiteSpace(codigo))
                throw new ArgumentException("El código del subcapítulo es requerido.");

            if (string.IsNullOrWhiteSpace(subcapitulo))
                throw new ArgumentException("El nombre del subcapítulo es requerido.");

            if (codigo.ToString().Length > 10)
                throw new ArgumentException("El código del subcapítulo no puede exceder 10 dígitos.");
        }

        /// <summary>
        /// Obtiene de forma asíncrona los subcapítulos asociados al capítulo especificado.
        /// </summary>
        /// <param name="capituloId">Identificador del capítulo.</param>
        /// <returns>Tarea que devuelve una lista de SubCapituloDto con los subcapítulos del capítulo.</returns>
        public async Task<List<SubCapituloDto>> ObtenerSubCapitulosPorCapituloAsync(int capituloId)
        {
            var lista = await _repo.ObtenerSubCapitulosPorCapituloAsync(capituloId);
            return lista.Select(SubCapituloMapper.ToDto).ToList();
        }

        /// <summary>
        /// Obtiene de forma asíncrona los subcapítulos asociados al código de capítulo especificado y los mapea a
        /// SubCapituloDto.
        /// </summary>
        /// <param name="codigoCapitulo">Código del capítulo cuyos subcapítulos se recuperan.</param>
        /// <returns>Una tarea que representa la operación asíncrona cuyo resultado es una lista de SubCapituloDto
        /// correspondiente al capítulo; la lista estará vacía si no existen subcapítulos.</returns>
        public async Task<List<SubCapituloDto>> ObtenerSubCapitulosPorCodigoCapituloAsync(string codigoCapitulo)
        {
            var lista = await _repo.ObtenerSubCapitulosPorCodigoCapituloAsync(codigoCapitulo);
            return lista.Select(SubCapituloMapper.ToDto).ToList();
        }

        /// <summary>
        /// Obtiene de forma asíncrona los subcapítulos cuyo capítulo coincide con el nombre proporcionado.
        /// </summary>
        /// <remarks>Consulta el repositorio y mapea las entidades a SubCapituloDto mediante
        /// SubCapituloMapper.</remarks>
        /// <param name="nombreCapitulo">Nombre del capítulo por el que se filtran los subcapítulos.</param>
        /// <returns>Una tarea que representa la operación asíncrona. El resultado contiene una lista de SubCapituloDto
        /// correspondientes al capítulo especificado.</returns>
        public async Task<List<SubCapituloDto>> ObtenerSubCapitulosPorNombreCapituloAsync(string nombreCapitulo)
        {
            var lista = await _repo.ObtenerSubCapitulosPorNombreCapituloAsync(nombreCapitulo);
            return lista.Select(SubCapituloMapper.ToDto).ToList();
        }

        /// <summary>
        /// Obtiene los subcapítulos que coinciden con el código de subcapítulo especificado y los devuelve como DTOs.
        /// </summary>
        /// <remarks>Ejecuta la consulta de forma asíncrona en el repositorio y mapea las entidades
        /// resultantes a DTOs.</remarks>
        /// <param name="codigoSubCapitulo">Código del subcapítulo utilizado para filtrar los resultados.</param>
        /// <returns>Lista de SubCapituloDto que coinciden con el código proporcionado.</returns>
        public async Task<List<SubCapituloDto>> ObtenerSubCapitulosPorCodigoSubCapituloAsync(string codigoSubCapitulo)
        {
            var lista = await _repo.ObtenerSubCapitulosPorCodigoSubCapituloAsync(codigoSubCapitulo);
            return lista.Select(SubCapituloMapper.ToDto).ToList();
        }

        /// <summary>
        /// Obtiene de forma asincrónica los subcapítulos cuyo nombre coincide con el valor proporcionado y los
        /// convierte a DTO.
        /// </summary>
        /// <remarks>Los resultados se obtienen del repositorio y se mapean a DTO mediante
        /// SubCapituloMapper.ToDto.</remarks>
        /// <param name="nombreSubCapitulo">Nombre del subcapítulo por el que filtrar.</param>
        /// <returns>Tarea que contiene la lista de SubCapituloDto correspondiente a los subcapítulos encontrados.</returns>
        public async Task<List<SubCapituloDto>> ObtenerSubCapitulosPorNombreSubCapituloAsync(string nombreSubCapitulo)
        {
            var lista = await _repo.ObtenerSubCapitulosPorNombreSubCapituloAsync(nombreSubCapitulo);
            return lista.Select(SubCapituloMapper.ToDto).ToList();
        }
    }
}
