using Mantenimiento.Core.Application.DTOs.Auditoria;

namespace Mantenimiento.Core.Application.DTOs.Daf
{
    public record CrearDafDTO : AuditableDto
    {
        public int IdSubCapitulo { get; set; }
        public string CodigoDaf { get; set; } = string.Empty;
        public string Daf { get; set; } = string.Empty;
    }
}
