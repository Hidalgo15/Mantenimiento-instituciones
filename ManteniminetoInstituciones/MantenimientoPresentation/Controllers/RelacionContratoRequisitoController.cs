using Mantenimiento.Core.Application.DTOs.RelacionContratoRequisito;
using Mantenimiento.Core.Application.InterfaceServices;
using MantenimientoPresentation.Services;
using Microsoft.AspNetCore.Mvc;

namespace MantenimientoPresentation.Controllers
{
    public class RelacionContratoRequisitoController : Controller
    {
        private readonly IRelacionContratoRequisitoService _service;
        private readonly ILogger<RelacionContratoRequisitoController> _logger;
        private readonly ICurrentUserService _currentUserService;

        public RelacionContratoRequisitoController(IRelacionContratoRequisitoService service, ILogger<RelacionContratoRequisitoController> logger, ICurrentUserService currentUserService)
        {
            _service = service;
            _logger = logger;
            _currentUserService = currentUserService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                ViewBag.Clasificaciones = await _service.ObtenerCatalogoClasificacionAsync();
                ViewBag.Requisitos = await _service.ObtenerCatalogoRequisitosAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cargar los catálogos de contrato y requisitos.");
                ViewBag.Clasificaciones = new List<CatalogoClasificacionContratoDto>();
                ViewBag.Requisitos = new List<CatalogoRequisitoContratoDto>();
            }

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerGrid(int? codigoClasificacion, int? codigoRequisito)
        {
            try
            {
                var data = await _service.ObtenerAsync(codigoClasificacion, codigoRequisito);
                return PartialView("_RelacionContratoRequisitoGridPartial", data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el grid de relaciones contrato-requisito.");
                return StatusCode(500, "Ocurrió un error al cargar los datos.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Guardar([FromBody] CrearRelacionContratoRequisitoDto dto)
        {
            try
            {
                dto.Usuario = _currentUserService.GetUsername();
                var resultado = await _service.AsignarAsync(dto);
                return Json(new { success = resultado.CodigoResultado == 1, message = resultado.Mensaje });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al guardar la relación contrato-requisito.");
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Actualizar([FromBody] ActualizarRelacionContratoRequisitoDto dto)
        {
            try
            {
                dto.Usuario = _currentUserService.GetUsername();
                var resultado = await _service.ActualizarAsync(dto);
                return Json(new { success = resultado.CodigoResultado == 1, message = resultado.Mensaje });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar la relación contrato-requisito.");
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Eliminar([FromBody] int id)
        {
            try
            {
                var usuario = _currentUserService.GetUsername();
                var resultado = await _service.EliminarAsync(id);
                return Json(new { success = resultado.CodigoResultado == 1, message = resultado.Mensaje });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar la relación contrato-requisito.");
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}