using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mantenimiento.Core.Application.DTOs.Cuenta
{
    public record ActualizarCuentaDto
    {
        public int IdCuenta { get; set; }
        public string DescripcionCuenta { get; set; } = string.Empty;
        public int TipoTramite { get; set; }
    }
}
