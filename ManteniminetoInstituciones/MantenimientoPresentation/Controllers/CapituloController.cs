using Mantenimiento.Core.Application.DTOs.Capitulo;
using Mantenimiento.Core.Application.InterfaceServices;
using Microsoft.AspNetCore.Mvc;

namespace MantenimientoPresentation.Controllers
{
    public class CapituloController : Controller
    {
        private readonly ICapituloService _service;

        public CapituloController(ICapituloService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var capitulos = await _service.ObtenerCapitulosAsync(null);
            ViewBag.Capitulos = capitulos;
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerGrid(string? codigo)
        {
            var capitulos = await _service.ObtenerCapitulosAsync(codigo);
            return PartialView("_CapitulosGridPartial", capitulos);
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            try
            {
                var capitulo = await _service.ObtenerPorIdAsync(id);
                return Json(new { success = true, data = capitulo });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] CrearCapituloDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { success = false, message = "Datos del formulario inválidos." });

            try
            {
                await _service.CrearCapituloAsync(dto);
                return Json(new { success = true, message = "Capítulo registrado exitosamente." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Actualizar([FromBody] ActualizarCapituloDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { success = false, message = "Datos inválidos." });

            try
            {
                await _service.ActualizarCapituloAsync(dto);
                return Json(new { success = true, message = "Capítulo actualizado exitosamente." });
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
                await _service.EliminarCapituloAsync(id);
                return Json(new { success = true, message = "Capítulo eliminado exitosamente." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}