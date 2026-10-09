using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mantenimiento.Core.Application.DTOs.TipoTramiteContrato
{
    public record TipoTramiteContratoDto
    {
        public int Id { get; set; }
        public string TipoTramite { get; set; } = string.Empty;
    }
}
