using Mantenimiento.Core.Application.DTOs.Cuenta.CtCuentasDTO;
using Mantenimiento.Core.Domain.Entities;

namespace Mantenimiento.Core.Application.Mappings
{
    public static class CuentaCtMapper
    {
        public static CuentaCtDto ToDto(CuentaCt entity) => new()
        {
            CuentaID = entity.CuentaID,
            IdCuenta = entity.IdCuenta,
            CodigoCuenta = entity.CodigoCuenta ?? string.Empty,
            DescripcionCuenta = entity.DescripcionCuenta ?? string.Empty
        };
    }
}
