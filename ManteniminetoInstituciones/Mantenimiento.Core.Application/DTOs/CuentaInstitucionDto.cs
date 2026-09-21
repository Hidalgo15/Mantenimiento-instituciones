namespace Mantenimiento.Core.Application.DTOs
{
    public record CuentaInstitucionDto
    {
        public int? Id { get; set; }
        public string InsCodigo { get; set; }
        public string Institucion { get; set; }
        public string CtaCtaBanco { get; set; }
        public string CtaDescripcion { get; set; }
    }
}
