using Mantenimiento.Core.Application.DTOs.Cuenta.DboCuentasDTO;
using Mantenimiento.Core.Domain.Entities;

namespace Mantenimiento.Core.Application.Mappings
{
    public static class CuentaMapeoMapper
    {
        public static CuentaDto ToDto(CuentaDbo entity) => new()
        {
            IdCuenta = entity.IdCuenta,
            CodigoCuenta = entity.CodigoCuenta,
            DescripcionCuenta = entity.DescripcionCuenta,
            TipoTramite = entity.TipoTramiteId,
            TipoTramiteNombre = entity.TipoTramiteNombre ?? string.Empty
        };
    }
}
