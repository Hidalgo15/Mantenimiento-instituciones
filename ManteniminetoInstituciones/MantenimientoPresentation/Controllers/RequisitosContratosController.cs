using Mantenimiento.Core.Application.DTOs.RequisitoContrato;
using Mantenimiento.Core.Application.InterfaceServices;
using MantenimientoPresentation.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MantenimientoPresentation.Controllers
{
    [Authorize]
    [AutoValidateAntiforgeryToken]
    public class RequisitosContratosController : Controller
    {
        private readonly IRequisitoContratoService _service;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<RequisitosContratosController> _logger;
        private readonly bool _gridPaginationEnabled;

        public RequisitosContratosController(
            IRequisitoContratoService service,
            ICurrentUserService currentUserService,
            ILogger<RequisitosContratosController> logger,
            IConfiguration configuration)
        {
            _service = service;
            _currentUserService = currentUserService;
            _logger = logger;
            _gridPaginationEnabled = configuration.GetValue<bool>("GridPagination:Enabled");
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerGrid(string? busqueda = null, int page = 1, int pageSize = 10)
        {
            try
            {
                if (_gridPaginationEnabled)
                {
                    var resultado = await _service.ObtenerPaginadoAsync(busqueda, page, pageSize);
                    GridPaginationViewData.Set(this, resultado);
                    return PartialView("_RequisitosContratosGridPartial", resultado.Items.ToList());
                }

                var requisitos = await _service.ObtenerRequisitosAsync(busqueda);
                return PartialView("_RequisitosContratosGridPartial", requisitos);
            }
            catch (Exception ex) when (ex is not FluentValidation.ValidationException && ex is not ArgumentException && ex is not KeyNotFoundException && ex is not InvalidOperationException)
            {
                _logger.LogError(ex, "Error al obtener el grid de requisitos de contratos.");
                return StatusCode(500, "Error al cargar la lista de requisitos.");
            }
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            try
            {
                if (id <= 0)
                    return BadRequest(new { success = false, message = "Identificador no válido." });

                var requisito = await _service.ObtenerPorIdAsync(id);
                return Json(new { success = true, data = requisito });
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { success = false, message = "No se encontró el requisito de contrato solicitado." });
            }
            catch (Exception ex) when (ex is not FluentValidation.ValidationException && ex is not ArgumentException && ex is not KeyNotFoundException && ex is not InvalidOperationException)
            {
                _logger.LogError(ex, "Error al obtener el requisito con código {Id}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError, new { success = false, message = "Ocurrió un error inesperado al procesar la solicitud. Inténtelo nuevamente." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] CrearRequisitoContratoDto dto)
        {
            if (dto == null || !ModelState.IsValid)
            {
                var errores = string.Join(" ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));

                return BadRequest(new { success = false, message = string.IsNullOrEmpty(errores) ? "Datos del formulario inválidos." : errores });
            }

            try
            {
                dto.Usuario = _currentUserService.GetUsername();
                await _service.CrearRequisitoContratoAsync(dto);
                _logger.LogInformation("Requisito de contrato registrado exitosamente.");
                return StatusCode(StatusCodes.Status201Created, new { success = true, message = "Requisito registrado exitosamente." });
            }
            catch (Exception ex) when (ex is not FluentValidation.ValidationException && ex is not ArgumentException && ex is not KeyNotFoundException && ex is not InvalidOperationException)
            {
                _logger.LogError(ex, "Error al registrar el requisito de contrato.");
                return StatusCode(StatusCodes.Status500InternalServerError, new { success = false, message = "Ocurrió un error inesperado al procesar la solicitud. Inténtelo nuevamente." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Actualizar([FromBody] ActualizarRequisitoContratoDto dto)
        {
            if (dto == null || !ModelState.IsValid)
            {
                var errores = string.Join(" ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));

                return BadRequest(new { success = false, message = string.IsNullOrEmpty(errores) ? "Datos del formulario inválidos." : errores });
            }

            try
            {
                dto.Usuario = _currentUserService.GetUsername();
                await _service.ActualizarRequisitoContratoAsync(dto);
                _logger.LogInformation("Requisito de contrato con código {Id} actualizado exitosamente.", dto.CodigoRequisito);
                return Json(new { success = true, message = "Requisito actualizado exitosamente." });
            }
            catch (Exception ex) when (ex is not FluentValidation.ValidationException && ex is not ArgumentException && ex is not KeyNotFoundException && ex is not InvalidOperationException)
            {
                _logger.LogError(ex, "Error al actualizar el requisito con código {Id}.", dto.CodigoRequisito);
                return StatusCode(StatusCodes.Status500InternalServerError, new { success = false, message = "Ocurrió un error inesperado al procesar la solicitud. Inténtelo nuevamente." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Eliminar(int id)
        {
            if (id <= 0)
                return BadRequest(new { success = false, message = "Identificador no válido." });

            try
            {
                var usuario = _currentUserService.GetUsername();
                await _service.EliminarRequisitoContratoAsync(id);
                _logger.LogInformation("Requisito de contrato con código {Id} eliminado exitosamente.", id);
                return Json(new { success = true, message = "Requisito eliminado exitosamente." });
            }
            catch (ArgumentException)
            {
                return BadRequest(new { success = false, message = "El identificador del requisito no es válido." });
            }
            catch (Exception ex) when (ex is not FluentValidation.ValidationException && ex is not ArgumentException && ex is not KeyNotFoundException && ex is not InvalidOperationException)
            {
                _logger.LogError(ex, "Error al eliminar el requisito con código {Id}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError, new { success = false, message = "Ocurrió un error inesperado al procesar la solicitud. Inténtelo nuevamente." });
            }
        }
    }
}
