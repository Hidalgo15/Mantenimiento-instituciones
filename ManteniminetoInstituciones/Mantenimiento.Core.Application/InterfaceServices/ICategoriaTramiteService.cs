using Mantenimiento.Core.Application.DTOs.UnidadEjecutora;

namespace Mantenimiento.Core.Application.InterfaceServices
{
    public interface ICategoriaTramiteService
    {
        Task<IEnumerable<CategoriaTramiteDto>> ObtenerTodasAsync();
    }
}