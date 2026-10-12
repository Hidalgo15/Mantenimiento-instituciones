using Mantenimiento.Core.Application.DTOs.Auditoria;

namespace Mantenimiento.Core.Application.DTOs.Capitulo
{
    public record CrearCapituloDto : AuditableDto
    {
        public string CodigoCapitulo { get; set; } = string.Empty;
        public string NombreCapitulo { get; set; } = string.Empty;
    }
}
