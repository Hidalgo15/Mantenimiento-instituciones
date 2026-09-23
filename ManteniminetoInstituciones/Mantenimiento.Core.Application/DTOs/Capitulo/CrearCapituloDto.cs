using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mantenimiento.Core.Application.DTOs.Capitulo
{
    public class CrearCapituloDto
    {
        public string CodigoCapitulo { get; set; } = string.Empty;
        public string NombreCapitulo { get; set; } = string.Empty;
    }
}
