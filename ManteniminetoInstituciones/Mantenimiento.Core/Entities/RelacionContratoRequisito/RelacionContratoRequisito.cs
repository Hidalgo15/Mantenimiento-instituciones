namespace Mantenimiento.Core.Domain.Entities.RelacionContratoRequisito
{
    public class RelacionContratoRequisito
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
}
