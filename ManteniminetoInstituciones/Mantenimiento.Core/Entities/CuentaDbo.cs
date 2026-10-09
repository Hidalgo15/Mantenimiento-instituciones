using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mantenimiento.Core.Domain.Entities
{
    public class CuentaDbo
    {
        public int IdCuenta { get; set; }
        public string CodigoCuenta { get; set; } = string.Empty;
        public string DescripcionCuenta { get; set; } = string.Empty;
        public int TipoTramiteId { get; set; }

        [NotMapped]
        // Propiedad opcional para mapear el nombre del trámite en consultas
        public string? TipoTramiteNombre { get; set; }
    }
}
