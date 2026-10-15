using Mantenimiento.Core.Application.DTOs.ClasificacionContrato;
using Mantenimiento.Core.Domain.Entities.ClasificacionContrato;

namespace Mantenimiento.Core.Application.Mappings
{
    public static class ClasificacionContratoMapper
    {
        public static ClasificacionContratoDto ToDto(ClasificacionContrato entity) => new()
        {
            CodigoClasificacion = entity.CodigoClasificacion,
            ContratoTipo = entity.ContratoTipo,
            TipoOperacion = entity.TipoOperacion,
            ContratoProcesoSeleccion = entity.ContratoProcesoSeleccion
        };

        public static ProcesoSeleccionCatalogoDto ToDto(ProcesoSeleccionCatalogo entity) => new()
        {
            Id = entity.Id,
            Proceso = entity.Proceso
        };

        public static ClasificacionOpcionDto ToDto(ClasificacionOpcion entity) => new()
        {
            Valor = entity.Valor,
            Texto = entity.Texto
        };
    }
}