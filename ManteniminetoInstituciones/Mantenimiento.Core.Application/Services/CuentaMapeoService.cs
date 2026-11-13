using FluentValidation;
using Mantenimiento.Core.Application.DTOs.Cuenta.DboCuentasDTO;
using Mantenimiento.Core.Application.InterfaceServices;
using Mantenimiento.Core.Application.Mappings;
using Mantenimiento.Core.Domain.Entities;
using Mantenimiento.Core.Domain.Pagination;
using Mantenimiento.Core.Domain.RepositoryInterfaces;

namespace Mantenimiento.Core.Application.Services
{
    public class CuentaMapeoService : ICuentaMapeoService
    {
        private readonly ICuentaMapeoRepository _repo;
        private readonly IValidator<CrearCuentaDto> _crearValidator;
        private readonly IValidator<ActualizarCuentaDto> _actualizarValidator;

        public CuentaMapeoService(
            ICuentaMapeoRepository repo,
            IValidator<CrearCuentaDto> crearValidator,
            IValidator<ActualizarCuentaDto> actualizarValidator)
        {
            _repo = repo;
            _crearValidator = crearValidator;
            _actualizarValidator = actualizarValidator;
        }

        /// <summary>
        /// Obtiene de forma asíncrona una lista de cuentas convertidas a CuentaDto.
        /// </summary>
        /// <remarks>Los datos se obtienen del repositorio y se mapean a DTOs antes de
        /// devolverlos.</remarks>
        /// <param name="tipoTramite">Filtro opcional por tipo de trámite; si es null no se aplica filtro.</param>
        /// <param name="busqueda">Texto de búsqueda opcional para filtrar cuentas; si es null o vacío no se aplica filtro.</param>
        /// <returns>Task que contiene la lista de CuentaDto que cumplen los filtros.</returns>
        public async Task<List<CuentaDto>> ObtenerCuentasAsync(int? tipoTramite = null, string? busqueda = null)
        {
            var lista = await _repo.ObtenerCuentasAsync(tipoTramite: tipoTramite, busqueda: busqueda);
            return lista.Select(CuentaMapeoMapper.ToDto).ToList();
        }

        public async Task<PagedResult<CuentaDto>> ObtenerPaginadoAsync(
            int? tipoTramite,
            string? busqueda,
            int pagina = 1,
            int tamanoPagina = 10)
        {
            var resultado = await _repo.ObtenerPaginadoAsync(tipoTramite, busqueda, pagina, tamanoPagina);
            return new PagedResult<CuentaDto>(
                resultado.Items.Select(CuentaMapeoMapper.ToDto).ToList(),
                resultado.PageNumber,
                resultado.PageSize,
                resultado.TotalCount);
        }

        /// <summary>
        /// Obtiene el catálogo base de cuentas de forma asíncrona.
        /// </summary>
        /// <remarks>Delegada al repositorio (_repo.ObtenerCuentasBaseCatalogAsync).</remarks>
        /// <returns>Una tarea que representa la operación asíncrona cuyo resultado es una lista de CuentaCt con el catálogo
        /// base.</returns>
        public async Task<List<CuentaCt>> ObtenerCatalogoBaseAsync()
        {
            return await _repo.ObtenerCuentasBaseCatalogAsync();
        }

        /// <summary>
        /// Obtiene de forma asíncrona la cuenta correspondiente al identificador especificado y la convierte a
        /// CuentaDto.
        /// </summary>
        /// <remarks>Consulta el repositorio y mapea la entidad persistente a un DTO.</remarks>
        /// <param name="idCuenta">Identificador de la cuenta a recuperar.</param>
        /// <returns>CuentaDto si se encuentra la entidad; en caso contrario, null.</returns>
        public async Task<CuentaDto?> ObtenerPorIdAsync(int idCuenta)
        {
            var entidad = await _repo.ObtenerPorIdAsync(idCuenta);
            return entidad != null ? CuentaMapeoMapper.ToDto(entidad) : null;
        }

        /// <summary>
        /// Crea una cuenta de forma asíncrona.
        /// </summary>
        /// <remarks>Recorta DescripcionCuenta y delega la creación en el repositorio.</remarks>
        /// <param name="dto">Datos necesarios para la creación: IdCuentaBase, DescripcionCuenta y TipoTramite.</param>
        /// <returns>Tarea que representa la operación asíncrona.</returns>
        /// <exception cref="ArgumentException">Se lanza cuando IdCuentaBase o TipoTramite tienen valores no válidos (<= 0).</exception>
        public async Task CrearCuentaAsync(CrearCuentaDto dto)
        {
            ArgumentNullException.ThrowIfNull(dto);
            await _crearValidator.ValidateAndThrowAsync(dto);

            await _repo.CrearCuentaAsync(dto.IdCuentaBase, dto.DescripcionCuenta?.Trim(), dto.TipoTramite);
        }

        /// <summary>
        /// Actualiza una cuenta existente de forma asíncrona.
        /// </summary>
        /// <remarks>La DescripcionCuenta se recorta (Trim) antes de persistir; la operación se delega al
        /// repositorio.</remarks>
        /// <param name="dto">Datos para actualizar la cuenta, incluyendo IdCuenta, DescripcionCuenta y TipoTramite.</param>
        /// <returns>Tarea que representa la operación asíncrona.</returns>
        /// <exception cref="ArgumentException">Se lanza ArgumentException si IdCuenta no es válido (<= 0), si DescripcionCuenta está nula o vacía, o si
        /// TipoTramite no es válido (<= 0).</exception>
        public async Task ActualizarCuentaAsync(ActualizarCuentaDto dto)
        {
            ArgumentNullException.ThrowIfNull(dto);
            await _actualizarValidator.ValidateAndThrowAsync(dto);

            await _repo.ActualizarCuentaAsync(dto.IdCuenta, dto.DescripcionCuenta.Trim(), dto.TipoTramite);
        }

        /// <summary>
        /// Elimina de forma asíncrona la cuenta con el identificador especificado.
        /// </summary>
        /// <remarks>Valida el identificador y delega la eliminación al repositorio subyacente.</remarks>
        /// <param name="idCuenta">Identificador de la cuenta a eliminar; debe ser mayor que cero.</param>
        /// <returns>Tarea que representa la operación asíncrona.</returns>
        /// <exception cref="ArgumentException">Se lanza si idCuenta es menor o igual que cero.</exception>
        public async Task EliminarCuentaAsync(int idCuenta)
        {
            if (idCuenta <= 0)
                throw new ArgumentException("ID de cuenta inválido para eliminar.");

            await _repo.EliminarCuentaAsync(idCuenta);
        }
    }
}