using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mantenimiento.Core.Domain.Entities
{
    public class CuentaRequisitoConsulta
    {
        public int CuentaRequisitosID { get; set; }
        public int IdCuenta { get; set; }
        public string CodigoCuenta { get; set; } = string.Empty;
        public string DescripcionCuenta { get; set; } = string.Empty;
        public int IdRequisito { get; set; }
        public string DescripcionRequisito { get; set; } = string.Empty;
        public bool Estado { get; set; }
    }
}
