using Mantenimiento.Core.Application.DTOs.Daf;
using Mantenimiento.Core.Application.InterfaceServices;
using Microsoft.AspNetCore.Mvc;

namespace MantenimientoPresentation.Controllers
{
    public class DafController : Controller
    {
        private readonly IDafService _dafService;
        private readonly ISubCapituloService _subCapituloService;

        public DafController(IDafService dafService, ISubCapituloService subCapituloService)
        {
            _dafService = dafService;
            _subCapituloService = subCapituloService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // Cargar la lista inicial de subcapítulos para llenar el dropdown de selección/filtro
            var subcapitulos = await _subCapituloService.ObtenerSubCapitulosAsync(null);
            ViewBag.SubCapitulos = subcapitulos;
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerGrid(string? codigo = null, string? buscar = null, int? idSubCapitulo = null)
        {
            IEnumerable<DafDto> resultados;

            if (!string.IsNullOrWhiteSpace(buscar))
            {
                // Búsqueda por coincidencia de texto en el nombre del DAF
                resultados = await _dafService.ObtenerAsync();
                resultados = resultados.Where(d => d.Daf != null && d.Daf.Contains(buscar, StringComparison.OrdinalIgnoreCase));
            }
            else if (!string.IsNullOrWhiteSpace(codigo))
            {
                // Búsqueda por código exacto o general
                resultados = await _dafService.ObtenerPorCodigoAsync(codigo);
            }
            else if (idSubCapitulo.HasValue && idSubCapitulo > 0)
            {
                // Búsqueda por Subcapítulo Padre
                resultados = await _dafService.ObtenerPorSubCapitulo(idSubCapitulo.Value);
            }
            else
            {
                resultados = Enumerable.Empty<DafDto>();
            }

            return PartialView("_DafGridPartial", resultados);
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            try
            {
                var daf = await _dafService.ObtenerPorIdAsync(id);
                if (daf == null)
                {
                    return Json(new { success = false, message = "Registro DAF no encontrado." });
                }

                return Json(new { success = true, data = daf });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] CrearDafDTO dto)
        {
            try
            {
                var resultado = await _dafService.CrearAsync(dto);
                return Json(new { success = true, data = resultado, message = "DAF creado exitosamente." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Actualizar([FromBody] ActualizarDafDTO dto)
        {
            try
            {
                var resultado = await _dafService.ActualizarAsync(dto);
                return Json(new { success = true, data = resultado, message = "DAF actualizado exitosamente." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Eliminar(int id)
        {
            try
            {
                await _dafService.EliminarAsync(id);
                return Json(new { success = true, message = "DAF eliminado correctamente." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}
