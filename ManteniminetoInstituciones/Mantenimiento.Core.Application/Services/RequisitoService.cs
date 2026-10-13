using Mantenimiento.Core.Application.DTOs.Requisito;
using Mantenimiento.Core.Application.DTOs.Resultado;
using Mantenimiento.Core.Application.InterfaceServices;
using Mantenimiento.Core.Application.Mappings;
using Mantenimiento.Core.Domain.RepositoryInterfaces;

namespace Mantenimiento.Core.Application.Services
{
    public class RequisitoService : IRequisitoService
    {
        private readonly IRequisitoRepository _repository;

        public RequisitoService(IRequisitoRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<RequisitoDto>> ObtenerRequisitosAsync(string? busqueda)
        {
            var entidades = await _repository.ObtenerRequisitosAsync(busqueda);
            return entidades.Select(RequisitoMapper.ToDto).ToList();
        }

        public async Task<ResultadoOperacionDto> CrearAsync(CrearRequisitoDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.RequisitoDescripcion))
                throw new ArgumentException("La descripción del requisito es obligatoria.");

            var res = await _repository.CrearAsync(dto.RequisitoDescripcion.Trim());
            return ResultadoOperacionMapper.ToDto(res);
        }

        public async Task<ResultadoOperacionDto> ActualizarAsync(ActualizarRequisitoDto dto)
        {
            if (dto.RequisitosID <= 0)
                throw new ArgumentException("El ID del requisito es inválido.");

            if (string.IsNullOrWhiteSpace(dto.RequisitoDescripcion))
                throw new ArgumentException("La descripción del requisito no puede estar vacía.");

            var res = await _repository.ActualizarAsync(dto.RequisitosID, dto.RequisitoDescripcion.Trim());
            return ResultadoOperacionMapper.ToDto(res);
        }

        public async Task<ResultadoOperacionDto> EliminarAsync(int requisitosID)
        {
            if (requisitosID <= 0)
                throw new ArgumentException("El ID del requisito es inválido.");

            var res = await _repository.EliminarAsync(requisitosID);
            return ResultadoOperacionMapper.ToDto(res);
        }
    }
}