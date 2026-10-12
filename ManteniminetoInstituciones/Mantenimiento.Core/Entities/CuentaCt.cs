using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mantenimiento.Core.Domain.Entities
{
    public class CuentaCt
    {
        public int CuentaID { get; set; }
        public double? IdCuenta { get; set; }
        public string? CodigoCuenta { get; set; }
        public string? DescripcionCuenta { get; set; }
    }
}
