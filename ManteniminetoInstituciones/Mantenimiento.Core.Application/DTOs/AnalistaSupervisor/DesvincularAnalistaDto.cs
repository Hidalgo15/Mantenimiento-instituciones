using Mantenimiento.Core.Application.DTOs.Auditoria;
using System.ComponentModel.DataAnnotations;

namespace Mantenimiento.Core.Application.DTOs.AnalistaSupervisor
{
    public record DesvincularAnalistaDto : AuditableDto
    {
        [Required(ErrorMessage = "El despacho del analista es obligatorio.")]
        [StringLength(20, ErrorMessage = "El despacho no debe superar los 20 caracteres.")]
        public string DespachoAnalista { get; set; } = string.Empty;
    }
}
