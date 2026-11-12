using Mantenimiento.Core.Application.DTOs.Cuenta.CtCuentasDTO;
using Mantenimiento.Core.Application.InterfaceServices;
using MantenimientoPresentation.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MantenimientoPresentation.Controllers
{
    [Authorize]
    [AutoValidateAntiforgeryToken]
    public class CuentaCtController : Controller
    {
        private readonly ICuentaCtService _cuentaCtService;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<CuentaCtController> _logger;
        private readonly bool _gridPaginationEnabled;

        public CuentaCtController(
            ICuentaCtService cuentaCtService,
            ICurrentUserService currentUserService,
            ILogger<CuentaCtController> logger,
            IConfiguration configuration)
        {
            _cuentaCtService = cuentaCtService;
            _currentUserService = currentUserService;
            _logger = logger;
            _gridPaginationEnabled = configuration.GetValue<bool>("GridPagination:Enabled");
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerGrid(string? busqueda = null, int page = 1, int pageSize = 10)
        {
            if (_gridPaginationEnabled)
            {
                var resultado = await _cuentaCtService.ObtenerPaginadoAsync(busqueda, page, pageSize);
                GridPaginationViewData.Set(this, resultado);
                return PartialView("_CuentasCtGridPartial", resultado.Items);
            }

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
                    return NotFound(new { success = false, message = "La cuenta no fue encontrada." });

                return Json(new { success = true, data = cuenta });
            }
            catch (Exception ex) when (ex is not FluentValidation.ValidationException && ex is not ArgumentException && ex is not KeyNotFoundException && ex is not InvalidOperationException)
            {
                _logger.LogError(ex, "Error al obtener la cuenta contable con Id {Id}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError, new { success = false, message = "Ocurrió un error inesperado al procesar la solicitud. Inténtelo nuevamente." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] CrearCuentaCtDto dto)
        {
            try
            {
                dto.Usuario = _currentUserService.GetUsername();
                await _cuentaCtService.CrearCuentaAsync(dto);
                return StatusCode(StatusCodes.Status201Created, new { success = true, message = "Cuenta creada exitosamente." });
            }
            catch (Exception ex) when (ex is not FluentValidation.ValidationException && ex is not ArgumentException && ex is not KeyNotFoundException && ex is not InvalidOperationException)
            {
                _logger.LogError(ex, "Error al crear la cuenta contable.");
                return StatusCode(StatusCodes.Status500InternalServerError, new { success = false, message = "Ocurrió un error inesperado al procesar la solicitud. Inténtelo nuevamente." });
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
            catch (Exception ex) when (ex is not FluentValidation.ValidationException && ex is not ArgumentException && ex is not KeyNotFoundException && ex is not InvalidOperationException)
            {
                _logger.LogError(ex, "Error al actualizar la cuenta contable con Id {Id}.", dto.CuentaID);
                return StatusCode(StatusCodes.Status500InternalServerError, new { success = false, message = "Ocurrió un error inesperado al procesar la solicitud. Inténtelo nuevamente." });
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
            catch (ArgumentException)
            {
                return BadRequest(new { success = false, message = "El identificador de la cuenta no es válido." });
            }
            catch (Exception ex) when (ex is not FluentValidation.ValidationException && ex is not ArgumentException && ex is not KeyNotFoundException && ex is not InvalidOperationException)
            {
                _logger.LogError(ex, "Error al eliminar la cuenta contable con Id {Id}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError, new { success = false, message = "Ocurrió un error inesperado al procesar la solicitud. Inténtelo nuevamente." });
            }
        }
    }
}
