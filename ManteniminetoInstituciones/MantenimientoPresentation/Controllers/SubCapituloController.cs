using Mantenimiento.Core.Application.DTOs.Capitulo;
using Mantenimiento.Core.Application.DTOs.SubCapitulo;
using Mantenimiento.Core.Application.InterfaceServices;
using Microsoft.AspNetCore.Mvc;

namespace MantenimientoPresentation.Controllers
{
    public class SubCapituloController : Controller
    {
        private readonly ISubCapituloService _service;
        private readonly ICapituloService _capituloService;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<SubCapituloController> _logger;

        public SubCapituloController(
            ISubCapituloService service,
            ICapituloService capituloService,
            ICurrentUserService currentUserService,
            ILogger<SubCapituloController> logger)
        {
            _service = service;
            _capituloService = capituloService;
            _currentUserService = currentUserService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var capitulos = await _capituloService.ObtenerCapitulosAsync();
                ViewBag.Capitulos = capitulos;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cargar los capítulos para el filtro de SubCapítulos.");
                ViewBag.Capitulos = new List<CapituloDto>();
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerGrid(int capituloId)
        {
            try
            {
                var subCapitulos = await _service.ObtenerSubCapitulosPorCapituloAsync(capituloId);
                return PartialView("_SubCapitulosGridPartial", subCapitulos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el grid de subcapítulos para CapítuloId {CapituloId}.", capituloId);
                return StatusCode(500, "Error al cargar la lista de subcapítulos.");
            }
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerPorId(int id, int id_capitulo = 0, string? buscar = null)
        {
            try
            {
                if (id <= 0)
                    return Json(new { success = false, message = "Identificador no válido." });

                var subCapitulo = await _service.ObtenerPorIdAsync(id, id_capitulo, buscar ?? string.Empty);
                return Json(new { success = true, data = subCapitulo });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el subcapítulo con Id {Id}.", id);
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] CrearSubCapituloDto dto)
        {
            if (dto == null || !ModelState.IsValid)
            {
                var errores = string.Join(" ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));

                return Json(new { success = false, message = string.IsNullOrEmpty(errores) ? "Datos del formulario inválidos." : errores });
            }

            try
            {
                dto.Usuario = _currentUserService.GetUsername();
                await _service.CrearSubCapituloAsync(dto);

                _logger.LogInformation("Subcapítulo registrado exitosamente por {Usuario}.", dto.Usuario);
                return Json(new { success = true, message = "Subcapítulo registrado exitosamente." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al registrar el subcapítulo.");
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Actualizar([FromBody] ActualizarSubCapituloDto dto)
        {
            if (dto == null || !ModelState.IsValid)
            {
                var errores = string.Join(" ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));

                return Json(new { success = false, message = string.IsNullOrEmpty(errores) ? "Datos del formulario inválidos." : errores });
            }

            try
            {
                dto.Usuario = _currentUserService.GetUsername();
                await _service.ActualizarSubCapituloAsync(dto);

                _logger.LogInformation("Subcapítulo con Id {Id} actualizado por {Usuario}.", dto.Id, dto.Usuario);
                return Json(new { success = true, message = "Subcapítulo actualizado exitosamente." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el subcapítulo con Id {Id}.", dto.Id);
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Eliminar(int id)
        {
            if (id <= 0)
                return Json(new { success = false, message = "Identificador no válido." });

            try
            {
                var usuario = _currentUserService.GetUsername();
                await _service.EliminarSubCapituloAsync(id);

                _logger.LogInformation("Subcapítulo con Id {Id} eliminado por {Usuario}.", id, usuario);
                return Json(new { success = true, message = "Subcapítulo eliminado exitosamente." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el subcapítulo con Id {Id}.", id);
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerPorCapituloId(int capituloId)
        {
            var data = await _service.ObtenerSubCapitulosPorCapituloAsync(capituloId);
            return Json(new { success = true, data });
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerPorCodigoCapitulo(string codigoCapitulo)
        {
            var data = await _service.ObtenerSubCapitulosPorCodigoCapituloAsync(codigoCapitulo);
            return Json(new { success = true, data });
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerPorNombreCapitulo(string nombreCapitulo)
        {
            var data = await _service.ObtenerSubCapitulosPorNombreCapituloAsync(nombreCapitulo);
            return Json(new { success = true, data });
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerPorCodigoSubCapitulo(string codigoSubCapitulo)
        {
            var data = await _service.ObtenerSubCapitulosPorCodigoSubCapituloAsync(codigoSubCapitulo);
            return Json(new { success = true, data });
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerPorNombreSubCapitulo(string nombreSubCapitulo)
        {
            var data = await _service.ObtenerSubCapitulosPorNombreSubCapituloAsync(nombreSubCapitulo);
            return Json(new { success = true, data });
        }
    }
}