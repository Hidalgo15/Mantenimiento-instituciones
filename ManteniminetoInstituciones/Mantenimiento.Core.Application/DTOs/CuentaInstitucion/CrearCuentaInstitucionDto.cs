namespace Mantenimiento.Core.Application.DTOs.CuentaInstitucion
{
    public record CrearCuentaInstitucionDto
    {
        public string InsCodigo { get; set; }
        public string Institucion { get; set; }
        public string CtaCtaBanco { get; set; }
        public string CtaDescripcion { get; set; }
    }
}
