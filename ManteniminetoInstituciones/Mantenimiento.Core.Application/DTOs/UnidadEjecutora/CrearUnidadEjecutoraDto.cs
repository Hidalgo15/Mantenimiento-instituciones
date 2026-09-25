using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mantenimiento.Core.Application.DTOs.UnidadEjecutora
{
    public record CrearUnidadEjecutoraDto
    {
        public int? IdPadre { get; set; }
        public int IdDaf { get; set; }
        public string CodigoUnidadEjecutora { get; set; } = string.Empty;
        public string NombreUnidadEjecutora { get; set; } = string.Empty;
        public string? Rnc { get; set; }
        public bool Estado { get; set; } = true;
        public bool PortalCompra { get; set; } = false;
        public string Usuario { get; set; } = string.Empty;
    }
}
