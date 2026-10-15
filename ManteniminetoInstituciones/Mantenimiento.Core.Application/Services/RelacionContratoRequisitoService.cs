using Mantenimiento.Core.Application.DTOs.RelacionContratoRequisito;
using Mantenimiento.Core.Application.DTOs.Resultado;
using Mantenimiento.Core.Application.InterfaceServices;
using Mantenimiento.Core.Application.Mappings;
using Mantenimiento.Core.Domain.RepositoryInterfaces;

namespace Mantenimiento.Core.Application.Services
{
    public class RelacionContratoRequisitoService : IRelacionContratoRequisitoService
    {
        private readonly IRelacionContratoRequisitoRepository _repository;

        public RelacionContratoRequisitoService(IRelacionContratoRequisitoRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<RelacionContratoRequisitoDto>> ObtenerAsync(int? codigoClasificacion, int? codigoRequisito)
        {
            var entidades = await _repository.ObtenerAsync(codigoClasificacion, codigoRequisito);
            return entidades.Select(RelacionContratoRequisitoMapper.ToDto).ToList();
        }

        public async Task<ResultadoOperacionDto> AsignarAsync(CrearRelacionContratoRequisitoDto dto)
        {
            if (dto.CodigoClasificacion <= 0)
                throw new ArgumentException("Debe seleccionar una clasificación de contrato válida.");
            if (dto.CodigoRequisito <= 0)
                throw new ArgumentException("Debe seleccionar un requisito válido.");

            var res = await _repository.AsignarAsync(dto.CodigoClasificacion, dto.CodigoRequisito);
            return ResultadoOperacionMapper.ToDto(res);
        }

        public async Task<ResultadoOperacionDto> ActualizarAsync(ActualizarRelacionContratoRequisitoDto dto)
        {
            if (dto.Id <= 0)
                throw new ArgumentException("El ID de la relación es inválido.");
            if (dto.CodigoClasificacion <= 0)
                throw new ArgumentException("Debe seleccionar una clasificación de contrato válida.");
            if (dto.CodigoRequisito <= 0)
                throw new ArgumentException("Debe seleccionar un requisito válido.");

            var res = await _repository.ActualizarAsync(dto.Id, dto.CodigoClasificacion, dto.CodigoRequisito);
            return ResultadoOperacionMapper.ToDto(res);
        }

        public async Task<ResultadoOperacionDto> EliminarAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("El ID de la relación es inválido.");

            var res = await _repository.EliminarAsync(id);
            return ResultadoOperacionMapper.ToDto(res);
        }

        public async Task<List<CatalogoClasificacionContratoDto>> ObtenerCatalogoClasificacionAsync()
        {
            var catalogo = await _repository.ObtenerCatalogoClasificacionAsync();
            return catalogo.Select(RelacionContratoRequisitoMapper.ToDto).ToList();
        }

        public async Task<List<CatalogoRequisitoContratoDto>> ObtenerCatalogoRequisitosAsync()
        {
            var catalogo = await _repository.ObtenerCatalogoRequisitosAsync();
            return catalogo.Select(RelacionContratoRequisitoMapper.ToDto).ToList();
        }
    }
}