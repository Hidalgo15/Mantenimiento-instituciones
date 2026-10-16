using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mantenimiento.Core.Domain.Entities
{
    public class RequisitoContrato
    {
        public int CodigoRequisito { get; set; }
        public string? DescripcionRequisito { get; set; }
        public int? CodigoDocumentoTre { get; set; }
    }
}
