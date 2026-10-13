using Mantenimiento.Core.Application.DTOs.Institucion;
using Mantenimiento.Core.Domain.Entities;

namespace Mantenimiento.Core.Application.Mappings
{
    public static class InstitucionMapper
    {
        public static InstitucionSeleccionDto ToDto(Institucion entity) => new()
        {
            IdInstitucion = entity.IdInstitucion,
            Estructura = entity.Estructura,
            UnidadEjecutora = entity.UnidadEjecutora
        };
    }
}
