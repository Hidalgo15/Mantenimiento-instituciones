using Mantenimiento.Core.Application.DTOs.AnalistaSupervisor;
using Mantenimiento.Core.Application.InterfaceServices;
using Microsoft.AspNetCore.Mvc;

namespace MantenimientoPresentation.Controllers
{
    public class AnalistaSupervisorController : Controller
    {
        private readonly IAnalistaSupervisorService _service;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<AnalistaSupervisorController> _logger;

        public AnalistaSupervisorController(
            IAnalistaSupervisorService service,
            ICurrentUserService currentUserService,
            ILogger<AnalistaSupervisorController> logger)
        {
            _service = service;
            _currentUserService = currentUserService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            // Carga la lista de supervisores para el select del modal de asignación/reasignación
            ViewBag.Supervisores = await _service.ObtenerSupervisoresDisponiblesAsync();
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerGrid(string? nombreAnalista, string? estadoAnalista, string? despachoSupervisor)
        {
            try
            {
                var data = await _service.ObtenerMantenimientoAsync(nombreAnalista, estadoAnalista, despachoSupervisor);
                ViewData["NombreAnalista"] = nombreAnalista;
                ViewData["EstadoAnalista"] = estadoAnalista;
                ViewData["DespachoSupervisor"] = despachoSupervisor;
                return PartialView("_AnalistaSupervisorGridPartial", data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener las relaciones de analistas y supervisores.");
                return StatusCode(500, "Ocurrió un error al cargar las relaciones. Intente nuevamente.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Asignar([FromBody] AsignarSupervisorDto dto)
        {
            if (!ModelState.IsValid)
            {
                var errores = string.Join(" ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));

                return Json(new { success = false, message = string.IsNullOrEmpty(errores) ? "Datos de asignación inválidos." : errores });
            }

            try
            {
                dto.Usuario = _currentUserService.GetUsername();

                await _service.AsignarAsync(dto);
                return Json(new { success = true, message = "Supervisor asignado exitosamente." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al asignar supervisor al analista.");
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Reasignar([FromBody] AsignarSupervisorDto dto)
        {
            if (!ModelState.IsValid)
            {
                var errores = string.Join(" ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));

                return Json(new { success = false, message = string.IsNullOrEmpty(errores) ? "Datos de reasignación inválidos." : errores });
            }

            try
            {
                dto.Usuario = _currentUserService.GetUsername();

                await _service.ReasignarAsync(dto);
                return Json(new { success = true, message = "Supervisor reasignado exitosamente." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al reasignar supervisor.");
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Desvincular([FromBody] DesvincularAnalistaDto dto)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Datos de desvinculación inválidos." });
            }

            try
            {
                dto.Usuario = _currentUserService.GetUsername();

                await _service.DesvincularAsync(dto);
                return Json(new { success = true, message = "Relación eliminada exitosamente." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al desvincular analista.");
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}