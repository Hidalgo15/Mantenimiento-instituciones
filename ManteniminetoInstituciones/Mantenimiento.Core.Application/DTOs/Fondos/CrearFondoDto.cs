using Mantenimiento.Core.Domain.Entities;

namespace Mantenimiento.Core.Application.DTOs.Fondos
{
    public record CrearFondoDto
    {

        public int IdInstitucion { get; set; }
        public string Fondo { get; set; } = string.Empty;
        public string? Descripcion { get; set; } = string.Empty;
        public string? Estructura { get; set; } = string.Empty;
        public TramiteContrato? tipoTramiteContrato { get; set; } = new TramiteContrato();

    }
}
