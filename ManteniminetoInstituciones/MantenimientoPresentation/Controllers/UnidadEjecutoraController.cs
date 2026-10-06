using Mantenimiento.Core.Application.DTOs.Daf;
using Mantenimiento.Core.Application.DTOs.UnidadEjecutora;
using Mantenimiento.Core.Application.InterfaceServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace MantenimientoPresentation.Controllers
{
    public class UnidadEjecutoraController : Controller
    {
        private readonly IUnidadEjecutoraService _service;
        private readonly IDafService _dafService;
        private readonly ICategoriaTramiteService _categoriaService;
        private readonly ILogger<UnidadEjecutoraController> _logger;

        public UnidadEjecutoraController(
            IUnidadEjecutoraService service,
            IDafService dafService,
            ICategoriaTramiteService categoriaService,
            ILogger<UnidadEjecutoraController> logger)
        {
            _service = service;
            _dafService = dafService;
            _categoriaService = categoriaService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                ViewBag.Dafs = await _dafService.ObtenerAsync();
                ViewBag.Categorias = await _categoriaService.ObtenerTodasAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cargar catálogos para Unidad Ejecutora.");
                ViewBag.Dafs = new List<DafDto>();
                ViewBag.Categorias = new List<CategoriaTramiteDto>();
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerGrid(int? id, int? idDaf)
        {
            try
            {
                if (id.HasValue && id.Value > 0)
                {
                    var item = await _service.ObtenerPorIdAsync(id.Value);
                    var listaSingular = item != null ? new List<UnidadEjecutoraDto> { item } : new List<UnidadEjecutoraDto>();
                    return PartialView("_UnidadesEjecutorasGridPartial", listaSingular);
                }

                var lista = await _service.ObtenerAsync(idDaf: idDaf);
                return PartialView("_UnidadesEjecutorasGridPartial", lista);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el grid de Unidades Ejecutoras. Parámetros - Id: {Id}, IdDaf: {IdDaf}", id, idDaf);

                // Mensaje genérico y seguro para el usuario final
                return StatusCode(500, "Ha ocurrido un error al cargar la información. Intente nuevamente.");
            }
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            try
            {
                if (id <= 0)
                    return Json(new { success = false, message = "Identificador no válido." });

                var item = await _service.ObtenerPorIdAsync(id);
                if (item == null)
                    return Json(new { success = false, message = "No se encontró el registro solicitado." });

                return Json(new { success = true, data = item });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la Unidad Ejecutora con Id {Id}", id);
                return Json(new { success = false, message = "No se pudo recuperar la información del registro." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] CrearUnidadEjecutoraDto dto)
        {
            if (dto == null || !ModelState.IsValid)
            {
                var error = ModelState.Values.SelectMany(v => v.Errors)
                                             .Select(e => e.ErrorMessage)
                                             .FirstOrDefault() ?? "Datos del formulario inválidos.";
                return Json(new { success = false, message = error });
            }

            try
            {
                var usuarioActual = !string.IsNullOrWhiteSpace(User.Identity?.Name)
                    ? User.Identity.Name
                    : (Environment.UserName ?? "SISTEMA");

                dto.Usuario = usuarioActual;
                await _service.CrearAsync(dto);

                _logger.LogInformation("Unidad Ejecutora {Codigo} creada exitosamente por el usuario {Usuario}.", dto.CodigoUnidadEjecutora, dto.Usuario);
                return Json(new { success = true, message = "Unidad Ejecutora registrada con éxito." });
            }
            catch (ArgumentException ex)
            {
                // Advertencia de validación de negocio (no requiere log de excepción técnica completa)
                _logger.LogWarning(ex, "Validación fallida al crear Unidad Ejecutora. Código: {Codigo}", dto.CodigoUnidadEjecutora);
                return Json(new { success = false, message = ex.Message });
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Error de base de datos al intentar crear Unidad Ejecutora {Codigo}", dto.CodigoUnidadEjecutora);
                return Json(new { success = false, message = "Ocurrió un problema de base de datos al guardar los datos." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error no controlado al crear Unidad Ejecutora {Codigo}", dto.CodigoUnidadEjecutora);
                return Json(new { success = false, message = "Ocurrió un error inesperado en el servidor." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Actualizar([FromBody] ActualizarUnidadEjecutoraDto dto)
        {
            if (dto == null || dto.Id <= 0)
                return Json(new { success = false, message = "Registro no válido para actualización." });

            if (!ModelState.IsValid)
            {
                var error = ModelState.Values.SelectMany(v => v.Errors)
                                             .Select(e => e.ErrorMessage)
                                             .FirstOrDefault() ?? "Datos del formulario inválidos.";
                return Json(new { success = false, message = error });
            }

            try
            {
                var usuarioActual = !string.IsNullOrWhiteSpace(User.Identity?.Name)
                    ? User.Identity.Name
                    : (Environment.UserName ?? "SISTEMA");

                dto.Usuario = usuarioActual;
                await _service.ActualizarAsync(dto);

                _logger.LogInformation("Unidad Ejecutora con Id {Id} actualizada exitosamente por el usuario {Usuario}.", dto.Id, dto.Usuario);
                return Json(new { success = true, message = "Unidad Ejecutora actualizada con éxito." });
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al actualizar Unidad Ejecutora con Id {Id}", dto.Id);
                return Json(new { success = false, message = ex.Message });
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Error de base de datos al actualizar Unidad Ejecutora con Id {Id}", dto.Id);
                return Json(new { success = false, message = "Ocurrió un problema de base de datos al actualizar los datos." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error no controlado al actualizar Unidad Ejecutora con Id {Id}", dto.Id);
                return Json(new { success = false, message = "Ocurrió un error inesperado en el servidor." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Eliminar(int id)
        {
            if (id <= 0)
                return Json(new { success = false, message = "Identificador no válido." });

            try
            {
                var usuarioActual = !string.IsNullOrWhiteSpace(User.Identity?.Name)
                    ? User.Identity.Name
                    : (Environment.UserName ?? "SISTEMA");

                await _service.EliminarAsync(id, usuarioActual);

                _logger.LogInformation("Unidad Ejecutora con Id {Id} desactivada correctamente por {Usuario}.", id, usuarioActual);
                return Json(new { success = true, message = "Unidad Ejecutora desactivada correctamente." });
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al intentar eliminar la Unidad Ejecutora con Id {Id}", id);
                return Json(new { success = false, message = ex.Message });
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Error de base de datos al eliminar Unidad Ejecutora con Id {Id}", id);
                return Json(new { success = false, message = "No se pudo realizar la desactivación en la base de datos." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error no controlado al eliminar Unidad Ejecutora con Id {Id}", id);
                return Json(new { success = false, message = "Ocurrió un error inesperado en el servidor." });
            }
        }
    }
}