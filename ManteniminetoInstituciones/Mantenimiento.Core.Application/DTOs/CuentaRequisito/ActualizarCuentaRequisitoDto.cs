using Mantenimiento.Core.Application.DTOs.Auditoria;

namespace Mantenimiento.Core.Application.DTOs.CuentaRequisito
{
    public record ActualizarCuentaRequisitoDto : AuditableDto
    {
        public int CuentaRequisitosID { get; set; }
        public double IdCuenta { get; set; }
        public int IdRequisito { get; set; }
        public bool Estado { get; set; }
    }
}
