using Mantenimiento.Core.Application.DTOs.Capitulo;
using Mantenimiento.Core.Application.InterfaceServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace MantenimientoPresentation.Controllers
{
    //[Authorize]
    public class CapituloController : Controller
    {
        private readonly ICapituloService _service;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<CapituloController> _logger;

        public CapituloController(
            ICapituloService service,
            ICurrentUserService currentUserService,
            ILogger<CapituloController> logger)
        {
            _service = service;
            _currentUserService = currentUserService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var capitulos = await _service.ObtenerCapitulosAsync(null);
                ViewBag.Capitulos = capitulos;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cargar la vista principal de Capítulos.");
                ViewBag.Capitulos = new List<CapituloDto>();
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerGrid(string? codigo)
        {
            try
            {
                var capitulos = await _service.ObtenerCapitulosAsync(codigo);
                return PartialView("_CapitulosGridPartial", capitulos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el grid de capítulos.");
                return StatusCode(500, "Ocurrió un error al cargar la información. Intente nuevamente.");
            }
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerOpciones()
        {
            try
            {
                var capitulos = await _service.ObtenerCapitulosAsync();
                return Json(new { success = true, data = capitulos });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar las opciones de capítulos.");
                return StatusCode(500, new { success = false, message = "No se pudo actualizar la lista de capítulos." });
            }
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            try
            {
                if (id <= 0)
                    return Json(new { success = false, message = "Identificador no válido." });

                var capitulo = await _service.ObtenerPorIdAsync(id);
                return Json(new { success = true, data = capitulo });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el capítulo con Id {Id}", id);
                return Json(new { success = false, message = "No se pudo recuperar la información del registro." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] CrearCapituloDto dto)
        {
            if (dto == null || !ModelState.IsValid)
                return Json(new { success = false, message = "Datos del formulario inválidos." });

            try
            {
                dto.Usuario = _currentUserService.GetUsername();
                await _service.CrearCapituloAsync(dto);

                _logger.LogInformation("Capítulo registrado exitosamente por {Usuario}.", dto.Usuario);
                return Json(new { success = true, message = "Capítulo registrado exitosamente." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear el capítulo.");
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Actualizar([FromBody] ActualizarCapituloDto dto)
        {
            if (dto == null || !ModelState.IsValid)
                return Json(new { success = false, message = "Datos del formulario inválidos." });

            try
            {
                dto.Usuario = _currentUserService.GetUsername();
                await _service.ActualizarCapituloAsync(dto);

                _logger.LogInformation("Capítulo con Id {Id} actualizado por {Usuario}.", dto.Id, dto.Usuario);
                return Json(new { success = true, message = "Capítulo actualizado exitosamente." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el capítulo con Id {Id}.", dto.Id);
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Eliminar(int id)
        {
            if (id <= 0)
                return Json(new { success = false, message = "Identificador no válido." });

            try
            {
                var usuario = _currentUserService.GetUsername();
                await _service.EliminarCapituloAsync(id);

                _logger.LogInformation("Capítulo con Id {Id} eliminado por {Usuario}.", id, usuario);
                return Json(new { success = true, message = "Capítulo eliminado exitosamente." });
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "No se pudo eliminar el capítulo con Id {Id} porque tiene registros dependientes.", id);
                return Json(new { success = false, message = ex.Message });
            }
            catch (SqlException ex) when (ex.Number == 547)
            {
                _logger.LogWarning(ex, "La eliminación del capítulo con Id {Id} fue bloqueada por registros dependientes.", id);
                return Json(new
                {
                    success = false,
                    message = "No se puede eliminar el capítulo porque tiene subcapítulos asociados. Elimine primero sus subcapítulos."
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el capítulo con Id {Id}.", id);
                return Json(new { success = false, message = "Ocurrió un error al eliminar el capítulo." });
            }
        }
    }
}