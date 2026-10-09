namespace Mantenimiento.Core.Application.DTOs.Auditoria
{
    public abstract record AuditableDto
    {
        public string? Usuario { get; set; }
    }
}
