using Mantenimiento.Core.Application.DTOs.ClasificacionContrato;
using Mantenimiento.Core.Application.InterfaceServices;
using Mantenimiento.Core.Application.Mappings;
using Mantenimiento.Core.Domain.RepositoryInterfaces;

namespace Mantenimiento.Core.Application.Services
{
    public class ClasificacionContratoService : IClasificacionContratoService
    {
        private readonly IClasificacionContratoRepository _repository;

        public ClasificacionContratoService(IClasificacionContratoRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Obtiene de forma asíncrona una lista de ClasificacionContratoDto que coinciden con los filtros
        /// especificados.
        /// </summary>
        /// <remarks>Los datos se obtienen del repositorio y se transforman a DTO mediante
        /// ClasificacionContratoMapper.ToDto.</remarks>
        /// <param name="codigoClasificacion">Código de la clasificación para filtrar; si es null no se aplica filtro.</param>
        /// <param name="contratoTipo">Tipo de contrato para filtrar; si es null no se aplica filtro.</param>
        /// <param name="tipoOperacion">Tipo de operación para filtrar; si es null no se aplica filtro.</param>
        /// <param name="contratoProcesoSeleccion">Proceso de selección del contrato para filtrar; si es null no se aplica filtro.</param>
        /// <returns>Tarea que representa la operación asíncrona y devuelve una lista de ClasificacionContratoDto que cumplen los
        /// criterios de filtrado.</returns>
        public async Task<List<ClasificacionContratoDto>> ObtenerAsync(int? codigoClasificacion, string? contratoTipo, string? tipoOperacion, string? contratoProcesoSeleccion)
        {
            var entidades = await _repository.ObtenerAsync(codigoClasificacion, contratoTipo, tipoOperacion, contratoProcesoSeleccion);
            return entidades.Select(ClasificacionContratoMapper.ToDto).ToList();
        }

        /// <summary>
        /// Obtiene de forma asíncrona una clasificación de contrato por su código.
        /// </summary>
        /// <remarks>Realiza una consulta al repositorio y mapea la entidad resultante a
        /// ClasificacionContratoDto.</remarks>
        /// <param name="codigoClasificacion">Código de la clasificación de contrato a recuperar.</param>
        /// <returns>Tarea que contiene el DTO de la clasificación de contrato si existe; en caso contrario, null.</returns>
        public async Task<ClasificacionContratoDto?> ObtenerPorIdAsync(int codigoClasificacion)
        {
            var entidad = (await _repository.ObtenerAsync(codigoClasificacion, null, null, null)).FirstOrDefault();
            return entidad != null ? ClasificacionContratoMapper.ToDto(entidad) : null;
        }

        /// <summary>
        /// Crea una clasificación de contrato a partir del DTO proporcionado.
        /// </summary>
        /// <param name="dto">DTO que contiene los valores necesarios (dto.ContratoTipo, dto.TipoOperacion, dto.ContratoProcesoSeleccion).</param>
        /// <returns>Identificador entero de la clasificación creada.</returns>
        public async Task<int> CrearAsync(CrearClasificacionContratoDto dto)
        {
            return await _repository.CrearAsync(dto.ContratoTipo, dto.TipoOperacion, dto.ContratoProcesoSeleccion);
        }

        /// <summary>
        /// Actualiza una clasificación de contrato existente según los valores del DTO proporcionado.
        /// </summary>
        /// <param name="dto">DTO que contiene los valores actualizados (dto.CodigoClasificacion, dto.ContratoTipo, dto.TipoOperacion, dto.ContratoProcesoSeleccion).</param>
        /// <returns>Tarea que representa la operación asincrónica.</returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task ActualizarAsync(ActualizarClasificacionContratoDto dto)
        {
            if (dto.CodigoClasificacion <= 0)
                throw new ArgumentException("El código de clasificación es inválido.");

            await _repository.ActualizarAsync(dto.CodigoClasificacion, dto.ContratoTipo, dto.TipoOperacion, dto.ContratoProcesoSeleccion);
        }

        /// <summary>
        /// Elimina de forma asíncrona la clasificación identificada por el código especificado.
        /// </summary>
        /// <remarks>La operación se delega al repositorio subyacente.</remarks>
        /// <param name="codigoClasificacion">Código de la clasificación; debe ser mayor que cero.</param>
        /// <returns>Tarea que representa la operación de eliminación asíncrona.</returns>
        /// <exception cref="ArgumentException">Se lanza si el código de clasificación es menor o igual a cero.</exception>
        public async Task EliminarAsync(int codigoClasificacion)
        {
            if (codigoClasificacion <= 0)
                throw new ArgumentException("El código de clasificación es inválido.");

            await _repository.EliminarAsync(codigoClasificacion);
        }

        /// <summary>
        /// Obtiene el catálogo de procesos de selección y lo devuelve como una lista de ProcesoSeleccionCatalogoDto.
        /// </summary>
        /// <remarks>Los elementos se obtienen del repositorio y se proyectan a DTO mediante
        /// ClasificacionContratoMapper.ToDto.</remarks>
        /// <returns>Una tarea que representa la operación asíncrona. Contiene la lista de ProcesoSeleccionCatalogoDto
        /// resultante.</returns>
        public async Task<List<ProcesoSeleccionCatalogoDto>> ObtenerCatalogoProcesoSeleccionAsync()
        {
            var catalogo = await _repository.ObtenerCatalogoProcesoSeleccionAsync();
            return catalogo.Select(ClasificacionContratoMapper.ToDto).ToList();
        }

        /// <summary>
        /// Obtiene una lista de opciones de clasificación para el tipo de opción especificado.
        /// </summary>
        /// <param name="tipoOpcion">Tipo de opción que se usa para filtrar las opciones recuperadas.</param>
        /// <returns>Lista de ClasificacionOpcionDto mapeada desde el repositorio para el tipo especificado.</returns>
        public async Task<List<ClasificacionOpcionDto>> ObtenerOpcionesAsync(string tipoOpcion)
        {
            var opciones = await _repository.ObtenerOpcionesAsync(tipoOpcion);
            return opciones.Select(ClasificacionContratoMapper.ToDto).ToList();
        }
    }
}