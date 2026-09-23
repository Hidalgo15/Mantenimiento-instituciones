using Mantenimiento.Core.Application.DTOs.CuentaInstitucion;
using Mantenimiento.Core.Application.InterfaceServices;
using Microsoft.AspNetCore.Mvc;

namespace MantenimientoPresentation.Controllers
{
    public class CuentaInstitucionController : Controller
    {
        private readonly ICuentaInstitucionService _service;

        public CuentaInstitucionController(ICuentaInstitucionService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            ViewBag.Instituciones = await _service.ObtenerInstitucionesSelectorAsync();
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerCuentasGrid(string insCodigo)
        {
            if (string.IsNullOrWhiteSpace(insCodigo))
                return PartialView("_CuentasGridPartial", new List<CuentaInstitucionDto>());

            var cuentas = await _service.ObtenerCuentasPorEstructuraAsync(insCodigo);
            return PartialView("_CuentasGridPartial", cuentas);
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            try
            {
                var cuenta = await _service.ObtenerPorIdAsync(id);
                return Json(new { success = true, data = cuenta });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] CrearCuentaInstitucionDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { success = false, message = "Datos del formulario inválidos." });

            try
            {
                await _service.CrearCuentaAsync(dto);
                return Json(new { success = true, message = "Cuenta registrada exitosamente." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Actualizar([FromBody] ActualizarCuentaInstitucionDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { success = false, message = "Datos inválidos." });

            try
            {
                await _service.ActualizarCuentaAsync(dto);
                return Json(new { success = true, message = "Cuenta actualizada exitosamente." });
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
                await _service.EliminarCuentaAsync(id);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}