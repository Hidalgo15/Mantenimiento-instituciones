using Mantenimiento.Core.Application.DTOs.RelacionContratoRequisito;
using Mantenimiento.Core.Domain.Entities.RelacionContratoRequisito;

namespace Mantenimiento.Core.Application.Mappings
{
    public static class RelacionContratoRequisitoMapper
    {
        public static RelacionContratoRequisitoDto ToDto(RelacionContratoRequisito entity) => new()
        {
            Id = entity.Id,
            CodigoClasificacion = entity.CodigoClasificacion,
            ContratoTipo = entity.ContratoTipo,
            TipoOperacion = entity.TipoOperacion,
            ContratoProcesoSeleccion = entity.ContratoProcesoSeleccion,
            CodigoRequisito = entity.CodigoRequisito,
            DescripcionRequisito = entity.DescripcionRequisito,
            CodigoDocumentoTre = entity.CodigoDocumentoTre
        };

        public static CatalogoClasificacionContratoDto ToDto(CatalogoClasificacionContrato entity) => new()
        {
            CodigoClasificacion = entity.CodigoClasificacion,
            ContratoTipo = entity.ContratoTipo,
            TipoOperacion = entity.TipoOperacion,
            ContratoProcesoSeleccion = entity.ContratoProcesoSeleccion
        };

        public static CatalogoRequisitoContratoDto ToDto(CatalogoRequisitoContrato entity) => new()
        {
            CodigoRequisito = entity.CodigoRequisito,
            DescripcionRequisito = entity.DescripcionRequisito,
            CodigoDocumentoTre = entity.CodigoDocumentoTre
        };
    }
}