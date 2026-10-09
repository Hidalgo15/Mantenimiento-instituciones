using Mantenimiento.Core.Application.DTOs.Daf;
using Mantenimiento.Core.Application.DTOs.SubCapitulo;
using Mantenimiento.Core.Application.InterfaceServices;
using Microsoft.AspNetCore.Mvc;

namespace MantenimientoPresentation.Controllers
{
    public class DafController : Controller
    {
        private readonly IDafService _dafService;
        private readonly ISubCapituloService _subCapituloService;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<DafController> _logger;

        public DafController(
            IDafService dafService,
            ISubCapituloService subCapituloService,
            ICurrentUserService currentUserService,
            ILogger<DafController> logger)
        {
            _dafService = dafService;
            _subCapituloService = subCapituloService;
            _currentUserService = currentUserService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var subcapitulos = await _subCapituloService.ObtenerSubCapitulosAsync(null);
                ViewBag.SubCapitulos = subcapitulos;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cargar la lista inicial de subcapítulos para DAF.");
                ViewBag.SubCapitulos = new List<SubCapituloDto>();
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerGrid(string? codigo = null, string? buscar = null, int? idSubCapitulo = null)
        {
            try
            {
                IEnumerable<DafDto> resultados;

                if (!string.IsNullOrWhiteSpace(buscar))
                {
                    resultados = await _dafService.ObtenerAsync();
                    resultados = resultados.Where(d => d.Daf != null && d.Daf.Contains(buscar, StringComparison.OrdinalIgnoreCase));
                }
                else if (!string.IsNullOrWhiteSpace(codigo))
                {
                    resultados = await _dafService.ObtenerPorCodigoAsync(codigo);
                }
                else if (idSubCapitulo.HasValue && idSubCapitulo > 0)
                {
                    resultados = await _dafService.ObtenerPorSubCapitulo(idSubCapitulo.Value);
                }
                else
                {
                    resultados = Enumerable.Empty<DafDto>();
                }

                return PartialView("_DafGridPartial", resultados);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cargar el grid de registros DAF.");
                return StatusCode(500, "Error al recuperar los registros DAF.");
            }
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            try
            {
                if (id <= 0)
                    return Json(new { success = false, message = "Identificador no válido." });

                var daf = await _dafService.ObtenerPorIdAsync(id);
                if (daf == null)
                    return Json(new { success = false, message = "Registro DAF no encontrado." });

                return Json(new { success = true, data = daf });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el registro DAF con Id {Id}", id);
                return Json(new { success = false, message = "No se pudo recuperar el registro DAF." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] CrearDafDTO dto)
        {
            if (dto == null || !ModelState.IsValid)
                return Json(new { success = false, message = "Datos del formulario inválidos." });

            try
            {
                dto.Usuario = _currentUserService.GetUsername();
                var resultado = await _dafService.CrearAsync(dto);

                _logger.LogInformation("Registro DAF creado exitosamente por {Usuario}.", dto.Usuario);
                return Json(new { success = true, data = resultado, message = "DAF creado exitosamente." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear el registro DAF.");
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Actualizar([FromBody] ActualizarDafDTO dto)
        {
            if (dto == null || !ModelState.IsValid)
                return Json(new { success = false, message = "Datos del formulario inválidos." });

            try
            {
                dto.Usuario = _currentUserService.GetUsername();
                var resultado = await _dafService.ActualizarAsync(dto);

                _logger.LogInformation("Registro DAF con Id {Id} actualizado por {Usuario}.", dto.Id, dto.Usuario);
                return Json(new { success = true, data = resultado, message = "DAF actualizado exitosamente." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el registro DAF con Id {Id}.", dto.Id);
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
                await _dafService.EliminarAsync(id);

                _logger.LogInformation("Registro DAF con Id {Id} eliminado por {Usuario}.", id, usuario);
                return Json(new { success = true, message = "DAF eliminado correctamente." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el registro DAF con Id {Id}.", id);
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}