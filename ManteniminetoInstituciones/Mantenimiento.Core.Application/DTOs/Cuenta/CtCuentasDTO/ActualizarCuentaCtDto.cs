namespace Mantenimiento.Core.Application.DTOs.Cuenta.CtCuentasDTO
{
    public record ActualizarCuentaCtDto
    {
        public int CuentaID { get; set; }
        public double? IdCuenta { get; set; }
        public string? CodigoCuenta { get; set; }
        public string? DescripcionCuenta { get; set; }
    }
}