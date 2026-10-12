namespace Mantenimiento.Core.Application.DTOs.CuentaRequisito
{
    public class ActualizarCuentaRequisitoDto
    {
        public int CuentaRequisitosID { get; set; }
        public double IdCuenta { get; set; }
        public int IdRequisito { get; set; }
        public bool Estado { get; set; }
    }
}
