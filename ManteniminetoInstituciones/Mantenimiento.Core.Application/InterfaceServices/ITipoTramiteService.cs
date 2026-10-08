using Mantenimiento.Core.Application.DTOs.TipoTramite;


namespace Mantenimiento.Core.Application.InterfaceServices
{
    public interface ITipoTramiteService
    {
        Task<List<TipoTramiteDto>> ObtenerTiposTramiteAsync();
        Task<TipoTramiteDto?> ObtenerPorIdAsync(int id);
    }
}
