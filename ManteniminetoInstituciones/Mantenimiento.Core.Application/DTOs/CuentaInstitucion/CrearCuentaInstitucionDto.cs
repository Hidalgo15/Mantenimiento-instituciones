using Mantenimiento.Core.Application.DTOs.Auditoria;

namespace Mantenimiento.Core.Application.DTOs.CuentaInstitucion
{
    public record CrearCuentaInstitucionDto : AuditableDto
    {
        public string InsCodigo { get; set; }
        public string Institucion { get; set; }
        public string CtaCtaBanco { get; set; }
        public string CtaDescripcion { get; set; }
    }
}
