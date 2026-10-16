namespace Mantenimiento.Core.Domain.Entities
{
    public class CuentaRequisito
    {
        public int CuentaRequisitosID { get; set; }
        public double IdCuenta { get; set; }
        public int IdRequisito { get; set; }
        public bool Estado { get; set; }
    }
}
