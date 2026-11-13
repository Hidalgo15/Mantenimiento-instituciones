using Mantenimiento.Core.Application.DTOs.ProveedoresInhabilitados;
using Mantenimiento.Core.Application.InterfaceServices;
using MantenimientoPresentation.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MantenimientoPresentation.Controllers
{
    [Authorize]
    [AutoValidateAntiforgeryToken]
    public class ProveedoresInhabilitadosController : Controller
    {
        private readonly IProveedorInhabilitadoService _service;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<ProveedoresInhabilitadosController> _logger;
        private readonly bool _gridPaginationEnabled;

        public ProveedoresInhabilitadosController(
            IProveedorInhabilitadoService service,
            ICurrentUserService currentUserService,
            ILogger<ProveedoresInhabilitadosController> logger,
            IConfiguration configuration)
        {
            _service = service;
            _currentUserService = currentUserService;
            _logger = logger;
            _gridPaginationEnabled = configuration.GetValue<bool>("GridPagination:Enabled");
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var opciones = await _service.ObtenerOpcionesAsync();
                ViewBag.Generos = opciones.Generos;
                ViewBag.EstadosRpe = opciones.EstadosRpe;
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cargar las opciones de género y estado RPE.");
                return StatusCode(StatusCodes.Status500InternalServerError, "No se pudo cargar el mantenimiento de proveedores inhabilitados.");
            }
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerGrid(string? filtroTexto, int page = 1, int pageSize = 10)
        {
            if (filtroTexto?.Length > 255)
                return BadRequest("El texto de búsqueda no puede exceder 255 caracteres.");

            try
            {
                if (_gridPaginationEnabled)
                {
                    var resultado = await _service.ObtenerPaginadoAsync(filtroTexto, page, pageSize);
                    GridPaginationViewData.Set(this, resultado);
                    return PartialView("_ProveedoresInhabilitadosGridPartial", resultado.Items);
                }

                var registros = await _service.ObtenerAsync(filtroTexto);
                return PartialView("_ProveedoresInhabilitadosGridPartial", registros);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cargar el listado de proveedores inhabilitados.");
                return StatusCode(StatusCodes.Status500InternalServerError, "No se pudo cargar el listado de proveedores inhabilitados.");
            }
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            if (id <= 0)
                return BadRequest(new { success = false, message = "Identificador no válido." });

            try
            {
                var proveedor = await _service.ObtenerPorIdAsync(id);
                return proveedor == null
                    ? NotFound(new { success = false, message = "No se encontró el proveedor solicitado." })
                    : Json(new { success = true, data = proveedor });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el proveedor inhabilitado con Id {Id}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError, new { success = false, message = "No se pudo recuperar el proveedor." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] CrearProveedorInhabilitadoDto? dto)
        {
            if (dto == null || !ModelState.IsValid)
                return BadRequest(new { success = false, message = "Datos del formulario inválidos." });

            try
            {
                dto.Usuario = _currentUserService.GetUsername();
                var id = await _service.CrearAsync(dto);
                _logger.LogInformation("Proveedor inhabilitado {Id} registrado por {Usuario}.", id, dto.Usuario);
                return StatusCode(StatusCodes.Status201Created, new { success = true, id, message = "Proveedor inhabilitado registrado exitosamente." });
            }
            catch (FluentValidation.ValidationException ex)
            {
                return BadRequest(new { success = false, message = string.Join(" ", ex.Errors.Select(error => error.ErrorMessage)) });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al registrar un proveedor inhabilitado.");
                return StatusCode(StatusCodes.Status500InternalServerError, new { success = false, message = "No se pudo registrar el proveedor inhabilitado." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Actualizar([FromBody] ActualizarProveedorInhabilitadoDto? dto)
        {
            if (dto == null || !ModelState.IsValid)
                return BadRequest(new { success = false, message = "Datos del formulario inválidos." });

            try
            {
                dto.Usuario = _currentUserService.GetUsername();
                await _service.ActualizarAsync(dto);
                _logger.LogInformation("Proveedor inhabilitado {Id} actualizado por {Usuario}.", dto.Id, dto.Usuario);
                return Json(new { success = true, message = "Proveedor inhabilitado actualizado exitosamente." });
            }
            catch (FluentValidation.ValidationException ex)
            {
                return BadRequest(new { success = false, message = string.Join(" ", ex.Errors.Select(error => error.ErrorMessage)) });
            }
            catch (ArgumentOutOfRangeException)
            {
                return BadRequest(new { success = false, message = "Identificador no válido." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el proveedor inhabilitado con Id {Id}.", dto.Id);
                return StatusCode(StatusCodes.Status500InternalServerError, new { success = false, message = "No se pudo actualizar el proveedor inhabilitado." });
            }
        }

        // [HttpPost]
        // public async Task<IActionResult> Eliminar(int id)
        // {
        //     if (id <= 0)
        //         return BadRequest(new { success = false, message = "Identificador no válido." });
        //
        //     try
        //     {
        //         var usuario = _currentUserService.GetUsername();
        //         await _service.EliminarAsync(id, usuario);
        //         _logger.LogInformation("Proveedor inhabilitado {Id} eliminado por {Usuario}.", id, usuario);
        //         return Json(new { success = true, message = "Proveedor inhabilitado eliminado exitosamente." });
        //     }
        //     catch (Exception ex)
        //     {
        //         _logger.LogError(ex, "Error al eliminar el proveedor inhabilitado con Id {Id}.", id);
        //         return StatusCode(StatusCodes.Status500InternalServerError, new { success = false, message = "No se pudo eliminar el proveedor inhabilitado." });
        //     }
        // }
    }
}
