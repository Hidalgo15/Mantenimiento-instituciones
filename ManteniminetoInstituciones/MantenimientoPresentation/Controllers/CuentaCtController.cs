using Mantenimiento.Core.Application.DTOs.Cuenta.CtCuentasDTO;
using Mantenimiento.Core.Application.InterfaceServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MantenimientoPresentation.Controllers
{
    [Authorize]
    public class CuentaCtController : Controller
    {
        private readonly ICuentaCtService _cuentaCtService;
        private readonly ICurrentUserService _currentUserService;

        public CuentaCtController(ICuentaCtService cuentaCtService, ICurrentUserService currentUserService)
        {
            _cuentaCtService = cuentaCtService;
            _currentUserService = currentUserService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerGrid(string? busqueda = null)
        {
            var cuentas = await _cuentaCtService.ObtenerCuentasAsync(busqueda);
            return PartialView("_CuentasCtGridPartial", cuentas);
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            try
            {
                var cuenta = await _cuentaCtService.ObtenerPorIdAsync(id);
                if (cuenta == null)
                    return Json(new { success = false, message = "La cuenta no fue encontrada." });

                return Json(new { success = true, data = cuenta });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] CrearCuentaCtDto dto)
        {
            try
            {
                dto.Usuario = _currentUserService.GetUsername();
                await _cuentaCtService.CrearCuentaAsync(dto);
                return Json(new { success = true, message = "Cuenta creada exitosamente." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Actualizar([FromBody] ActualizarCuentaCtDto dto)
        {
            try
            {
                dto.Usuario = _currentUserService.GetUsername();
                await _cuentaCtService.ActualizarCuentaAsync(dto);
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
                var usuario = _currentUserService.GetUsername();
                await _cuentaCtService.EliminarCuentaAsync(id);
                return Json(new { success = true, message = "Cuenta eliminada exitosamente." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}
