using Mantenimiento.Core.Application.DTOs.Daf;
using Mantenimiento.Core.Application.InterfaceServices;
using Mantenimiento.Core.Application.Mappings;
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

        /// <summary>
        /// Obtiene de forma asíncrona una colección de DafDto filtrada opcionalmente por id, idSubCapitulo o codigoDaf.
        /// </summary>
        /// <param name="id">Identificador opcional del registro; null para no filtrar por id.</param>
        /// <param name="idSubCapitulo">Identificador opcional del subcapítulo; null para no filtrar por subcapítulo.</param>
        /// <param name="codigoDaf">Código opcional del DAF; null para no filtrar por código.</param>
        /// <returns>Tarea que representa la operación asíncrona. El resultado contiene una colección de DafDto según los filtros
        /// especificados.</returns>
        public async Task<IEnumerable<DafDto>> ObtenerAsync(int? id = null, int? idSubCapitulo = null, string? codigoDaf = null)
        {
            var lista = await _dafRepository.ObtenerAsync(id, idSubCapitulo, codigoDaf);

            return lista.Select(DafMapper.ToDto);
        }

        /// <summary>
        /// Obtiene de forma asíncrona una colección de DafDto filtrada por el código DAF.
        /// </summary>
        /// <remarks>Los parámetros IdSubCapitulo e id se aceptan pero no se utilizan en la implementación
        /// actual; la consulta se realiza únicamente por codigoDaf.</remarks>
        /// <param name="codigoDaf">Código del DAF usado para filtrar la colección.</param>
        /// <param name="IdSubCapitulo">Identificador opcional del subcapítulo para filtrar los resultados.</param>
        /// <param name="id">Identificador opcional adicional para filtrar los resultados.</param>
        /// <returns>Tarea que representa la operación asíncrona y devuelve una colección de DafDto filtrada por codigoDaf.</returns>
        public async Task<IEnumerable<DafDto>> ObtenerPorCodigoAsync(string codigoDaf, int? IdSubCapitulo = null, int? id = null)
        {
            var lista = await _dafRepository.ObtenerPorCodigoDafAsync(codigoDaf);
            return lista.Select(DafMapper.ToDto).ToList();
        }

        /// <summary>
        /// Obtiene un DAF por su identificador y lo devuelve como DafDto.
        /// </summary>
        /// <remarks>Los datos se obtienen del repositorio y se mapean a un DTO mediante
        /// DafMapper.</remarks>
        /// <param name="id">Identificador del DAF.</param>
        /// <param name="IdSubCapitulo">Identificador opcional del subcapítulo asociado.</param>
        /// <param name="codigoDaf">Código opcional del DAF.</param>
        /// <returns>Tarea que devuelve el DafDto correspondiente al identificador especificado.</returns>
        public async Task<DafDto> ObtenerPorIdAsync(int id, int? IdSubCapitulo = null, string? codigoDaf = null)
        {
            var daf = await _dafRepository.ObtenerPorIdAsync(id);
            return DafMapper.ToDto(daf);
        }

        /// <summary>
        /// Obtiene de forma asíncrona los DAF asociados al subcapítulo indicado.
        /// </summary>
        /// <remarks>Los datos se obtienen del repositorio y se proyectan a DafDto.</remarks>
        /// <param name="idSubCapitulo">Identificador del subcapítulo cuyos DAF se recuperan.</param>
        /// <param name="id">Identificador opcional del DAF para filtrar los resultados.</param>
        /// <param name="codigoDaf">Código opcional del DAF para filtrar los resultados.</param>
        /// <returns>Tarea que representa la operación asíncrona. El resultado contiene una colección de DafDto correspondientes
        /// al subcapítulo.</returns>
        public async Task<IEnumerable<DafDto>> ObtenerPorSubCapitulo(int idSubCapitulo, int? id = null, string? codigoDaf = null)
        {
            var lista = await _dafRepository.ObtenerPorSubCapituloAsync(idSubCapitulo);
            return lista.Select(DafMapper.ToDto).ToList();
        }

        /// <summary>
        /// Crea un nuevo DAF a partir de los datos proporcionados de forma asíncrona.
        /// </summary>
        /// <remarks>Valida el DTO, mapea a entidad y persiste la entidad mediante el
        /// repositorio.</remarks>
        /// <param name="dafDto">DTO con los datos necesarios para crear el DAF.</param>
        /// <returns>Tarea que representa la operación asíncrona y contiene el DafDto creado.</returns>
        public async Task<DafDto> CrearAsync(CrearDafDTO dafDto)
        {
            ValidarDaf(dafDto.CodigoDaf, dafDto.Daf);

            var entidad = DafMapper.ToEntity(dafDto);

            await _dafRepository.CrearAsync(entidad);
            return DafMapper.ToDto(entidad);
        }

        /// <summary>
        /// Actualiza de forma asíncrona un DAF existente en el repositorio y devuelve su representación como DTO.
        /// </summary>
        /// <remarks>Valida el DAF antes de actualizar; realiza el mapeo entre DTO y entidad y persiste
        /// los cambios. Puede lanzar una excepción si la validación falla.</remarks>
        /// <param name="dafDto">DTO que contiene los datos del DAF a actualizar (incluye CodigoDaf y Daf).</param>
        /// <returns>DafDto que representa el DAF actualizado.</returns>
        public async Task<DafDto> ActualizarAsync(ActualizarDafDTO dafDto)
        {
            ValidarDaf(dafDto.CodigoDaf, dafDto.Daf);

            var entidad = DafMapper.ToEntity(dafDto);

            await _dafRepository.ActualizarAsync(entidad);
            return DafMapper.ToDto(entidad);
        }

        /// <summary>
        /// Elimina de forma asíncrona el elemento identificado por el identificador especificado.
        /// </summary>
        /// <param name="id">Identificador del elemento a eliminar.</param>
        /// <returns>Tarea que representa la operación asíncrona de eliminación.</returns>
        public async Task EliminarAsync(int id)
        {
            await _dafRepository.EliminarAsync(id);
        }

        /// <summary>
        /// Valida que 'codigo' y 'daf' no sean nulos, vacíos ni contengan solo espacios, y que 'codigo' no supere 10
        /// caracteres.
        /// </summary>
        /// <param name="codigo">Código del daf; no puede ser nulo, vacío ni contener solo espacios; máximo 10 caracteres.</param>
        /// <param name="daf">Nombre del daf; no puede ser nulo, vacío ni contener solo espacios.</param>
        /// <exception cref="ArgumentException">Se lanza si 'codigo' o 'daf' son nulos, vacíos o contienen solo espacios, o si 'codigo' tiene más de 10
        /// caracteres.</exception>
        private static void ValidarDaf(string? codigo, string daf)
        {
            if (string.IsNullOrWhiteSpace(codigo))
                throw new ArgumentException("El código del daf es requerido.");

            if (string.IsNullOrWhiteSpace(daf))
                throw new ArgumentException("El nombre del daf es requerido.");

            if (codigo.ToString().Length > 10)
                throw new ArgumentException("El código del daf no puede exceder 10 dígitos.");
        }
    }
}