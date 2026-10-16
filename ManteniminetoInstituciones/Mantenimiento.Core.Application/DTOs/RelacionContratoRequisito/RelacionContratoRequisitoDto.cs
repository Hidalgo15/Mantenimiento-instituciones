using Mantenimiento.Core.Application.DTOs.Auditoria;

namespace Mantenimiento.Core.Application.DTOs.RelacionContratoRequisito
{
    public record RelacionContratoRequisitoDto
    {
        public int Id { get; set; }
        public int CodigoClasificacion { get; set; }
        public string? ContratoTipo { get; set; }
        public string? TipoOperacion { get; set; }
        public string? ContratoProcesoSeleccion { get; set; }
        public int CodigoRequisito { get; set; }
        public string? DescripcionRequisito { get; set; }
        public int? CodigoDocumentoTre { get; set; }
    }

    public record CrearRelacionContratoRequisitoDto : AuditableDto
    {
        public int CodigoClasificacion { get; set; }
        public int CodigoRequisito { get; set; }
    }

    public record ActualizarRelacionContratoRequisitoDto : AuditableDto
    {
        public int Id { get; set; }
        public int CodigoClasificacion { get; set; }
        public int CodigoRequisito { get; set; }
    }

    public record CatalogoClasificacionContratoDto
    {
        public int CodigoClasificacion { get; set; }
        public string? ContratoTipo { get; set; }
        public string? TipoOperacion { get; set; }
        public string? ContratoProcesoSeleccion { get; set; }
    }

    public record CatalogoRequisitoContratoDto
    {
        public int CodigoRequisito { get; set; }
        public string? DescripcionRequisito { get; set; }
        public int? CodigoDocumentoTre { get; set; }
    }
}
