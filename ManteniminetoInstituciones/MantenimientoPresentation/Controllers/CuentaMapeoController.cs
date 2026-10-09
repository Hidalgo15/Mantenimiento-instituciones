using Mantenimiento.Core.Application.DTOs.Cuenta;
using Mantenimiento.Core.Application.InterfaceServices;
using Microsoft.AspNetCore.Mvc;

namespace MantenimientoPresentation.Controllers
{
    [Route("CuentaMapeo/[action]/{id?}")]
    public class CuentaMapeoController : Controller
    {
        private readonly ICuentaMapeoService _cuentaMapeoService;
        private readonly ITipoTramitePagoService _tipoTramitePagoService; // Servicio que lista dbo.tipo_tramite_pago

        public CuentaMapeoController(
            ICuentaMapeoService cuentaMapeoService,
            ITipoTramitePagoService tipoTramitePagoService)
        {
            _cuentaMapeoService = cuentaMapeoService;
            _tipoTramitePagoService = tipoTramitePagoService;
        }

        [HttpGet("")]
        [HttpGet("Index")]
        public async Task<IActionResult> Index()
        {
            // Cargar el catálogo base de cuentas (ct.cuentas) ordenado alfabéticamente
            var catalogoBase = await _cuentaMapeoService.ObtenerCatalogoBaseAsync();
            ViewBag.CatalogoBase = catalogoBase.OrderBy(c => c.CodigoCuenta).ToList();

            // Cargar los tipos de trámite de pago (dbo.tipo_tramite_pago)
            var tiposTramite = await _tipoTramitePagoService.ObtenerTodosAsync();
            ViewBag.TiposTramite = tiposTramite.OrderBy(t => t.Tipo).ToList();

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerGrid(int? tipoTramite = null)
        {
            var cuentas = await _cuentaMapeoService.ObtenerCuentasAsync(tipoTramite);
            return PartialView("_CuentasGridPartial", cuentas);
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            try
            {
                var cuenta = await _cuentaMapeoService.ObtenerPorIdAsync(id);
                return Json(new { success = true, data = cuenta });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] CrearCuentaDto dto)
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
                await _cuentaMapeoService.CrearCuentaAsync(dto);
                return Json(new { success = true, message = "Cuenta registrada exitosamente." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Actualizar([FromBody] ActualizarCuentaDto dto)
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
                await _cuentaMapeoService.ActualizarCuentaAsync(dto);
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
                await _cuentaMapeoService.EliminarCuentaAsync(id);
                return Json(new { success = true, message = "Cuenta eliminada exitosamente." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}