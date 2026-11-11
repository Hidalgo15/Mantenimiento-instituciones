namespace Mantenimiento.Core.Application.DTOs.ProveedoresInhabilitados
{
    public record ProveedorInhabilitadoOpcionesDto
    {
        public IReadOnlyList<string> Generos { get; init; } = Array.Empty<string>();
        public IReadOnlyList<string> EstadosRpe { get; init; } = Array.Empty<string>();
    }
}
