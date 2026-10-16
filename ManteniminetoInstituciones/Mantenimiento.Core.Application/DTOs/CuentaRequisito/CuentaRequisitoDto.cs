namespace Mantenimiento.Core.Application.DTOs.CuentaRequisito
{
    public class CuentaRequisitoDto
    {
        public int CuentaRequisitosID { get; set; }
        public double IdCuenta { get; set; }
        public string CodigoCuenta { get; set; } = string.Empty;
        public string DescripcionCuenta { get; set; } = string.Empty;
        public int IdRequisito { get; set; }
        public string DescripcionRequisito { get; set; } = string.Empty;
        public bool Estado { get; set; }
    }
}
