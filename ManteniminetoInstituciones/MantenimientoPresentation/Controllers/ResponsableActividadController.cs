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
            }

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerGrid(string codigoDespa, string codigosSubtipos)
        {
            try
            {
                var disponibles = await _service.ObtenerActividadesPorSubtiposAsync(codigosSubtipos);
                var actuales = await _service.ObtenerActividadesUsuarioAsync(codigoDespa);
                var actualesIds = actuales.Select(a => a.CodigoActividad).ToList();

                return PartialView("_ResponsableActividadGridPartial", (disponibles, actualesIds));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el grid de actividades.");
                return StatusCode(500, "Ocurrió un error al cargar las actividades.");
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
        public async Task<IActionResult> ObtenerUsuariosYActividades(string codigosSubtipos)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(codigosSubtipos))
                    return Json(new { success = false, message = "Debe seleccionar al menos un subtipo." });

                var usuarios = await _service.ObtenerUsuariosPorSubtiposAsync(codigosSubtipos);
                var actividades = await _service.ObtenerActividadesPorSubtiposAsync(codigosSubtipos);

                return Json(new { success = true, usuarios, actividades });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener usuarios y actividades por subtipos.");
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerActividadesUsuario(string codigoDespa)
        {
            try
            {
                var actividadesActuales = await _service.ObtenerActividadesUsuarioAsync(codigoDespa);
                return Json(new { success = true, data = actividadesActuales });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener las actividades asignadas al usuario.");
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> GuardarAsignacion([FromBody] GestionarResponsableActividadDto dto)
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
                _logger.LogError(ex, "Error al sincronizar las actividades del responsable.");
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}