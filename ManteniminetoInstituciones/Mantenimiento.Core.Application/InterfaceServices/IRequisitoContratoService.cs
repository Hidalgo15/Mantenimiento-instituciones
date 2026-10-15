using Mantenimiento.Core.Application.DTOs.RequisitoContrato;

namespace Mantenimiento.Core.Application.InterfaceServices
{
    public interface IRequisitoContratoService
    {
        Task<List<RequisitoContratoDto>> ObtenerRequisitosAsync(string? busqueda = null);
        Task<RequisitoContratoDto> ObtenerPorIdAsync(int codigoRequisito);
        Task<int> CrearRequisitoContratoAsync(CrearRequisitoContratoDto dto);
        Task ActualizarRequisitoContratoAsync(ActualizarRequisitoContratoDto dto);
        Task EliminarRequisitoContratoAsync(int codigoRequisito);
    }
}
