using Mantenimiento.Core.Application.DTOs.Auditoria;

namespace Mantenimiento.Core.Application.DTOs.Cuenta.DboCuentasDTO
{
    public record ActualizarCuentaDto : AuditableDto
    {
        public int IdCuenta { get; set; }
        public string DescripcionCuenta { get; set; } = string.Empty;
        public int TipoTramite { get; set; }
    }
}
