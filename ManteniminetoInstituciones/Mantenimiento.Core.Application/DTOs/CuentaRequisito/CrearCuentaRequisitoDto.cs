using Mantenimiento.Core.Application.DTOs.Auditoria;

namespace Mantenimiento.Core.Application.DTOs.CuentaRequisito
{
    public record CrearCuentaRequisitoDto : AuditableDto
    {
        public double IdCuenta { get; set; }
        public int IdRequisito { get; set; }
        public bool Estado { get; set; } = true;
    }
}
