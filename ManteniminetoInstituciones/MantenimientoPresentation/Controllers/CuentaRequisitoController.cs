using Mantenimiento.Core.Application.DTOs.CuentaRequisito;
using Mantenimiento.Core.Application.InterfaceServices;
using Microsoft.AspNetCore.Mvc;

namespace MantenimientoPresentation.Controllers
{
    public class CuentaRequisitoController : Controller
    {
        private readonly ICuentaRequisitoService _service;
        private readonly ILogger<CuentaRequisitoController> _logger;

        public CuentaRequisitoController(ICuentaRequisitoService service, ILogger<CuentaRequisitoController> logger)
        {
            _service = service;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Cuentas = await _service.ObtenerCatalogoCuentasAsync();
            ViewBag.Requisitos = await _service.ObtenerCatalogoRequisitosAsync();
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerGrid(double? idCuenta, int? idRequisito, bool? estado)
        {
            try
            {
                var data = await _service.ObtenerRelacionAsync(idCuenta, idRequisito, estado);
                ViewData["IdCuenta"] = idCuenta;
                ViewData["IdRequisito"] = idRequisito;
                ViewData["Estado"] = estado;
                return PartialView("_CuentaRequisitoGridPartial", data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener las relaciones de cuenta y requisitos.");
                return StatusCode(500, "Ocurrió un error al cargar los datos. Intente nuevamente.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Guardar([FromBody] CrearCuentaRequisitoDto dto)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, message = "Datos inválidos." });

            try
            {
                var resultado = await _service.AsignarAsync(dto);
                return Json(new { success = resultado.CodigoResultado == 1, message = resultado.Mensaje });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al asignar requisito a la cuenta.");
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Actualizar([FromBody] ActualizarCuentaRequisitoDto dto)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, message = "Datos inválidos." });

            try
            {
                var resultado = await _service.ActualizarAsync(dto);
                return Json(new { success = resultado.CodigoResultado == 1, message = resultado.Mensaje });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar la relación cuenta-requisito.");
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Eliminar([FromBody] int cuentaRequisitosID)
        {
            try
            {
                var resultado = await _service.EliminarAsync(cuentaRequisitosID);
                return Json(new { success = resultado.CodigoResultado == 1, message = resultado.Mensaje });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar la relación cuenta-requisito.");
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}