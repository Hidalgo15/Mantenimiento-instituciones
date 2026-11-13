using FluentValidation;
using Mantenimiento.Core.Application.DTOs.Fondos;
using Mantenimiento.Core.Application.InterfaceServices;
using Mantenimiento.Core.Application.Mappings;
using Mantenimiento.Core.Domain.Pagination;
using Mantenimiento.Core.Domain.RepositoryInterfaces;


namespace Mantenimiento.Core.Application.Services
{
    public class FondoEspecialesService : IFondoEspecialesService
    {
        private readonly IFondoEspecialesRepository _fondoRepo;
        private readonly ITipoTramiteService _tipoTramiteService;
        private readonly IValidator<CrearFondoDto> _crearValidator;
        private readonly IValidator<FondoDto> _actualizarValidator;

        public FondoEspecialesService(
            IFondoEspecialesRepository fondoRepo,
            ITipoTramiteService tipoTramiteService,
            IValidator<CrearFondoDto> crearValidator,
            IValidator<FondoDto> actualizarValidator)
        {
            _fondoRepo = fondoRepo;
            _tipoTramiteService = tipoTramiteService;
            _crearValidator = crearValidator;
            _actualizarValidator = actualizarValidator;
        }

        /// <summary>
        /// Obtiene una lista de fondos especiales filtrados por estructura y tipo de trámite.
        /// </summary>
        /// <param name="estructura"></param>
        /// <param name="tipoTramite"></param>
        /// <returns></returns>
        public async Task<PagedResult<FondoDto>> ObtenerFondosEspecialesAsync(
            string? estructura = null,
            string? tipoTramite = null,
            int pageNumber = 1,
            int pageSize = 10)
        {
            var pagina = await _fondoRepo.GetFondosEspecialesPaginadosAsync(
                null,
                estructura,
                tipoTramite,
                pageNumber,
                pageSize);

            return new PagedResult<FondoDto>(
                pagina.Items.Select(FondoEspecialesMapper.ToDto).ToList(),
                pagina.PageNumber,
                pagina.PageSize,
                pagina.TotalCount);
        }

        public async Task<List<FondoDto>> ObtenerFondosEspecialesSinPaginacionAsync(
            string? estructura = null,
            string? tipoTramite = null)
        {
            var fondos = await _fondoRepo.GetFondosEspecialesAsync(null, estructura, tipoTramite);
            return fondos.Select(FondoEspecialesMapper.ToDto).ToList();
        }

        /// <summary>
        /// Obtiene de forma asíncrona un FondoDto por su identificador.
        /// </summary>
        /// <remarks>Realiza una llamada asíncrona al repositorio y mapea la entidad a un DTO mediante
        /// FondoEspecialesMapper.</remarks>
        /// <param name="id">Identificador del fondo especial.</param>
        /// <returns>Un FondoDto que representa el fondo encontrado, o null si no existe.</returns>
        public async Task<FondoDto?> ObtenerPorIdAsync(int id)
        {
            var entidad = await _fondoRepo.GetFondoEspecialByIdAsync(id);
            return entidad != null ? FondoEspecialesMapper.ToDto(entidad) : null;
        }

        /// <summary>
        /// Crea de forma asíncrona un registro de fondos especiales a partir del DTO proporcionado.
        /// </summary>
        /// <remarks>Valida el objeto Fondo, obtiene el nombre del tipo de trámite mediante el servicio
        /// correspondiente y delega la persistencia al repositorio.</remarks>
        /// <param name="dto">Datos para crear el fondo especial (incluye Fondo, IdInstitucion y tipoTramiteContrato). IdInstitucion debe
        /// ser mayor que 0.</param>
        /// <returns>Una tarea que representa la operación asíncrona de creación.</returns>
        /// <exception cref="ArgumentException">Se lanza si IdInstitucion no es un identificador válido (≤ 0).</exception>
        public async Task CrearFondoEspecialAsync(CrearFondoDto dto)
        {
            ArgumentNullException.ThrowIfNull(dto);
            await _crearValidator.ValidateAndThrowAsync(dto);
            int tipoTramiteId = dto.tipoTramiteContrato?.Id ?? 0;

            string tipoTramiteNombre = "Pendiente";

            if (tipoTramiteId > 0)
            {
                var tipoTramiteDto = await _tipoTramiteService.ObtenerPorIdAsync(tipoTramiteId);
                if (tipoTramiteDto != null && !string.IsNullOrWhiteSpace(tipoTramiteDto.Tipo))
                {
                    tipoTramiteNombre = tipoTramiteDto.Tipo;
                }
            }

            await _fondoRepo.CreateFondosEspecialesAsync(
                FondoEspecialesMapper.ToEntity(dto, tipoTramiteNombre),
                dto.IdInstitucion);
        }

        /// <summary>
        /// Actualiza un fondo especial de forma asíncrona.
        /// </summary>
        /// <remarks>Valida el contenido de Fondo y resuelve el nombre del tipo de trámite antes de
        /// actualizar el repositorio.</remarks>
        /// <param name="dto">Datos del fondo especial a actualizar; debe incluir IdInstitucion y los datos de Fondo, y puede incluir
        /// TipoTramiteContrato.</param>
        /// <returns>Una tarea que representa la operación asíncrona.</returns>
        /// <exception cref="ArgumentException">Se lanza si IdInstitucion es menor o igual que cero.</exception>
        public async Task ActualizarFondoEspecialAsync(FondoDto dto)
        {
            ArgumentNullException.ThrowIfNull(dto);
            await _actualizarValidator.ValidateAndThrowAsync(dto);
            int tipoTramiteId = dto.TipoTramiteContrato?.Id ?? 0;

            string tipoTramiteNombre = "Pendiente";

            if (tipoTramiteId > 0)
            {
                var tipoTramiteDto = await _tipoTramiteService.ObtenerPorIdAsync(tipoTramiteId);
                if (tipoTramiteDto != null && !string.IsNullOrWhiteSpace(tipoTramiteDto.Tipo))
                {
                    tipoTramiteNombre = tipoTramiteDto.Tipo;
                }
            }

            await _fondoRepo.UpdateFondosEspecialesAsync(
                FondoEspecialesMapper.ToEntity(dto, tipoTramiteNombre),
                dto.IdInstitucion);
        }

        /// <summary>
        /// Elimina el fondo especial identificado por el identificador proporcionado de forma asíncrona.
        /// </summary>
        /// <param name="id">Identificador del fondo especial a eliminar.</param>
        /// <returns>Una tarea que representa la operación asíncrona.</returns>
        public async Task EliminarFondoEspecialAsync(int id)
        {
            await _fondoRepo.DeleteFondosEspecialesAsync(id);
        }

    }

}
