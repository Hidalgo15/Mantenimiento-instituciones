using Mantenimiento.Core.Application.DTOs.ClasificacionContrato;
using Mantenimiento.Core.Application.InterfaceServices;
using Mantenimiento.Core.Application.Mappings;
using Mantenimiento.Core.Domain.RepositoryInterfaces;

namespace Mantenimiento.Core.Application.Services
{
    public class ClasificacionContratoService : IClasificacionContratoService
    {
        private readonly IClasificacionContratoRepository _repository;

        public ClasificacionContratoService(IClasificacionContratoRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<ClasificacionContratoDto>> ObtenerAsync(int? codigoClasificacion, string? contratoTipo, string? tipoOperacion, string? contratoProcesoSeleccion)
        {
            var entidades = await _repository.ObtenerAsync(codigoClasificacion, contratoTipo, tipoOperacion, contratoProcesoSeleccion);
            return entidades.Select(ClasificacionContratoMapper.ToDto).ToList();
        }

        public async Task<ClasificacionContratoDto?> ObtenerPorIdAsync(int codigoClasificacion)
        {
            var entidad = (await _repository.ObtenerAsync(codigoClasificacion, null, null, null)).FirstOrDefault();
            return entidad != null ? ClasificacionContratoMapper.ToDto(entidad) : null;
        }

        public async Task<int> CrearAsync(CrearClasificacionContratoDto dto)
        {
            return await _repository.CrearAsync(dto.ContratoTipo, dto.TipoOperacion, dto.ContratoProcesoSeleccion);
        }

        public async Task ActualizarAsync(ActualizarClasificacionContratoDto dto)
        {
            if (dto.CodigoClasificacion <= 0)
                throw new ArgumentException("El código de clasificación es inválido.");

            await _repository.ActualizarAsync(dto.CodigoClasificacion, dto.ContratoTipo, dto.TipoOperacion, dto.ContratoProcesoSeleccion);
        }

        public async Task EliminarAsync(int codigoClasificacion)
        {
            if (codigoClasificacion <= 0)
                throw new ArgumentException("El código de clasificación es inválido.");

            await _repository.EliminarAsync(codigoClasificacion);
        }

        public async Task<List<ProcesoSeleccionCatalogoDto>> ObtenerCatalogoProcesoSeleccionAsync()
        {
            var catalogo = await _repository.ObtenerCatalogoProcesoSeleccionAsync();
            return catalogo.Select(ClasificacionContratoMapper.ToDto).ToList();
        }

        public async Task<List<ClasificacionOpcionDto>> ObtenerOpcionesAsync(string tipoOpcion)
        {
            var opciones = await _repository.ObtenerOpcionesAsync(tipoOpcion);
            return opciones.Select(ClasificacionContratoMapper.ToDto).ToList();
        }
    }
}