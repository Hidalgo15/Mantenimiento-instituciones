using Mantenimiento.Core.Application.DTOs.RequisitoContrato;
using Mantenimiento.Core.Domain.Entities;


namespace Mantenimiento.Core.Application.Mappings
{
    public static class RequisitoContratoMapper
    {
        public static RequisitoContratoDto ToDto(RequisitoContrato entity) => new()
        {
            CodigoRequisito = entity.CodigoRequisito,
            DescripcionRequisito = entity.DescripcionRequisito,
            CodigoDocumentoTre = entity.CodigoDocumentoTre
        };

        public static RequisitoContrato ToEntity(CrearRequisitoContratoDto dto) => new()
        {
            DescripcionRequisito = dto.DescripcionRequisito?.Trim() ?? string.Empty,
            CodigoDocumentoTre = dto.CodigoDocumentoTre
        };

        public static RequisitoContrato ToEntity(ActualizarRequisitoContratoDto dto) => new()
        {
            CodigoRequisito = dto.CodigoRequisito,
            DescripcionRequisito = dto.DescripcionRequisito?.Trim() ?? string.Empty,
            CodigoDocumentoTre = dto.CodigoDocumentoTre
        };
    }
}
