using Mantenimiento.Core.Application.DTOs.Auditoria;
using System.ComponentModel.DataAnnotations;

namespace Mantenimiento.Core.Application.DTOs.ResponsableActividad
{
    public record GestionarActividadUsuariosDto : AuditableDto
    {
        [Required(ErrorMessage = "El código de actividad es obligatorio.")]
        public int CodigoActividad { get; set; }

        public int CodigoTre { get; set; }

        public string? ListaCodigosDespa { get; set; }
    }
}
