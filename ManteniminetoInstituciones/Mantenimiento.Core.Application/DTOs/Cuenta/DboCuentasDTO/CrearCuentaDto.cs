using Mantenimiento.Core.Application.DTOs.Auditoria;

namespace Mantenimiento.Core.Application.DTOs.Cuenta.DboCuentasDTO
{
    public record CrearCuentaDto : AuditableDto
    {
        public int IdCuentaBase { get; set; } // Representa el id_cuenta de ct.cuentas
        public string? DescripcionCuenta { get; set; } // Opcional (si viene nulo toma la original)
        public int TipoTramite { get; set; }
    }
}
