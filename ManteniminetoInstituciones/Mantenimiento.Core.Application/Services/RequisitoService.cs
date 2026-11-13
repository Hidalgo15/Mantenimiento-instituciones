using FluentValidation;
using Mantenimiento.Core.Application.DTOs.Requisito;
using Mantenimiento.Core.Application.DTOs.Resultado;
using Mantenimiento.Core.Application.InterfaceServices;
using Mantenimiento.Core.Application.Mappings;
using Mantenimiento.Core.Domain.Pagination;
using Mantenimiento.Core.Domain.RepositoryInterfaces;

namespace Mantenimiento.Core.Application.Services
{
    public class RequisitoService : IRequisitoService
    {
        private readonly IRequisitoRepository _repository;
        private readonly IApplicationCache _cache;
        private readonly IValidator<CrearRequisitoDto> _crearValidator;
        private readonly IValidator<ActualizarRequisitoDto> _actualizarValidator;
        private const string CacheKeyCatalogoRequisitos = "cat_requisitos_cache_key";

        public RequisitoService(
            IRequisitoRepository repository,
            IApplicationCache cache,
            IValidator<CrearRequisitoDto> crearValidator,
            IValidator<ActualizarRequisitoDto> actualizarValidator)
        {
            _repository = repository;
            _cache = cache;
            _crearValidator = crearValidator;
            _actualizarValidator = actualizarValidator;
        }

        /// <summary>
        /// Obtiene de forma asíncrona una lista de RequisitoDto filtrada por la cadena de búsqueda opcional.
        /// </summary>
        /// <remarks>Los requisitos se obtienen del repositorio y se convierten a DTO mediante
        /// RequisitoMapper.</remarks>
        /// <param name="busqueda">Cadena de búsqueda opcional para filtrar los requisitos; null o vacía devuelve todos.</param>
        /// <returns>Una tarea que representa la operación asíncrona. El resultado es la lista de RequisitoDto que coincide con
        /// la búsqueda.</returns>
        public async Task<List<RequisitoDto>> ObtenerRequisitosAsync(string? busqueda)
        {
            var entidades = await _repository.ObtenerRequisitosAsync(busqueda);
            return entidades.Select(RequisitoMapper.ToDto).ToList();
        }

        public async Task<PagedResult<RequisitoDto>> ObtenerPaginadoAsync(string? busqueda, int pagina = 1, int tamanoPagina = 10)
        {
            var resultado = await _repository.ObtenerPaginadoAsync(busqueda, pagina, tamanoPagina);
            return new PagedResult<RequisitoDto>(
                resultado.Items.Select(RequisitoMapper.ToDto).ToList(),
                resultado.PageNumber,
                resultado.PageSize,
                resultado.TotalCount);
        }

        /// <summary>
        /// Crea un requisito de forma asíncrona y devuelve el resultado de la operación.
        /// </summary>
        /// <remarks>La descripción se recorta antes de persistir.</remarks>
        /// <param name="dto">Datos para crear el requisito; RequisitoDescripcion debe contener texto no vacío.</param>
        /// <returns>Un ResultadoOperacionDto que indica el resultado de la operación de creación.</returns>
        /// <exception cref="ArgumentException">Si RequisitoDescripcion es nula, vacía o contiene solo espacios en blanco.</exception>
        public async Task<ResultadoOperacionDto> CrearAsync(CrearRequisitoDto dto)
        {
            ArgumentNullException.ThrowIfNull(dto);
            await _crearValidator.ValidateAndThrowAsync(dto);

            var res = await _repository.CrearAsync(dto.RequisitoDescripcion.Trim());
            if (res.CodigoResultado == 1)
                _cache.Remove(CacheKeyCatalogoRequisitos);

            return ResultadoOperacionMapper.ToDto(res);
        }

        /// <summary>
        /// Actualiza de forma asíncrona la descripción de un requisito identificado por dto.RequisitosID.
        /// </summary>
        /// <remarks>La descripción se recorta y la actualización se realiza mediante el repositorio
        /// (_repository.ActualizarAsync).</remarks>
        /// <param name="dto">Objeto con los datos de actualización; debe contener RequisitosID > 0 y RequisitoDescripcion no vacío; la
        /// descripción se recorta antes de actualizar.</param>
        /// <returns>ResultadoOperacionDto que representa el resultado de la operación de actualización.</returns>
        /// <exception cref="ArgumentException">Se lanza si RequisitosID es menor o igual a cero o si RequisitoDescripcion es nulo, vacío o contiene solo
        /// espacios.</exception>
        public async Task<ResultadoOperacionDto> ActualizarAsync(ActualizarRequisitoDto dto)
        {
            ArgumentNullException.ThrowIfNull(dto);
            await _actualizarValidator.ValidateAndThrowAsync(dto);

            var res = await _repository.ActualizarAsync(dto.RequisitosID, dto.RequisitoDescripcion.Trim());
            if (res.CodigoResultado == 1)
                _cache.Remove(CacheKeyCatalogoRequisitos);

            return ResultadoOperacionMapper.ToDto(res);
        }

        /// <summary>
        /// Elimina de forma asíncrona el requisito identificado por su ID y devuelve el resultado de la operación.
        /// </summary>
        /// <remarks>La operación delega en el repositorio para realizar la eliminación y mapea el
        /// resultado a ResultadoOperacionDto.</remarks>
        /// <param name="requisitosID">Identificador del requisito a eliminar. Debe ser mayor que cero.</param>
        /// <returns>Resultado de la operación de eliminación convertido a DTO, indicando éxito o fallo y detalles de la
        /// operación.</returns>
        /// <exception cref="ArgumentException">Se lanza si requisitosID es menor o igual a cero.</exception>
        public async Task<ResultadoOperacionDto> EliminarAsync(int requisitosID)
        {
            if (requisitosID <= 0)
                throw new ArgumentException("El ID del requisito es inválido.");

            var res = await _repository.EliminarAsync(requisitosID);
            if (res.CodigoResultado == 1)
                _cache.Remove(CacheKeyCatalogoRequisitos);

            return ResultadoOperacionMapper.ToDto(res);
        }
    }
}