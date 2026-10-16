using Mantenimiento.Core.Application.DTOs.Auditoria;

namespace Mantenimiento.Core.Application.DTOs.Cuenta.CtCuentasDTO
{
    public record CrearCuentaCtDto : AuditableDto
    { 
        public double? IdCuenta { get; set; }
        public string? CodigoCuenta { get; set; }
        public string? DescripcionCuenta { get; set; }
    }
}