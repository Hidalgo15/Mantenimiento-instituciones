using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mantenimiento.Core.Domain.Entities
{
    public class TipoTramitePago
    {
        public int Id { get; set; }
        public string Tipo { get; set; } = string.Empty;
    }
}
