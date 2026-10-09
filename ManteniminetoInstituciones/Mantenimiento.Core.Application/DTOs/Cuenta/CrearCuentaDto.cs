using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mantenimiento.Core.Application.DTOs.Cuenta
{
    public record CrearCuentaDto
    {
        public int IdCuentaBase { get; set; } // Representa el id_cuenta de ct.cuentas
        public string? DescripcionCuenta { get; set; } // Opcional (si viene nulo toma la original)
        public int TipoTramite { get; set; }
    }
}
