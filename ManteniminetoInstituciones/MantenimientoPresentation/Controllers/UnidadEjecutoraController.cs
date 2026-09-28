using Mantenimiento.Core.Application.DTOs.UnidadEjecutora;
using Mantenimiento.Core.Application.InterfaceServices;
using Microsoft.AspNetCore.Mvc;

namespace MantenimientoPresentation.Controllers
{
    public class UnidadEjecutoraController : Controller
    {
        private readonly IUnidadEjecutoraService _service;
        private readonly IDafService _dafService;

        public UnidadEjecutoraController(IUnidadEjecutoraService service, IDafService dafService)
        {
            _service = service;
            _dafService = dafService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            ViewBag.Dafs = await _dafService.ObtenerAsync();
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerGrid(int? id, int? idDaf)
        {
            try
            {
                if (id.HasValue && id.Value > 0)
                {
                    var item = await _service.ObtenerPorIdAsync(id.Value);
                    var listaSingular = item != null ? new List<UnidadEjecutoraDto> { item } : new List<UnidadEjecutoraDto>();
                    return PartialView("_UnidadesEjecutorasGridPartial", listaSingular);
                }

                var lista = await _service.ObtenerAsync(idDaf: idDaf);
                return PartialView("_UnidadesEjecutorasGridPartial", lista);
            }
            catch (Exception ex)
            {
                // Retorna el mensaje de la excepción para identificar la columna o conversión fallida
                var errorMensaje = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return StatusCode(500, errorMensaje);
            }
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            try
            {
                var item = await _service.ObtenerPorIdAsync(id);
                return Json(new { success = true, data = item });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] CrearUnidadEjecutoraDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { success = false, message = "Datos del formulario inválidos." });

            try
            {
                var usuarioActual = !string.IsNullOrWhiteSpace(User.Identity?.Name)
                ? User.Identity.Name
                : (Environment.UserName ?? "SISTEMA");

                dto.Usuario = usuarioActual;
                await _service.CrearAsync(dto);
                return Json(new { success = true, message = "Unidad Ejecutora registrada con éxito." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Actualizar([FromBody] ActualizarUnidadEjecutoraDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { success = false, message = "Datos del formulario inválidos." });

            try
            {
                var usuarioActual = !string.IsNullOrWhiteSpace(User.Identity?.Name)
                ? User.Identity.Name
                : (Environment.UserName ?? "SISTEMA");

                dto.Usuario = usuarioActual;
                await _service.ActualizarAsync(dto);
                return Json(new { success = true, message = "Unidad Ejecutora actualizada con éxito." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Eliminar(int id)
        {
            try
            {
                var usuarioActual = !string.IsNullOrWhiteSpace(User.Identity?.Name)
                ? User.Identity.Name
                : (Environment.UserName ?? "SISTEMA");

                await _service.EliminarAsync(id, usuarioActual);
                return Json(new { success = true, message = "Unidad Ejecutora desactivada correctamente." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}