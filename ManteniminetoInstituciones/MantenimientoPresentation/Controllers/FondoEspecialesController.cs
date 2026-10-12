using Mantenimiento.Core.Application.DTOs.Fondos;
using Mantenimiento.Core.Application.DTOs.Institucion;
using Mantenimiento.Core.Application.DTOs.TipoTramite;
using Mantenimiento.Core.Application.InterfaceServices;
using Microsoft.AspNetCore.Mvc;

namespace MantenimientoPresentation.Controllers
{
    [Route("FondoEspeciales/[action]/{id?}")]
    public class FondoEspecialesController : Controller
    {
        private readonly IFondoEspecialesService _fondoService;
        private readonly ITipoTramiteService _tipoTramiteService;
        private readonly IInstitucionService _institucionService;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<FondoEspecialesController> _logger;

        public FondoEspecialesController(
            IFondoEspecialesService fondoService,
            ITipoTramiteService tipoTramiteService,
            IInstitucionService institucionService,
            ICurrentUserService currentUserService,
            ILogger<FondoEspecialesController> logger)
        {
            _fondoService = fondoService;
            _tipoTramiteService = tipoTramiteService;
            _institucionService = institucionService;
            _currentUserService = currentUserService;
            _logger = logger;
        }

        [HttpGet("")]
        [HttpGet("Index")]
        public async Task<IActionResult> Index()
        {
            try
            {
                var tiposTramite = await _tipoTramiteService.ObtenerTiposTramiteAsync();
                ViewBag.TiposTramite = tiposTramite;

                var instituciones = await _institucionService.ObtenerInstitucionesSelectorAsync();
                ViewBag.Instituciones = instituciones.OrderBy(i => i.UnidadEjecutora).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cargar los catálogos en Fondos Especiales.");
                ViewBag.TiposTramite = new List<TipoTramiteDto>();
                ViewBag.Instituciones = new List<InstitucionSeleccionDto>();
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerGrid(string? estructura = null, string? tipoTramite = null)
        {
            try
            {
                var fondos = await _fondoService.ObtenerFondosEspecialesAsync(estructura, tipoTramite);
                return PartialView("_FondosGridPartial", fondos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cargar el grid de fondos especiales.");
                return StatusCode(500, "Error al cargar los datos.");
            }
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            try
            {
                if (id <= 0)
                    return Json(new { success = false, message = "Identificador no válido." });

                var fondo = await _fondoService.ObtenerPorIdAsync(id);
                return Json(new { success = true, data = fondo });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el fondo especial con Id {Id}", id);
                return Json(new { success = false, message = "No se pudo recuperar la información del fondo especial." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] CrearFondoDto dto)
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
                await _fondoService.CrearFondoEspecialAsync(dto);

                _logger.LogInformation("Fondo especial creado exitosamente por {Usuario}.", dto.Usuario);
                return Json(new { success = true, message = "Fondo especial registrado exitosamente." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al registrar el fondo especial.");
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Actualizar([FromBody] FondoDto dto)
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
                await _fondoService.ActualizarFondoEspecialAsync(dto);

                _logger.LogInformation("Fondo especial con Id {Id} actualizado por {Usuario}.", dto.Id, dto.Usuario);
                return Json(new { success = true, message = "Fondo especial actualizado exitosamente." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el fondo especial con Id {Id}.", dto.Id);
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
                await _fondoService.EliminarFondoEspecialAsync(id);

                _logger.LogInformation("Fondo especial con Id {Id} eliminado por {Usuario}.", id, usuario);
                return Json(new { success = true, message = "Fondo especial eliminado exitosamente." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el fondo especial con Id {Id}.", id);
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}