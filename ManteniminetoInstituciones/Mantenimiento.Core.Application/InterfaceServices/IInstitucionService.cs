using Mantenimiento.Core.Application.DTOs.Institucion;

namespace Mantenimiento.Core.Application.InterfaceServices
{
    public interface IInstitucionService
    {
        Task<List<InstitucionSeleccionDto>> ObtenerInstitucionesSelectorAsync();
    }
}
