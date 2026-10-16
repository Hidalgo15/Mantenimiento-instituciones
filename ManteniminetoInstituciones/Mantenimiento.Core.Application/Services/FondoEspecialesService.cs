using Mantenimiento.Core.Application.DTOs.Fondos;
using Mantenimiento.Core.Application.InterfaceServices;
using Mantenimiento.Core.Application.Mappings;
using Mantenimiento.Core.Domain.RepositoryInterfaces;


namespace Mantenimiento.Core.Application.Services
{
    public class FondoEspecialesService : IFondoEspecialesService
    {
        private readonly IFondoEspecialesRepository _fondoRepo;
        private readonly ITipoTramiteService _tipoTramiteService;

        public FondoEspecialesService(
            IFondoEspecialesRepository fondoRepo,
            ITipoTramiteService tipoTramiteService)
        {
            _fondoRepo = fondoRepo;
            _tipoTramiteService = tipoTramiteService;
        }

        /// <summary>
        /// Obtiene una lista de fondos especiales filtrados por estructura y tipo de trámite.
        /// </summary>
        /// <param name="estructura"></param>
        /// <param name="tipoTramite"></param>
        /// <returns></returns>
        public async Task<List<FondoDto>> ObtenerFondosEspecialesAsync(string? estructura = null, string? tipoTramite = null)
        {
            var lista = await _fondoRepo.GetAllFondosEspecialesAsync();
            var query = lista.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(estructura))
            {
                query = query.Where(f => string.Equals(
                    f.Estructura,
                    estructura,
                    StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace(tipoTramite))
            {
                query = query.Where(f => f.TipoTramite != null &&
                    f.TipoTramite.Equals(tipoTramite, StringComparison.OrdinalIgnoreCase));
            }

            return query.Select(FondoEspecialesMapper.ToDto).ToList();
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
            int tipoTramiteId = dto.tipoTramiteContrato?.Id ?? 0;
            ValidarFondo(dto.Fondo);

            if (dto.IdInstitucion <= 0)
                throw new ArgumentException("Debe seleccionar una institución válida.");

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
            int tipoTramiteId = dto.TipoTramiteContrato?.Id ?? 0;
            ValidarFondo(dto.Fondo);

            if (dto.IdInstitucion <= 0)
                throw new ArgumentException("Debe seleccionar una institución válida.");

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

        /// <summary>
        /// Valida que 'fondo' no sea nulo, vacío o compuesto solo por espacios y que, si es numérico, sea un entero
        /// positivo.
        /// </summary>
        /// <remarks>La comprobación numérica solo se realiza si la cadena puede convertirse a un entero
        /// (int); las cadenas no numéricas válidas se aceptan.</remarks>
        /// <param name="fondo">Cadena que representa el valor del fondo; no puede ser nula, vacía ni contener solo espacios; si representa
        /// un número, debe ser un entero positivo.</param>
        /// <exception cref="ArgumentException">Se lanza cuando 'fondo' es nulo, vacío o contiene solo espacios, o cuando representa un número entero menor
        /// o igual a cero.</exception>
        private static void ValidarFondo(string fondo)
        {
            if (string.IsNullOrWhiteSpace(fondo))
                throw new ArgumentException("El campo fondo es requerido.");

            if (int.TryParse(fondo, out var fondoValue) && fondoValue <= 0)
                throw new ArgumentException("El campo fondo debe ser un número positivo.");
        }
    }

}
