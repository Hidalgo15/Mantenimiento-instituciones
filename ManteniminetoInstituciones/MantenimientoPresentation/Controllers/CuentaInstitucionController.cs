using Mantenimiento.Core.Application.DTOs;
using Mantenimiento.Core.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace MantenimientoPresentation.Controllers
{
    public class CuentaInstitucionController : Controller
    {
        private readonly CuentaInstitucionService _service;

        public CuentaInstitucionController(CuentaInstitucionService service)
        {
            _service = service;
        }

        // GET: /CuentaInstitucion
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // Carga la lista inicial para el combo/selector
            var instituciones = await _service.ObtenerInstitucionesSelectorAsync();
            ViewBag.Instituciones = instituciones;
            return View();
        }

        // GET: /CuentaInstitucion/ObtenerCuentasGrid?insCodigo=XXX
        [HttpGet]
        public async Task<IActionResult> ObtenerCuentasGrid(string insCodigo)
        {
            if (string.IsNullOrWhiteSpace(insCodigo))
                return PartialView("_CuentasGridPartial", new List<CuentaInstitucionDto>());

            var cuentas = await _service.ObtenerCuentasPorEstructuraAsync(insCodigo);
            return PartialView("_CuentasGridPartial", cuentas);
        }

        // POST: /CuentaInstitucion/Crear
        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] CrearCuentaInstitucionDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { success = false, message = "Datos inválidos." });

            try
            {
                var nuevaCuenta = await _service.CrearCuentaAsync(dto);
                return Json(new { success = true, data = nuevaCuenta });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // POST: /CuentaInstitucion/Eliminar/5
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
