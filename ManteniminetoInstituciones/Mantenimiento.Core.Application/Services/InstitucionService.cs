using Mantenimiento.Core.Application.DTOs.Institucion;
using Mantenimiento.Core.Application.InterfaceServices;
using Mantenimiento.Core.Domain.RepositoryInterfaces;

namespace Mantenimiento.Core.Application.Services
{
    public class InstitucionService : IInstitucionService
    {
        private readonly IInstitucionRepository _repository;

        public InstitucionService(IInstitucionRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<InstitucionSeleccionDto>> ObtenerInstitucionesSelectorAsync()
        {
            var instituciones = await _repository.ObtenerInstitucionesAsync();
            return instituciones
                .Where(institucion => institucion.Estatus)
                .Select(institucion => new InstitucionSeleccionDto
                {
                    IdInstitucion = institucion.IdInstitucion,
                    Estructura = institucion.Estructura,
                    UnidadEjecutora = institucion.UnidadEjecutora
                })
                .DistinctBy(institucion => institucion.Estructura)
                .OrderBy(x => x.UnidadEjecutora) // <-- ORDENAR ALFABÉTICAMENTE POR NOMBRE
                .ToList();
        }
    }
}
