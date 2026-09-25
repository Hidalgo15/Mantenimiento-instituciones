using Mantenimiento.Core.Application.DTOs.SubCapitulo;
using Mantenimiento.Core.Application.InterfaceServices;
using Mantenimiento.Core.Domain.Entities;
using Microsoft.AspNetCore.Mvc;


namespace MantenimientoPresentation.Controllers
{
    public class SubCapituloController : Controller
    {
        private readonly ISubCapituloService _service;
        public SubCapituloController(ISubCapituloService service)
        { 
            _service = service;
        }

        public async Task<IActionResult> Index()
        {
            // Carga la lista inicial para popular el <select> en el Index.cshtml
            var subCapitulos = await _service.ObtenerSubCapitulosAsync(null);
            ViewBag.SubCapitulos = subCapitulos;
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerGrid(string? codigo)
        {
            var subCapitulos = await _service.ObtenerSubCapitulosAsync(codigo);
            return PartialView("_SubCapitulosGridPartial", subCapitulos);
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerPorId(int id, int id_capitulo = 0, string? buscar = null)
        {
            try
            {
                var subCapitulo = await _service.ObtenerPorIdAsync(id, id_capitulo, buscar ?? string.Empty);
                return Json(new { success = true, data = subCapitulo });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] CrearSubCapituloDto dto)
        {
            if (!ModelState.IsValid)
            {
                var errores = string.Join(" ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));

                return Json(new { success = false, message = string.IsNullOrEmpty(errores) ? "Datos del formulario inválidos." : errores });
            }

            try
            {
                await _service.CrearSubCapituloAsync(dto);
                return Json(new { success = true, message = "Subcapítulo registrado exitosamente." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Actualizar([FromBody] ActualizarSubCapituloDto dto)
        {
            if (!ModelState.IsValid)
            {
                var errores = string.Join(" ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));

                return Json(new { success = false, message = string.IsNullOrEmpty(errores) ? "Datos del formulario inválidos." : errores });
            }

            try
            {
                await _service.ActualizarSubCapituloAsync(dto);
                return Json(new { success = true, message = "Subcapítulo actualizado exitosamente." });
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
                await _service.EliminarSubCapituloAsync(id);
                return Json(new { success = true, message = "Subcapítulo eliminado exitosamente." });
            }
            catch (Exception ex)
            {
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
