using Mantenimiento.Core.Application.DTOs.ResponsableActividad;
using Mantenimiento.Core.Application.InterfaceServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MantenimientoPresentation.Controllers
{
    [Authorize]
    public class ResponsableActividadController : Controller
    {
        private readonly IResponsableActividadService _service;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<ResponsableActividadController> _logger;

        public ResponsableActividadController(
            IResponsableActividadService service,
            ICurrentUserService currentUserService,
            ILogger<ResponsableActividadController> logger)
        {
            _service = service;
            _currentUserService = currentUserService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                ViewBag.Moldes = await _service.ObtenerCatalogosMoldesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cargar los moldes en la vista inicial.");
                ViewBag.Moldes = new List<MoldeCatalogoDto>();
                ViewBag.ErrorCargaMoldes = "No se pudo cargar la lista de moldes. Actualice la página para intentarlo nuevamente.";
            }

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerGrid(int codigoActividad, int codigoTre, string? filtroCargo, string? filtroArea, string? busquedaGeneral)
        {
            try
            {
                var usuarios = await _service.ObtenerUsuariosPorActividadAsync(codigoActividad, codigoTre, filtroCargo, filtroArea, busquedaGeneral);

                return PartialView("_ResponsableActividadGridPartial", usuarios);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el grid inverso de usuarios por actividad.");
                return StatusCode(500, "Ocurrió un error al cargar los usuarios.");
            }
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerSubtipos(int codigoTre)
        {
            try
            {
                var subtipos = await _service.ObtenerSubtiposPorMoldeAsync(codigoTre);
                return Json(new { success = true, data = subtipos });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener subtipos por molde.");
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerActividadesPorSubtipos(string codigosSubtipos)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(codigosSubtipos))
                    return Json(new { success = false, message = "Debe seleccionar al menos un subtipo." });

                var actividades = await _service.ObtenerActividadesPorSubtiposAsync(codigosSubtipos);
                return Json(new { success = true, actividades });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener actividades por subtipos.");
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerFlujos(int codigoTre)
        {
            try
            {
                var flujos = await _service.ObtenerFlujosPorMoldeAsync(codigoTre);
                return Json(new { success = true, data = flujos });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener flujos por molde.");
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerActividadesPorFlujo(int codigoTre, string nombreFlujo)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(nombreFlujo))
                    return Json(new { success = false, message = "Debe seleccionar un flujo válido." });

                var actividades = await _service.ObtenerActividadesPorFlujoAsync(codigoTre, nombreFlujo);
                return Json(new { success = true, actividades });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener actividades por flujo.");
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerIdsActividadesPorFlujo(int codigoTre, string nombreFlujo)
        {
            try
            {
                var actividades = await _service.ObtenerActividadesPorFlujoAsync(codigoTre, nombreFlujo);
                var codigos = actividades.Select(a => a.CodigoActividad).ToList();
                return Json(new { success = true, codigosActividades = codigos });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener los códigos de actividades del flujo.");
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerActividadesUsuario(string codigoDespa)
        {
            try
            {
                var actividades = await _service.ObtenerActividadesUsuarioAsync(codigoDespa);
                return Json(actividades);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener actividades del usuario.");
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> GuardarAsignacionInversa([FromBody] GestionarActividadUsuariosDto dto)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Datos de asignación inválidos." });
            }

            try
            {
                dto.Usuario = _currentUserService.GetUsername();
                var resultado = await _service.GestionarActividadUsuariosAsync(dto);
                return Json(new { success = resultado.CodigoResultado == 1, message = resultado.Mensaje });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al sincronizar los usuarios de la actividad.");
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> GuardarAsignacionResponsable([FromBody] GestionarResponsableActividadDto dto)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Datos de asignación inválidos." });
            }

            try
            {
                dto.Usuario = _currentUserService.GetUsername();
                var resultado = await _service.GestionarActividadesAsync(dto);
                return Json(new { success = resultado.CodigoResultado == 1, message = resultado.Mensaje });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al gestionar las actividades del responsable.");
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}