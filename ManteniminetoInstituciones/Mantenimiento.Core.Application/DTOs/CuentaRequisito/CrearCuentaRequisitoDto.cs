namespace Mantenimiento.Core.Application.DTOs.CuentaRequisito
{
    public class CrearCuentaRequisitoDto
    {
        public double IdCuenta { get; set; }
        public int IdRequisito { get; set; }
        public bool Estado { get; set; } = true;
    }
}
