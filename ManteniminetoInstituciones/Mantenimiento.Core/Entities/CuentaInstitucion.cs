namespace Mantenimiento.Core.Domain.Entities
{
    public class CuentaInstitucion
    {
        public int? Id { get; set; }
        public string InsCodigo { get; set; }
        public string Institucion { get; set; }
        public string CtaCtaBanco { get; set; }
        public string CtaDescripcion { get; set; }
    }
}
