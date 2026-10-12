namespace Mantenimiento.Core.Domain.Entities
{
    public class CuentaCatalogo
    {
        public double IdCuenta { get; set; }
        public string CodigoCuenta { get; set; } = string.Empty;
        public string DescripcionCuenta { get; set; } = string.Empty;
    }
}
