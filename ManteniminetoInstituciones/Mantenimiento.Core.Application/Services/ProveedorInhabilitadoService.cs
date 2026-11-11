using FluentValidation;
using Mantenimiento.Core.Application.DTOs.ProveedoresInhabilitados;
using Mantenimiento.Core.Application.InterfaceServices;
using Mantenimiento.Core.Application.Mappings;
using Mantenimiento.Core.Domain.Pagination;
using Mantenimiento.Core.Domain.RepositoryInterfaces;

namespace Mantenimiento.Core.Application.Services
{
    /// <summary>
    /// Coordina la validación y las operaciones de mantenimiento de proveedores inhabilitados.
    /// </summary>
    public class ProveedorInhabilitadoService : IProveedorInhabilitadoService
    {
        private readonly IProveedorInhabilitadoRepository _repository;
        private readonly IValidator<CrearProveedorInhabilitadoDto> _crearValidator;
        private readonly IValidator<ActualizarProveedorInhabilitadoDto> _actualizarValidator;

        /// <summary>
        /// Inicializa el servicio de proveedores inhabilitados y sus validadores.
        /// </summary>
        /// <param name="repository">Repositorio de persistencia de proveedores inhabilitados.</param>
        /// <param name="crearValidator">Validador de los datos de creación.</param>
        /// <param name="actualizarValidator">Validador de los datos de actualización.</param>
        public ProveedorInhabilitadoService(
            IProveedorInhabilitadoRepository repository,
            IValidator<CrearProveedorInhabilitadoDto> crearValidator,
            IValidator<ActualizarProveedorInhabilitadoDto> actualizarValidator)
        {
            _repository = repository;
            _crearValidator = crearValidator;
            _actualizarValidator = actualizarValidator;
        }

        /// <summary>
        /// Obtiene una página de proveedores inhabilitados y convierte sus entidades en DTO.
        /// </summary>
        /// <param name="filtroTexto">Texto opcional para filtrar los resultados.</param>
        /// <param name="pagina">Número de página solicitado.</param>
        /// <param name="tamanoPagina">Cantidad de resultados por página.</param>
        /// <returns>Resultado paginado de proveedores inhabilitados.</returns>
        public async Task<PagedResult<ProveedorInhabilitadoDto>> ObtenerPaginadoAsync(
            string? filtroTexto,
            int pagina = 1,
            int tamanoPagina = 10)
        {
            var resultado = await _repository.ObtenerPaginadoAsync(filtroTexto, pagina, tamanoPagina);
            return new PagedResult<ProveedorInhabilitadoDto>(
                resultado.Items.Select(ProveedorInhabilitadoMapper.ToDto).ToList(),
                resultado.PageNumber,
                resultado.PageSize,
                resultado.TotalCount);
        }

        /// <summary>
        /// Obtiene los valores de género y estado RPE existentes para los formularios.
        /// </summary>
        /// <returns>Opciones disponibles para los campos seleccionables.</returns>
        public async Task<ProveedorInhabilitadoOpcionesDto> ObtenerOpcionesAsync()
        {
            var opciones = await _repository.ObtenerOpcionesAsync();
            return new ProveedorInhabilitadoOpcionesDto
            {
                Generos = opciones.Generos,
                EstadosRpe = opciones.EstadosRpe
            };
        }

        /// <summary>
        /// Busca un proveedor inhabilitado por su identificador.
        /// </summary>
        /// <param name="id">Identificador del registro.</param>
        /// <returns>El DTO encontrado o null si no existe.</returns>
        public async Task<ProveedorInhabilitadoDto?> ObtenerPorIdAsync(int id)
        {
            ValidarId(id);
            var entidad = await _repository.ObtenerPorIdAsync(id);
            return entidad == null ? null : ProveedorInhabilitadoMapper.ToDto(entidad);
        }

        /// <summary>
        /// Valida y registra un proveedor inhabilitado.
        /// </summary>
        /// <param name="dto">Datos del proveedor que se creará.</param>
        /// <returns>Identificador asignado al nuevo registro.</returns>
        public async Task<int> CrearAsync(CrearProveedorInhabilitadoDto dto)
        {
            ArgumentNullException.ThrowIfNull(dto);
            await _crearValidator.ValidateAndThrowAsync(dto);
            return await _repository.CrearAsync(ProveedorInhabilitadoMapper.ToEntity(dto), dto.Usuario ?? string.Empty);
        }

        /// <summary>
        /// Valida y actualiza los datos de un proveedor inhabilitado.
        /// </summary>
        /// <param name="dto">Datos actualizados del proveedor.</param>
        public async Task ActualizarAsync(ActualizarProveedorInhabilitadoDto dto)
        {
            ArgumentNullException.ThrowIfNull(dto);
            await _actualizarValidator.ValidateAndThrowAsync(dto);
            await _repository.ActualizarAsync(ProveedorInhabilitadoMapper.ToEntity(dto), dto.Usuario ?? string.Empty);
        }

        /// <summary>
        /// Elimina físicamente un proveedor inhabilitado.
        /// </summary>
        /// <param name="id">Identificador del registro que se eliminará.</param>
        /// <param name="usuario">Usuario responsable de la operación.</param>
        public Task EliminarAsync(int id, string usuario)
        {
            ValidarId(id);
            return _repository.EliminarAsync(id, usuario);
        }

        /// <summary>
        /// Comprueba que el identificador del proveedor sea válido.
        /// </summary>
        /// <param name="id">Identificador que se validará.</param>
        /// <exception cref="ArgumentOutOfRangeException">El identificador no es positivo.</exception>
        private static void ValidarId(int id)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id), "El identificador del proveedor no es válido.");
        }
    }
}
