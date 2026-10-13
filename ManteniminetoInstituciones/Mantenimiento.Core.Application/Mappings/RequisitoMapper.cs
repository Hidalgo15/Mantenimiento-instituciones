using Mantenimiento.Core.Application.DTOs.Requisito;
using Mantenimiento.Core.Domain.Entities;

namespace Mantenimiento.Core.Application.Mappings
{
    public static class RequisitoMapper
    {
        public static RequisitoDto ToDto(Requisito entity) => new()
        {
            RequisitosID = entity.RequisitosID,
            IdRequisitos = entity.IdRequisitos,
            Requisitos = entity.Requisitos
        };

    }
}