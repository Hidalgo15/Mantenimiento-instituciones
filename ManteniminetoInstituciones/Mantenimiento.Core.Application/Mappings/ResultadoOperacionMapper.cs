using Mantenimiento.Core.Application.DTOs.Resultado;
using Mantenimiento.Core.Domain.Entities;

namespace Mantenimiento.Core.Application.Mappings
{
    public static class ResultadoOperacionMapper
    {
        public static ResultadoOperacionDto ToDto(ResultadoOperacion entity) => new()
        {
            CodigoResultado = entity.CodigoResultado,
            Mensaje = entity.Mensaje
        };
    }
}
