using Mantenimiento.Core.Application.DTOs.Requisito;
using Mantenimiento.Core.Application.InterfaceServices;
using MantenimientoPresentation.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MantenimientoPresentation.Controllers
{
    [Authorize]
    [AutoValidateAntiforgeryToken]
    public class RequisitoController : Controller
    {
        private readonly IRequisitoService _service;
        private readonly ILogger<RequisitoController> _logger;
        private readonly ICurrentUserService _currentUserService;
        private readonly bool _gridPaginationEnabled;

        public RequisitoController(IRequisitoService service, ILogger<RequisitoController> logger, ICurrentUserService currentUserService, IConfiguration configuration)
        {
            _service = service;
            _logger = logger;
            _currentUserService = currentUserService;
            _gridPaginationEnabled = configuration.GetValue<bool>("GridPagination:Enabled");
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerGrid(string? busqueda, int page = 1, int pageSize = 10)
        {
            try
            {
                if (_gridPaginationEnabled)
                {
                    var resultado = await _service.ObtenerPaginadoAsync(busqueda, page, pageSize);
                    GridPaginationViewData.Set(this, resultado);
                    ViewData["Busqueda"] = busqueda;
                    return PartialView("_RequisitoGridPartial", resultado.Items.ToList());
                }

                var data = await _service.ObtenerRequisitosAsync(busqueda);
                ViewData["Busqueda"] = busqueda;
                return PartialView("_RequisitoGridPartial", data);
            }
            catch (Exception ex) when (ex is not FluentValidation.ValidationException && ex is not ArgumentException && ex is not KeyNotFoundException && ex is not InvalidOperationException)
            {
                _logger.LogError(ex, "Error al obtener la lista de requisitos.");
                return StatusCode(500, "Ocurrió un error al cargar los datos. Intente nuevamente.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Guardar([FromBody] CrearRequisitoDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { success = false, message = "Datos inválidos." });

            try
            {
                dto.Usuario = _currentUserService.GetUsername();
                var resultado = await _service.CrearAsync(dto);
                if (resultado.CodigoResultado != 1)
                    return Conflict(new { success = false, message = resultado.Mensaje });

                return StatusCode(StatusCodes.Status201Created, new { success = true, message = resultado.Mensaje });
            }
            catch (Exception ex) when (ex is not FluentValidation.ValidationException && ex is not ArgumentException && ex is not KeyNotFoundException && ex is not InvalidOperationException)
            {
                _logger.LogError(ex, "Error al crear el requisito.");
                return StatusCode(StatusCodes.Status500InternalServerError, new { success = false, message = "Ocurrió un error inesperado al procesar la solicitud. Inténtelo nuevamente." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Actualizar([FromBody] ActualizarRequisitoDto dto)
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
                _logger.LogError(ex, "Error al actualizar el requisito.");
                return StatusCode(StatusCodes.Status500InternalServerError, new { success = false, message = "Ocurrió un error inesperado al procesar la solicitud. Inténtelo nuevamente." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Eliminar([FromBody] int requisitosID)
        {
            try
            {
                var usuario = _currentUserService.GetUsername();
                var resultado = await _service.EliminarAsync(requisitosID);
                if (resultado.CodigoResultado != 1)
                    return Conflict(new { success = false, message = resultado.Mensaje });

                return Json(new { success = true, message = resultado.Mensaje });
            }
            catch (ArgumentException)
            {
                return BadRequest(new { success = false, message = "El identificador del requisito no es válido." });
            }
            catch (Exception ex) when (ex is not FluentValidation.ValidationException && ex is not ArgumentException && ex is not KeyNotFoundException && ex is not InvalidOperationException)
            {
                _logger.LogError(ex, "Error al eliminar el requisito.");
                return StatusCode(StatusCodes.Status500InternalServerError, new { success = false, message = "Ocurrió un error inesperado al procesar la solicitud. Inténtelo nuevamente." });
            }
        }
    }
}