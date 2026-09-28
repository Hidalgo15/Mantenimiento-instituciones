using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mantenimiento.Core.Application.DTOs.Daf
{
    public record CrearDafDTO
    {
        public int IdSubCapitulo { get; set; }
        public string CodigoDaf { get; set; } = string.Empty;
        public string Daf { get; set; } = string.Empty;
    }
}
