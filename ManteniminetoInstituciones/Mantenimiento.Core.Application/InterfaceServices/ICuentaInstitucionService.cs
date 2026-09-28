using Mantenimiento.Core.Application.DTOs.CuentaInstitucion;

namespace Mantenimiento.Core.Application.InterfaceServices
{
    public interface ICuentaInstitucionService
    {
        Task<List<InstitucionSeleccionDto>> ObtenerInstitucionesSelectorAsync();
        Task<List<CuentaInstitucionDto>> ObtenerCuentasPorEstructuraAsync(string insCodigo);
        Task<CuentaInstitucionDto> ObtenerPorIdAsync(int id);
        Task CrearCuentaAsync(CrearCuentaInstitucionDto dto);
        Task ActualizarCuentaAsync(ActualizarCuentaInstitucionDto dto);
        Task EliminarCuentaAsync(int id);
    }
}