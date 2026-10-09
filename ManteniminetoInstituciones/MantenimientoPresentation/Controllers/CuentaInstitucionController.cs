using Mantenimiento.Core.Application.DTOs.CuentaInstitucion;
using Mantenimiento.Core.Application.InterfaceServices;
using Microsoft.AspNetCore.Mvc;

namespace MantenimientoPresentation.Controllers
{
    public class CuentaInstitucionController : Controller
    {
        private readonly ICuentaInstitucionService _service;
        private readonly IInstitucionService _institucionService;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<CuentaInstitucionController> _logger;

        public CuentaInstitucionController(
            ICuentaInstitucionService service,
            IInstitucionService institucionService,
            ICurrentUserService currentUserService,
            ILogger<CuentaInstitucionController> logger)
        {
            _service = service;
            _institucionService = institucionService;
            _currentUserService = currentUserService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                ViewBag.Instituciones = await _institucionService.ObtenerInstitucionesSelectorAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cargar la lista de instituciones para selección.");
                ViewBag.Instituciones = new List<CuentaInstitucionDto>();
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerCuentasGrid(string insCodigo)
        {
            if (string.IsNullOrWhiteSpace(insCodigo))
                return PartialView("_CuentasGridPartial", new List<CuentaInstitucionDto>());

            try
            {
                var cuentas = await _service.ObtenerCuentasPorEstructuraAsync(insCodigo);
                return PartialView("_CuentasGridPartial", cuentas);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener las cuentas para el código de institución {InsCodigo}.", insCodigo);
                return StatusCode(500, "Ocurrió un error al cargar las cuentas.");
            }
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            try
            {
                if (id <= 0)
                    return Json(new { success = false, message = "Identificador no válido." });

                var cuenta = await _service.ObtenerPorIdAsync(id);
                return Json(new { success = true, data = cuenta });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la cuenta institucional con Id {Id}", id);
                return Json(new { success = false, message = "No se pudo recuperar la información de la cuenta." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] CrearCuentaInstitucionDto dto)
        {
            if (dto == null || !ModelState.IsValid)
                return Json(new { success = false, message = "Datos del formulario inválidos." });

            try
            {
                dto.Usuario = _currentUserService.GetUsername();
                await _service.CrearCuentaAsync(dto);

                _logger.LogInformation("Cuenta institucional registrada exitosamente por {Usuario}.", dto.Usuario);
                return Json(new { success = true, message = "Cuenta registrada exitosamente." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear la cuenta institucional.");
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Actualizar([FromBody] ActualizarCuentaInstitucionDto dto)
        {
            if (dto == null || !ModelState.IsValid)
                return Json(new { success = false, message = "Datos del formulario inválidos." });

            try
            {
                dto.Usuario = _currentUserService.GetUsername();
                await _service.ActualizarCuentaAsync(dto);

                _logger.LogInformation("Cuenta institucional con Id {Id} actualizada por {Usuario}.", dto.Id, dto.Usuario);
                return Json(new { success = true, message = "Cuenta actualizada exitosamente." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar la cuenta institucional con Id {Id}.", dto.Id);
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
                await _service.EliminarCuentaAsync(id);

                _logger.LogInformation("Cuenta institucional con Id {Id} eliminada por {Usuario}.", id, usuario);
                return Json(new { success = true, message = "Cuenta eliminada exitosamente." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar la cuenta institucional con Id {Id}.", id);
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}