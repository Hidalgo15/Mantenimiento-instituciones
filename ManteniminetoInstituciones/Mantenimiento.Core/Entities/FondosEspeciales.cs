namespace Mantenimiento.Core.Domain.Entities
{
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Fondo_especiales", Schema = "dbo")]
    public class FondosEspeciales
    {
        public int Id { get; set; }

        public string Fondo { get; set; } = string.Empty;

        [Column("descripcion")]
        public string? Descripcion { get; set; }

        [Column("estructura_institucion")]
        public string? Estructura { get; set; }

        [Column("tipo_tramite")]
        public string TipoTramite { get; set; } = string.Empty;
    }
}