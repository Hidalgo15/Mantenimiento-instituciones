using Mantenimiento.Core.Application.DTOs.Auditoria;
using System.ComponentModel.DataAnnotations;

namespace Mantenimiento.Core.Application.DTOs.ResponsableActividad
{
    public record GestionarResponsableActividadDto : AuditableDto
    {
        [Required(ErrorMessage = "El código de despacho es obligatorio.")]
        public string CodigoDespa { get; set; } = string.Empty;

        public string? ListaCodigosActividades { get; set; }
    }
}
