using FluentValidation;
using Mantenimiento.Core.Application.DTOs.Impedimentos;
using Mantenimiento.Core.Application.InterfaceServices;
using Mantenimiento.Core.Application.Mappings;
using Mantenimiento.Core.Domain.Pagination;
using Mantenimiento.Core.Domain.RepositoryInterfaces;

namespace Mantenimiento.Core.Application.Services
{
    /// <summary>
    /// Coordina la validación y las operaciones de mantenimiento de impedimentos.
    /// </summary>
    public class ImpedimentoService : IImpedimentoService
    {
        private readonly IImpedimentoRepository _repository;
        private readonly IValidator<CrearImpedimentoDto> _crearValidator;
        private readonly IValidator<ActualizarImpedimentoDto> _actualizarValidator;

        /// <summary>
        /// Inicializa el servicio de impedimentos y sus validadores.
        /// </summary>
        /// <param name="repository">Repositorio de persistencia de impedimentos.</param>
        /// <param name="crearValidator">Validador de los datos de creación.</param>
        /// <param name="actualizarValidator">Validador de los datos de actualización.</param>
        public ImpedimentoService(
            IImpedimentoRepository repository,
            IValidator<CrearImpedimentoDto> crearValidator,
            IValidator<ActualizarImpedimentoDto> actualizarValidator)
        {
            _repository = repository;
            _crearValidator = crearValidator;
            _actualizarValidator = actualizarValidator;
        }

        public async Task<List<ImpedimentoDto>> ObtenerAsync(string? filtroTexto)
        {
            var registros = await _repository.ObtenerAsync(filtroTexto);
            return registros.Select(ImpedimentoMapper.ToDto).ToList();
        }

        /// <summary>
        /// Obtiene una página de impedimentos y convierte las entidades en DTO.
        /// </summary>
        /// <param name="filtroTexto">Texto opcional para filtrar los resultados.</param>
        /// <param name="pagina">Número de página solicitado.</param>
        /// <param name="tamanoPagina">Cantidad de resultados por página.</param>
        /// <returns>Resultado paginado de impedimentos.</returns>
        public async Task<PagedResult<ImpedimentoDto>> ObtenerPaginadoAsync(
            string? filtroTexto,
            int pagina = 1,
            int tamanoPagina = 10)
        {
            var resultado = await _repository.ObtenerPaginadoAsync(filtroTexto, pagina, tamanoPagina);
            return new PagedResult<ImpedimentoDto>(
                resultado.Items.Select(ImpedimentoMapper.ToDto).ToList(),
                resultado.PageNumber,
                resultado.PageSize,
                resultado.TotalCount);
        }

        /// <summary>
        /// Busca un impedimento por su identificador.
        /// </summary>
        /// <param name="id">Identificador del impedimento.</param>
        /// <returns>El DTO encontrado o null si no existe.</returns>
        public async Task<ImpedimentoDto?> ObtenerPorIdAsync(int id)
        {
            ValidarId(id);
            var entidad = await _repository.ObtenerPorIdAsync(id);
            return entidad == null ? null : ImpedimentoMapper.ToDto(entidad);
        }

        /// <summary>
        /// Valida y registra un nuevo impedimento.
        /// </summary>
        /// <param name="dto">Datos del impedimento que se creará.</param>
        /// <returns>Identificador asignado al nuevo impedimento.</returns>
        public async Task<int> CrearAsync(CrearImpedimentoDto dto)
        {
            ArgumentNullException.ThrowIfNull(dto);
            await _crearValidator.ValidateAndThrowAsync(dto);
            return await _repository.CrearAsync(ImpedimentoMapper.ToEntity(dto));
        }

        /// <summary>
        /// Valida y actualiza un impedimento existente.
        /// </summary>
        /// <param name="dto">Datos actualizados del impedimento.</param>
        public async Task ActualizarAsync(ActualizarImpedimentoDto dto)
        {
            ArgumentNullException.ThrowIfNull(dto);
            await _actualizarValidator.ValidateAndThrowAsync(dto);
            await _repository.ActualizarAsync(ImpedimentoMapper.ToEntity(dto));
        }

        /// <summary>
        /// Deshabilita un impedimento mediante baja lógica.
        /// </summary>
        /// <param name="id">Identificador del impedimento a deshabilitar.</param>
        public Task EliminarAsync(int id)
        {
            ValidarId(id);
            return _repository.EliminarAsync(id);
        }

        /// <summary>
        /// Comprueba que el identificador de un impedimento sea válido.
        /// </summary>
        /// <param name="id">Identificador que se validará.</param>
        /// <exception cref="ArgumentOutOfRangeException">El identificador no es positivo.</exception>
        private static void ValidarId(int id)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id), "El identificador del impedimento no es válido.");
        }
    }
}
