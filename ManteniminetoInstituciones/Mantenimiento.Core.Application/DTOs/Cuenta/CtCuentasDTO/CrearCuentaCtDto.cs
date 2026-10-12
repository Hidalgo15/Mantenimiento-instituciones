namespace Mantenimiento.Core.Application.DTOs.Cuenta.CtCuentasDTO
{
    public record CrearCuentaCtDto
    { 
        public double? IdCuenta { get; set; }
        public string? CodigoCuenta { get; set; }
        public string? DescripcionCuenta { get; set; }
    }
}