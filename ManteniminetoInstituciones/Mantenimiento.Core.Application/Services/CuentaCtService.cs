using FluentValidation;
using Mantenimiento.Core.Application.DTOs.Cuenta.CtCuentasDTO;
using Mantenimiento.Core.Application.InterfaceServices;
using Mantenimiento.Core.Application.Mappings;
using Mantenimiento.Core.Domain.Pagination;
using Mantenimiento.Core.Domain.RepositoryInterfaces;

namespace Mantenimiento.Core.Application.Services
{
    public class CuentaCtService : ICuentaCtService
    {
        private readonly ICuentaCtRepository _repo;
        private readonly IApplicationCache _cache;
        private readonly IValidator<CrearCuentaCtDto> _crearValidator;
        private readonly IValidator<ActualizarCuentaCtDto> _actualizarValidator;
        private const string CacheKeyCatalogoCuentas = "cat_cuentas_cache_key";

        public CuentaCtService(
            ICuentaCtRepository repo,
            IApplicationCache cache,
            IValidator<CrearCuentaCtDto> crearValidator,
            IValidator<ActualizarCuentaCtDto> actualizarValidator)
        {
            _repo = repo;
            _cache = cache;
            _crearValidator = crearValidator;
            _actualizarValidator = actualizarValidator;
        }

        /// <summary>
        /// Obtiene de forma asíncrona las cuentas y las devuelve como DTOs de tipo CuentaCtDto.
        /// </summary>
        /// <remarks>Los datos se obtienen del repositorio y se convierten a DTO mediante
        /// CuentaCtMapper.</remarks>
        /// <param name="busqueda">Filtro de búsqueda opcional que restringe los resultados; nulo devuelve todas las cuentas.</param>
        /// <returns>Una tarea que al completarse contiene la lista de cuentas como List<CuentaCtDto>.</returns>
        public async Task<List<CuentaCtDto>> ObtenerCuentasAsync(string? busqueda = null)
        {
            var lista = await _repo.ObtenerCuentasAsync(busqueda);
            return lista.Select(CuentaCtMapper.ToDto).ToList();
        }

        public async Task<PagedResult<CuentaCtDto>> ObtenerPaginadoAsync(string? busqueda, int pagina = 1, int tamanoPagina = 10)
        {
            var resultado = await _repo.ObtenerPaginadoAsync(busqueda, pagina, tamanoPagina);
            return new PagedResult<CuentaCtDto>(
                resultado.Items.Select(CuentaCtMapper.ToDto).ToList(),
                resultado.PageNumber,
                resultado.PageSize,
                resultado.TotalCount);
        }

        /// <summary>
        /// Obtiene de forma asíncrona la cuenta con el identificador especificado.
        /// </summary>
        /// <remarks>Consulta el repositorio para recuperar la entidad y la mapea a CuentaCtDto.</remarks>
        /// <param name="cuentaId">Identificador de la cuenta.</param>
        /// <returns>Tarea que contiene el CuentaCtDto de la cuenta si existe; en caso contrario, null.</returns>
        public async Task<CuentaCtDto?> ObtenerPorIdAsync(int cuentaId)
        {
            var entidad = await _repo.ObtenerPorIdAsync(cuentaId);
            return entidad != null ? CuentaCtMapper.ToDto(entidad) : null;
        }
        
        /// <summary>
        /// Crea una nueva cuenta de forma asíncrona.
        /// </summary>
        /// <param name="dto">DTO con los datos de la cuenta a crear.</param>
        /// <returns>Una tarea que representa la operación asíncrona.</returns>
        public async Task CrearCuentaAsync(CrearCuentaCtDto dto)
        {
            ArgumentNullException.ThrowIfNull(dto);
            await _crearValidator.ValidateAndThrowAsync(dto);

            await _repo.CrearCuentaAsync(
                dto.IdCuenta,
                dto.CodigoCuenta!.Trim(),
                dto.DescripcionCuenta!.Trim()
            );
            _cache.Remove(CacheKeyCatalogoCuentas);
        }

        /// <summary>
        /// Actualiza una cuenta existente tras validar CuentaID, CodigoCuenta y DescripcionCuenta, y delega la
        /// operación al repositorio de datos.
        /// </summary>
        /// <remarks>CodigoCuenta y DescripcionCuenta se recortan (Trim) antes de la actualización; la
        /// operación se realiza mediante el repositorio.</remarks>
        /// <param name="dto">Objeto con los datos de actualización (CuentaID, IdCuenta, CodigoCuenta, DescripcionCuenta).</param>
        /// <returns>Una tarea que representa la operación asincrónica.</returns>
        /// <exception cref="ArgumentException">Se lanza cuando CuentaID es menor o igual a cero, o cuando CodigoCuenta o DescripcionCuenta son nulos,
        /// vacíos o contienen solo espacios en blanco.</exception>
        public async Task ActualizarCuentaAsync(ActualizarCuentaCtDto dto)
        {
            ArgumentNullException.ThrowIfNull(dto);
            await _actualizarValidator.ValidateAndThrowAsync(dto);

            await _repo.ActualizarCuentaAsync(
                dto.CuentaID,
                dto.IdCuenta,
                dto.CodigoCuenta!.Trim(),
                dto.DescripcionCuenta!.Trim()
            );
            _cache.Remove(CacheKeyCatalogoCuentas);
        }

        /// <summary>
        /// Elimina de forma asíncrona la cuenta con el identificador especificado.
        /// </summary>
        /// <param name="cuentaId">Identificador de la cuenta a eliminar. Debe ser mayor que cero.</param>
        /// <returns>Tarea que representa la operación asíncrona.</returns>
        /// <exception cref="ArgumentException">Si cuentaId es menor o igual que cero.</exception>
        public async Task EliminarCuentaAsync(int cuentaId)
        {
            if (cuentaId <= 0)
                throw new ArgumentException("ID de cuenta inválido para eliminar.");

            await _repo.EliminarCuentaAsync(cuentaId);
            _cache.Remove(CacheKeyCatalogoCuentas);
        }
    }
}
