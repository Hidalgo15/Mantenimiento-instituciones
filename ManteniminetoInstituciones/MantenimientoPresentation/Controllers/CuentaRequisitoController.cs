using Mantenimiento.Core.Application.DTOs.CuentaRequisito;
using Mantenimiento.Core.Application.InterfaceServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MantenimientoPresentation.Controllers
{
    [Authorize]
    [AutoValidateAntiforgeryToken]
    public class CuentaRequisitoController : Controller
    {
        private readonly ICuentaRequisitoService _service;
        private readonly ILogger<CuentaRequisitoController> _logger;
        private readonly ICurrentUserService _currentUserService;

        public CuentaRequisitoController(ICuentaRequisitoService service, ILogger<CuentaRequisitoController> logger, ICurrentUserService currentUserService)
        {
            _service = service;
            _logger = logger;
            _currentUserService = currentUserService;
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
            catch (Exception ex) when (ex is not FluentValidation.ValidationException && ex is not ArgumentException && ex is not KeyNotFoundException && ex is not InvalidOperationException)
            {
                _logger.LogError(ex, "Error al obtener las relaciones de cuenta y requisitos.");
                return StatusCode(500, "Ocurrió un error al cargar los datos. Intente nuevamente.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Guardar([FromBody] CrearCuentaRequisitoDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { success = false, message = "Datos inválidos." });

            try
            {
                dto.Usuario = _currentUserService.GetUsername();
                var resultado = await _service.AsignarAsync(dto);
                if (resultado.CodigoResultado != 1)
                    return Conflict(new { success = false, message = resultado.Mensaje });

                return StatusCode(StatusCodes.Status201Created, new { success = true, message = resultado.Mensaje });
            }
            catch (Exception ex) when (ex is not FluentValidation.ValidationException && ex is not ArgumentException && ex is not KeyNotFoundException && ex is not InvalidOperationException)
            {
                _logger.LogError(ex, "Error al asignar requisito a la cuenta.");
                return StatusCode(StatusCodes.Status500InternalServerError, new { success = false, message = "Ocurrió un error inesperado al procesar la solicitud. Inténtelo nuevamente." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Actualizar([FromBody] ActualizarCuentaRequisitoDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { success = false, message = "Datos inválidos." });

            try
            {
                dto.Usuario = _currentUserService.GetUsername();
                var resultado = await _service.ActualizarAsync(dto);
                if (resultado.CodigoResultado != 1)
                    return Conflict(new { success = false, message = resultado.Mensaje });

                return Json(new { success = true, message = resultado.Mensaje });
            }
            catch (Exception ex) when (ex is not FluentValidation.ValidationException && ex is not ArgumentException && ex is not KeyNotFoundException && ex is not InvalidOperationException)
            {
                _logger.LogError(ex, "Error al actualizar la relación cuenta-requisito.");
                return StatusCode(StatusCodes.Status500InternalServerError, new { success = false, message = "Ocurrió un error inesperado al procesar la solicitud. Inténtelo nuevamente." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Eliminar([FromBody] int cuentaRequisitosID)
        {
            try
            {
                var usuario = _currentUserService.GetUsername();
                var resultado = await _service.EliminarAsync(cuentaRequisitosID);
                if (resultado.CodigoResultado != 1)
                    return Conflict(new { success = false, message = resultado.Mensaje });

                return Json(new { success = true, message = resultado.Mensaje });
            }
            catch (ArgumentException)
            {
                return BadRequest(new { success = false, message = "El identificador de la relación no es válido." });
            }
            catch (Exception ex) when (ex is not FluentValidation.ValidationException && ex is not ArgumentException && ex is not KeyNotFoundException && ex is not InvalidOperationException)
            {
                _logger.LogError(ex, "Error al eliminar la relación cuenta-requisito.");
                return StatusCode(StatusCodes.Status500InternalServerError, new { success = false, message = "Ocurrió un error inesperado al procesar la solicitud. Inténtelo nuevamente." });
            }
        }
    }
}