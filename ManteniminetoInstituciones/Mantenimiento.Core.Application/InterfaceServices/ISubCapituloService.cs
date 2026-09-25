

using Mantenimiento.Core.Application.DTOs.SubCapitulo;

namespace Mantenimiento.Core.Application.InterfaceServices
{
    public interface ISubCapituloService
    {
        Task<List<SubCapituloDto>> ObtenerSubCapitulosAsync( string? codigo);
        Task<SubCapituloDto> ObtenerPorIdAsync(int id, int id_capitulo, string buscar);
        Task CrearSubCapituloAsync(CrearSubCapituloDto dto);
        Task ActualizarSubCapituloAsync(ActualizarSubCapituloDto dto);
        Task EliminarSubCapituloAsync(int id);
       
        //-- 1
        Task<List<SubCapituloDto>> ObtenerSubCapitulosPorCapituloAsync(int capituloId);

        //-- 2
        Task<List<SubCapituloDto>> ObtenerSubCapitulosPorCodigoCapituloAsync(string codigoCapitulo);

        //-- 3
        Task<List<SubCapituloDto>> ObtenerSubCapitulosPorCodigoSubCapituloAsync(string codigoSubCapitulo);

        //-- 4
        Task<List<SubCapituloDto>> ObtenerSubCapitulosPorNombreCapituloAsync(string nombreCapitulo);
        
        //-- 5
        Task<List<SubCapituloDto>> ObtenerSubCapitulosPorNombreSubCapituloAsync(string nombreSubCapitulo);
    
  }
}