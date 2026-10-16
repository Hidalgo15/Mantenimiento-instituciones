using Mantenimiento.Core.Application.DTOs.ClasificacionContrato;
using Mantenimiento.Core.Application.InterfaceServices;
using Microsoft.AspNetCore.Mvc;

namespace MantenimientoPresentation.Controllers
{
    public class ClasificacionContratoController : Controller
    {
        private readonly IClasificacionContratoService _service;
        private readonly ILogger<ClasificacionContratoController> _logger;
        private readonly ICurrentUserService _currentUserService;

        public ClasificacionContratoController(IClasificacionContratoService service, ILogger<ClasificacionContratoController> logger, ICurrentUserService currentUserService)
        {
            _service = service;
            _logger = logger;
            _currentUserService = currentUserService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                ViewBag.ContratoTipos = await _service.ObtenerOpcionesAsync("CONTRATO_TIPO");
                ViewBag.TiposOperacion = await _service.ObtenerOpcionesAsync("TIPO_OPERACION");
                ViewBag.ProcesosSeleccion = await _service.ObtenerCatalogoProcesoSeleccionAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cargar los catálogos para clasificación de contratos.");
                ViewBag.ContratoTipos = new List<ClasificacionOpcionDto>();
                ViewBag.TiposOperacion = new List<ClasificacionOpcionDto>();
                ViewBag.ProcesosSeleccion = new List<ProcesoSeleccionCatalogoDto>();
            }

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerGrid(string? contratoTipo, string? tipoOperacion, string? contratoProcesoSeleccion)
        {
            try
            {
                var data = await _service.ObtenerAsync(null, contratoTipo, tipoOperacion, contratoProcesoSeleccion);
                return PartialView("_ClasificacionContratoGridPartial", data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el grid de clasificación de contratos.");
                return StatusCode(500, "Ocurrió un error al cargar los datos.");
            }
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            try
            {
                var data = await _service.ObtenerPorIdAsync(id);
                if (data == null)
                    return Json(new { success = false, message = "Registro no encontrado." });

                return Json(new { success = true, data });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la clasificación por ID.");
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] CrearClasificacionContratoDto dto)
        {
            try
            {
                dto.Usuario = _currentUserService.GetUsername();
                var idGenerado = await _service.CrearAsync(dto);
                return Json(new { success = true, message = "Clasificación creada exitosamente.", id = idGenerado });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear la clasificación de contrato.");
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Actualizar([FromBody] ActualizarClasificacionContratoDto dto)
        {
            try
            {
                dto.Usuario = _currentUserService.GetUsername();
                await _service.ActualizarAsync(dto);
                return Json(new { success = true, message = "Clasificación actualizada exitosamente." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar la clasificación de contrato.");
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Eliminar([FromBody] int id)
        {
            try
            {
                var usuario = _currentUserService.GetUsername();
                await _service.EliminarAsync(id);
                return Json(new { success = true, message = "Clasificación eliminada exitosamente." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar la clasificación de contrato.");
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}