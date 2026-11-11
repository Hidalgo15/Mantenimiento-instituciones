using Mantenimiento.Core.Application.DTOs.Cuenta.DboCuentasDTO;
using Mantenimiento.Core.Application.InterfaceServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MantenimientoPresentation.Controllers
{
    [Authorize]
    [AutoValidateAntiforgeryToken]
    [Route("CuentaMapeo/[action]/{id?}")]
    public class CuentaMapeoController : Controller
    {
        private readonly ICuentaMapeoService _cuentaMapeoService;
        private readonly ITipoTramitePagoService _tipoTramitePagoService; // Servicio que lista dbo.tipo_tramite_pago
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<CuentaMapeoController> _logger;

        public CuentaMapeoController(
            ICuentaMapeoService cuentaMapeoService,
            ITipoTramitePagoService tipoTramitePagoService,
            ICurrentUserService currentUserService,
            ILogger<CuentaMapeoController> logger)
        {
            _cuentaMapeoService = cuentaMapeoService;
            _tipoTramitePagoService = tipoTramitePagoService;
            _currentUserService = currentUserService;
            _logger = logger;
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
        public async Task<IActionResult> ObtenerGrid(int? tipoTramite = null, string? busqueda = null)
        {
            var cuentas = await _cuentaMapeoService.ObtenerCuentasAsync(tipoTramite, busqueda);
            return PartialView("_CuentasGridPartial", cuentas);
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            try
            {
                if (id <= 0)
                    return BadRequest(new { success = false, message = "Identificador no válido." });

                var cuenta = await _cuentaMapeoService.ObtenerPorIdAsync(id);
                if (cuenta == null)
                    return NotFound(new { success = false, message = "La cuenta no fue encontrada." });

                return Json(new { success = true, data = cuenta });
            }
            catch (Exception ex) when (ex is not FluentValidation.ValidationException && ex is not ArgumentException && ex is not KeyNotFoundException && ex is not InvalidOperationException)
            {
                _logger.LogError(ex, "Error al obtener la cuenta mapeada con Id {Id}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError, new { success = false, message = "Ocurrió un error inesperado al procesar la solicitud. Inténtelo nuevamente." });
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

                return BadRequest(new { success = false, message = string.IsNullOrEmpty(errores) ? "Datos del formulario inválidos." : errores });
            }

            try
            {
                dto.Usuario = _currentUserService.GetUsername();
                await _cuentaMapeoService.CrearCuentaAsync(dto);
                return StatusCode(StatusCodes.Status201Created, new { success = true, message = "Cuenta registrada exitosamente." });
            }
            catch (Exception ex) when (ex is not FluentValidation.ValidationException && ex is not ArgumentException && ex is not KeyNotFoundException && ex is not InvalidOperationException)
            {
                _logger.LogError(ex, "Error al crear una cuenta mapeada.");
                return StatusCode(StatusCodes.Status500InternalServerError, new { success = false, message = "Ocurrió un error inesperado al procesar la solicitud. Inténtelo nuevamente." });
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

                return BadRequest(new { success = false, message = string.IsNullOrEmpty(errores) ? "Datos del formulario inválidos." : errores });
            }

            try
            {
                dto.Usuario = _currentUserService.GetUsername();
                await _cuentaMapeoService.ActualizarCuentaAsync(dto);
                return Json(new { success = true, message = "Cuenta actualizada exitosamente." });
            }
            catch (Exception ex) when (ex is not FluentValidation.ValidationException && ex is not ArgumentException && ex is not KeyNotFoundException && ex is not InvalidOperationException)
            {
                _logger.LogError(ex, "Error al actualizar la cuenta mapeada.");
                return StatusCode(StatusCodes.Status500InternalServerError, new { success = false, message = "Ocurrió un error inesperado al procesar la solicitud. Inténtelo nuevamente." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Eliminar(int id)
        {
            try
            {
                var usuario = _currentUserService.GetUsername();
                await _cuentaMapeoService.EliminarCuentaAsync(id);
                return Json(new { success = true, message = "Cuenta eliminada exitosamente." });
            }
            catch (ArgumentException)
            {
                return BadRequest(new { success = false, message = "El identificador de la cuenta no es válido." });
            }
            catch (Exception ex) when (ex is not FluentValidation.ValidationException && ex is not ArgumentException && ex is not KeyNotFoundException && ex is not InvalidOperationException)
            {
                _logger.LogError(ex, "Error al eliminar la cuenta mapeada con Id {Id}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError, new { success = false, message = "Ocurrió un error inesperado al procesar la solicitud. Inténtelo nuevamente." });
            }
        }
    }
}