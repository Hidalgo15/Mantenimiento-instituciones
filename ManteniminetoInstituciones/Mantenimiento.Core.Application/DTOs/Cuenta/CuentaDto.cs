using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mantenimiento.Core.Application.DTOs.Cuenta
{
    public record CuentaDto
    {
        public int IdCuenta { get; set; }
        public string CodigoCuenta { get; set; } = string.Empty;
        public string DescripcionCuenta { get; set; } = string.Empty;
        public int TipoTramite { get; set; }
        public string TipoTramiteNombre { get; set; } = string.Empty;
    }
}
