using Mantenimiento.Core.Application.DTOs.Institucion;
using Mantenimiento.Core.Application.InterfaceServices;
using Mantenimiento.Core.Application.Mappings;
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

        /// <summary>
        /// Obtiene las instituciones activas, convertidas a InstitucionSeleccionDto, únicas por Estructura y ordenadas
        /// por UnidadEjecutora.
        /// </summary>
        /// <remarks>Consulta _repository.ObtenerInstitucionesAsync, filtra por Estatus, mapea con
        /// InstitucionMapper.ToDto, aplica DistinctBy por Estructura y ordena por UnidadEjecutora.</remarks>
        /// <returns>Una tarea que representa la operación asíncrona y devuelve una lista de InstitucionSeleccionDto activos
        /// (Estatus = true), deduplicados por Estructura y ordenados por UnidadEjecutora.</returns>
        public async Task<List<InstitucionSeleccionDto>> ObtenerInstitucionesSelectorAsync()
        {
            var instituciones = await _repository.ObtenerInstitucionesAsync();
            return instituciones
                .Where(institucion => institucion.Estatus)
                .Select(InstitucionMapper.ToDto)
                .DistinctBy(institucion => institucion.Estructura)
                .OrderBy(x => x.UnidadEjecutora)
                .ToList();
        }
    }
}
