using Mantenimiento.Core.Application.DTOs.Impedimentos;
using Mantenimiento.Core.Application.InterfaceServices;
using MantenimientoPresentation.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MantenimientoPresentation.Controllers
{
    [Authorize]
    [AutoValidateAntiforgeryToken]
    public class ImpedimentosController : Controller
    {
        private readonly IImpedimentoService _service;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<ImpedimentosController> _logger;
        private readonly bool _gridPaginationEnabled;

        public ImpedimentosController(
            IImpedimentoService service,
            ICurrentUserService currentUserService,
            ILogger<ImpedimentosController> logger,
            IConfiguration configuration)
        {
            _service = service;
            _currentUserService = currentUserService;
            _logger = logger;
            _gridPaginationEnabled = configuration.GetValue<bool>("GridPagination:Enabled");
        }

        [HttpGet]
        public IActionResult Index() => View();

        [HttpGet]
        public async Task<IActionResult> ObtenerGrid(string? filtroTexto, int page = 1, int pageSize = 10)
        {
            if (filtroTexto?.Length > 255)
                return BadRequest("El texto de búsqueda no puede exceder 255 caracteres.");

            try
            {
                if (_gridPaginationEnabled)
                {
                    var resultado = await _service.ObtenerPaginadoAsync(filtroTexto, page, pageSize);
                    GridPaginationViewData.Set(this, resultado);
                    return PartialView("_ImpedimentosGridPartial", resultado.Items);
                }

                var registros = await _service.ObtenerAsync(filtroTexto);
                return PartialView("_ImpedimentosGridPartial", registros);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cargar el listado de impedimentos.");
                return StatusCode(StatusCodes.Status500InternalServerError, "No se pudo cargar el listado de impedimentos.");
            }
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            if (id <= 0)
                return BadRequest(new { success = false, message = "Identificador no válido." });

            try
            {
                var impedimento = await _service.ObtenerPorIdAsync(id);
                return impedimento == null
                    ? NotFound(new { success = false, message = "No se encontró el impedimento solicitado." })
                    : Json(new { success = true, data = impedimento });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el impedimento con Id {Id}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError, new { success = false, message = "No se pudo recuperar el impedimento." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] CrearImpedimentoDto? dto)
        {
            if (dto == null || !ModelState.IsValid)
                return BadRequest(new { success = false, message = "Datos del formulario inválidos." });

            try
            {
                dto.UsuarioAgrego = _currentUserService.GetUsername();
                var id = await _service.CrearAsync(dto);
                _logger.LogInformation("Impedimento {Id} registrado por {Usuario}.", id, dto.UsuarioAgrego);
                return StatusCode(StatusCodes.Status201Created, new { success = true, id, message = "Impedimento registrado exitosamente." });
            }
            catch (FluentValidation.ValidationException ex)
            {
                return BadRequest(new { success = false, message = string.Join(" ", ex.Errors.Select(error => error.ErrorMessage)) });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al registrar un impedimento.");
                return StatusCode(StatusCodes.Status500InternalServerError, new { success = false, message = "No se pudo registrar el impedimento." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Actualizar([FromBody] ActualizarImpedimentoDto? dto)
        {
            if (dto == null || !ModelState.IsValid)
                return BadRequest(new { success = false, message = "Datos del formulario inválidos." });

            try
            {
                await _service.ActualizarAsync(dto);
                _logger.LogInformation("Impedimento {Id} actualizado.", dto.Id);
                return Json(new { success = true, message = "Impedimento actualizado exitosamente." });
            }
            catch (FluentValidation.ValidationException ex)
            {
                return BadRequest(new { success = false, message = string.Join(" ", ex.Errors.Select(error => error.ErrorMessage)) });
            }
            catch (ArgumentOutOfRangeException)
            {
                return BadRequest(new { success = false, message = "Identificador no válido." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el impedimento con Id {Id}.", dto.Id);
                return StatusCode(StatusCodes.Status500InternalServerError, new { success = false, message = "No se pudo actualizar el impedimento." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Eliminar(int id)
        {
            if (id <= 0)
                return BadRequest(new { success = false, message = "Identificador no válido." });

            try
            {
                await _service.EliminarAsync(id);
                _logger.LogInformation("Impedimento {Id} deshabilitado.", id);
                return Json(new { success = true, message = "Impedimento deshabilitado exitosamente." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al deshabilitar el impedimento con Id {Id}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError, new { success = false, message = "No se pudo deshabilitar el impedimento." });
            }
        }
    }
}
