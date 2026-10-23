namespace Mantenimiento.Core.Application.DTOs.ResponsableActividad
{
    public record SubtipoMoldeDto
    {
        public int CodigoTipoTre { get; set; }
        public int CodigoTre { get; set; }
        public string? Descripcion { get; set; }
        public string? Abreviatura { get; set; }
    }
}
