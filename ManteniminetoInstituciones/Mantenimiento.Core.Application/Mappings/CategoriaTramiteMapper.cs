using Mantenimiento.Core.Application.DTOs.UnidadEjecutora;
using Mantenimiento.Core.Domain.Entities;

namespace Mantenimiento.Core.Application.Mappings
{
    public static class CategoriaTramiteMapper
    {
        public static CategoriaTramiteDto ToDto(CategoriaTramite entity) => new()
        {
            CodigoCategoria = entity.CodigoCategoria,
            Descripcion = entity.Descripcion
        };
    }
}
