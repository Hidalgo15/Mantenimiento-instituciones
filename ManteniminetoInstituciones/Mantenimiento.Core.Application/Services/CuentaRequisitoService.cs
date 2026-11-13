using FluentValidation;
using Mantenimiento.Core.Application.DTOs.CuentaRequisito;
using Mantenimiento.Core.Application.DTOs.Resultado;
using Mantenimiento.Core.Application.InterfaceServices;
using Mantenimiento.Core.Application.Mappings;
using Mantenimiento.Core.Domain.Pagination;
using Mantenimiento.Core.Domain.RepositoryInterfaces;

namespace Mantenimiento.Core.Application.Services
{
    public class CuentaRequisitoService : ICuentaRequisitoService
    {
        private readonly ICuentaRequisitoRepository _repository;
        private readonly IApplicationCache _cache;
        private readonly IValidator<CrearCuentaRequisitoDto> _crearValidator;
        private readonly IValidator<ActualizarCuentaRequisitoDto> _actualizarValidator;

        private const string CacheKeyCatalogoCuentas = "cat_cuentas_cache_key";
        private const string CacheKeyCatalogoRequisitos = "cat_requisitos_cache_key";
        private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(10);

        public CuentaRequisitoService(
            ICuentaRequisitoRepository repository,
            IApplicationCache cache,
            IValidator<CrearCuentaRequisitoDto> crearValidator,
            IValidator<ActualizarCuentaRequisitoDto> actualizarValidator)
        {
            _repository = repository;
            _cache = cache;
            _crearValidator = crearValidator;
            _actualizarValidator = actualizarValidator;
        }

        /// <summary>
        /// Obtiene de forma asíncrona las relaciones entre cuentas y requisitos y las devuelve como DTOs.
        /// </summary>
        /// <remarks>Consulta el repositorio mediante _repository.ObtenerRelacionAsync y convierte las
        /// entidades a DTOs usando CuentaRequisitoMapper.ToDto.</remarks>
        /// <param name="idCuenta">Identificador de la cuenta; si es null, no se aplica filtro por cuenta.</param>
        /// <param name="idRequisito">Identificador del requisito; si es null, no se aplica filtro por requisito.</param>
        /// <param name="estado">Estado de la relación; si es null, no se aplica filtro por estado.</param>
        /// <returns>Lista de CuentaRequisitoDto que cumplen los criterios de filtrado.</returns>
        public async Task<List<CuentaRequisitoDto>> ObtenerRelacionAsync(double? idCuenta, int? idRequisito, bool? estado)
        {
            var entidades = await _repository.ObtenerRelacionAsync(idCuenta, idRequisito, estado);
            return entidades.Select(CuentaRequisitoMapper.ToDto).ToList();
        }

        public async Task<PagedResult<CuentaRequisitoDto>> ObtenerPaginadoAsync(
            double? idCuenta,
            int? idRequisito,
            bool? estado,
            int pagina = 1,
            int tamanoPagina = 10)
        {
            var resultado = await _repository.ObtenerPaginadoAsync(idCuenta, idRequisito, estado, pagina, tamanoPagina);
            return new PagedResult<CuentaRequisitoDto>(
                resultado.Items.Select(CuentaRequisitoMapper.ToDto).ToList(),
                resultado.PageNumber,
                resultado.PageSize,
                resultado.TotalCount);
        }

        /// <summary>
        /// Asigna un requisito a una cuenta según los datos proporcionados y devuelve el resultado de la operación.
        /// </summary>
        /// <remarks>Valida los identificadores antes de delegar la asignación al repositorio de forma
        /// asíncrona.</remarks>
        /// <param name="dto">Objeto DTO que contiene IdCuenta, IdRequisito y Estado que indican qué requisito asignar y su estado.</param>
        /// <returns>ResultadoOperacionDto con el resultado de la asignación.</returns>
        public async Task<ResultadoOperacionDto> AsignarAsync(CrearCuentaRequisitoDto dto)
        {
            ArgumentNullException.ThrowIfNull(dto);
            await _crearValidator.ValidateAndThrowAsync(dto);
            var res = await _repository.AsignarAsync(dto.IdCuenta, dto.IdRequisito, dto.Estado);
            return ResultadoOperacionMapper.ToDto(res);
        }

        /// <summary>
        /// Actualiza de forma asíncrona la relación entre una cuenta y un requisito.
        /// </summary>
        /// <param name="dto">Datos de actualización que incluyen CuentaRequisitosID, IdCuenta, IdRequisito y Estado.</param>
        /// <returns>Un ResultadoOperacionDto que representa el resultado de la operación de actualización.</returns>
        /// <exception cref="FluentValidation.ValidationException">Se lanza cuando la relación o sus identificadores son inválidos.</exception>
        public async Task<ResultadoOperacionDto> ActualizarAsync(ActualizarCuentaRequisitoDto dto)
        {
            ArgumentNullException.ThrowIfNull(dto);
            await _actualizarValidator.ValidateAndThrowAsync(dto);
            var res = await _repository.ActualizarAsync(dto.CuentaRequisitosID, dto.IdCuenta, dto.IdRequisito, dto.Estado);
            return ResultadoOperacionMapper.ToDto(res);
        }

        /// <summary>
        /// Elimina de forma asíncrona la relación entre cuenta y requisitos identificada por su ID y devuelve el
        /// resultado de la operación.
        /// </summary>
        /// <remarks>La operación se delega al repositorio y el resultado se mapea a DTO mediante
        /// ResultadoOperacionMapper.</remarks>
        /// <param name="cuentaRequisitosID">ID de la relación cuenta-requisitos; debe ser mayor que cero.</param>
        /// <returns>Un ResultadoOperacionDto que indica el resultado de la operación de eliminación.</returns>
        /// <exception cref="ArgumentException">Si cuentaRequisitosID es menor o igual que cero.</exception>
        public async Task<ResultadoOperacionDto> EliminarAsync(int cuentaRequisitosID)
        {
            if (cuentaRequisitosID <= 0)
                throw new ArgumentException("El ID de la relación es inválido.");

            var res = await _repository.EliminarAsync(cuentaRequisitosID);
            return ResultadoOperacionMapper.ToDto(res);
        }

        /// <summary>
        /// Obtiene el catálogo de cuentas utilizando Caché en Memoria para optimizar rendimiento.
        /// </summary>
        public async Task<List<CuentaCatalogoDto>> ObtenerCatalogoCuentasAsync()
        {
            var catalogo = await _cache.GetOrCreateAsync(
                CacheKeyCatalogoCuentas,
                async () =>
                {
                    var entidades = await _repository.ObtenerCatalogoCuentasAsync();
                    return entidades.Select(CuentaRequisitoMapper.ToDto).ToArray();
                },
                CacheDuration);

            return catalogo.ToList();
        }

        /// <summary>
        /// Obtiene el catálogo de requisitos utilizando Caché en Memoria.
        /// </summary>
        public async Task<List<RequisitoCatalogoDto>> ObtenerCatalogoRequisitosAsync()
        {
            var catalogo = await _cache.GetOrCreateAsync(
                CacheKeyCatalogoRequisitos,
                async () =>
                {
                    var entidades = await _repository.ObtenerCatalogoRequisitosAsync();
                    return entidades.Select(CuentaRequisitoMapper.ToDto).ToArray();
                },
                CacheDuration);

            return catalogo.ToList();
        }
    }
}