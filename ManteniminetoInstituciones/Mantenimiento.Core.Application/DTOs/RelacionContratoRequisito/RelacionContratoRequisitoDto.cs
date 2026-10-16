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

    public record CrearRelacionContratoRequisitoDto
    {
        public int CodigoClasificacion { get; set; }
        public int CodigoRequisito { get; set; }
    }

    public record ActualizarRelacionContratoRequisitoDto
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
