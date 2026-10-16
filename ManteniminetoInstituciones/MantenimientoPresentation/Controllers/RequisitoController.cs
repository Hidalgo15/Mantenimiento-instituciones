using Mantenimiento.Core.Application.DTOs.Requisito;
using Mantenimiento.Core.Application.InterfaceServices;
using MantenimientoPresentation.Services;
using Microsoft.AspNetCore.Mvc;

namespace MantenimientoPresentation.Controllers
{
    public class RequisitoController : Controller
    {
        private readonly IRequisitoService _service;
        private readonly ILogger<RequisitoController> _logger;
        private readonly ICurrentUserService _currentUserService;

        public RequisitoController(IRequisitoService service, ILogger<RequisitoController> logger, ICurrentUserService currentUserService)
        {
            _service = service;
            _logger = logger;
            _currentUserService = currentUserService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerGrid(string? busqueda)
        {
            try
            {
                var data = await _service.ObtenerRequisitosAsync(busqueda);
                ViewData["Busqueda"] = busqueda;
                return PartialView("_RequisitoGridPartial", data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la lista de requisitos.");
                return StatusCode(500, "Ocurrió un error al cargar los datos. Intente nuevamente.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Guardar([FromBody] CrearRequisitoDto dto)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, message = "Datos inválidos." });

            try
            {
                dto.Usuario = _currentUserService.GetUsername();
                var resultado = await _service.CrearAsync(dto);
                return Json(new { success = resultado.CodigoResultado == 1, message = resultado.Mensaje });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear el requisito.");
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Actualizar([FromBody] ActualizarRequisitoDto dto)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, message = "Datos inválidos." });

            try
            {
                dto.Usuario = _currentUserService.GetUsername();
                var resultado = await _service.ActualizarAsync(dto);
                return Json(new { success = resultado.CodigoResultado == 1, message = resultado.Mensaje });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el requisito.");
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Eliminar([FromBody] int requisitosID)
        {
            try
            {
                var usuario = _currentUserService.GetUsername();
                var resultado = await _service.EliminarAsync(requisitosID);
                return Json(new { success = resultado.CodigoResultado == 1, message = resultado.Mensaje });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el requisito.");
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}