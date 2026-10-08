using Mantenimiento.Core.Application.DTOs.Fondos;
using Mantenimiento.Core.Application.InterfaceServices;
using Mantenimiento.Core.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace MantenimientoPresentation.Controllers
{

    [Route("FondoEspeciales/[action]/{id?}")]
    public class FondoEspecialesController : Controller
    {

        private readonly IFondoEspecialesService _fondoService;
        private readonly ITipoTramiteService _tipoTramiteService;
        private readonly IUnidadEjecutoraService _unidadEjecutoraService; // 1. Inyectar el servicio

        public FondoEspecialesController(
            IFondoEspecialesService fondoService,
            ITipoTramiteService tipoTramiteService,
            IUnidadEjecutoraService unidadEjecutoraService) // 1. Inyectar el servicio)
        {
            _fondoService = fondoService;
            _tipoTramiteService = tipoTramiteService;
            _unidadEjecutoraService = unidadEjecutoraService;
        }

        [HttpGet("")]
        [HttpGet("Index")]
        public async Task<IActionResult> Index()
        {
            // Carga los tipos de trámite desde el nuevo ITipoTramiteService
            var tiposTramite = await _tipoTramiteService.ObtenerTiposTramiteAsync();
            ViewBag.TiposTramite = tiposTramite;
            var unidadejecutora = await _unidadEjecutoraService.ObtenerAsync();
            ViewBag.UnidadEjecutora = unidadejecutora;
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerGrid(string? descripcion = null, string? tipoTramite = null)
        {
            var fondos = await _fondoService.ObtenerFondosEspecialesAsync(descripcion, tipoTramite);
            return PartialView("_FondosGridPartial", fondos);
        }


        [HttpGet]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            try
            {
                var fondo = await _fondoService.ObtenerPorIdAsync(id);
                return Json(new { success = true, data = fondo });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] CrearFondoDto dto)
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
                await _fondoService.CrearFondoEspecialAsync(dto);
                return Json(new { success = true, message = "Fondo especial registrado exitosamente." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Actualizar([FromBody] FondoDto dto)
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
                await _fondoService.ActualizarFondoEspecialAsync(dto);
                return Json(new { success = true, message = "Fondo especial actualizado exitosamente." });
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
                await _fondoService.EliminarFondoEspecialAsync(id);
                return Json(new { success = true, message = "Fondo especial eliminado exitosamente." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }



    }
}
