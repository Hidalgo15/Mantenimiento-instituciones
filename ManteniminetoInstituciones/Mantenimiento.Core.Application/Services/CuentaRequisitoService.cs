using Mantenimiento.Core.Application.DTOs.CuentaRequisito;
using Mantenimiento.Core.Application.DTOs.Resultado;
using Mantenimiento.Core.Application.InterfaceServices;
using Mantenimiento.Core.Application.Mappings;
using Mantenimiento.Core.Domain.RepositoryInterfaces;

namespace Mantenimiento.Core.Application.Services
{
    public class CuentaRequisitoService : ICuentaRequisitoService
    {
        private readonly ICuentaRequisitoRepository _repository;

        public CuentaRequisitoService(ICuentaRequisitoRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<CuentaRequisitoDto>> ObtenerRelacionAsync(double? idCuenta, int? idRequisito, bool? estado)
        {
            var entidades = await _repository.ObtenerRelacionAsync(idCuenta, idRequisito, estado);
            return entidades.Select(CuentaRequisitoMapper.ToDto).ToList();
        }

        public async Task<ResultadoOperacionDto> AsignarAsync(CrearCuentaRequisitoDto dto)
        {
            ValidarDatos(dto.IdCuenta, dto.IdRequisito);
            var res = await _repository.AsignarAsync(dto.IdCuenta, dto.IdRequisito, dto.Estado);
            return ResultadoOperacionMapper.ToDto(res);
        }

        public async Task<ResultadoOperacionDto> ActualizarAsync(ActualizarCuentaRequisitoDto dto)
        {
            if (dto.CuentaRequisitosID <= 0)
                throw new ArgumentException("El ID de la relación es inválido.");

            ValidarDatos(dto.IdCuenta, dto.IdRequisito);
            var res = await _repository.ActualizarAsync(dto.CuentaRequisitosID, dto.IdCuenta, dto.IdRequisito, dto.Estado);
            return ResultadoOperacionMapper.ToDto(res);
        }

        public async Task<ResultadoOperacionDto> EliminarAsync(int cuentaRequisitosID)
        {
            if (cuentaRequisitosID <= 0)
                throw new ArgumentException("El ID de la relación es inválido.");

            var res = await _repository.EliminarAsync(cuentaRequisitosID);
            return ResultadoOperacionMapper.ToDto(res);
        }

        public async Task<List<CuentaCatalogoDto>> ObtenerCatalogoCuentasAsync()
        {
            var entidades = await _repository.ObtenerCatalogoCuentasAsync();
            return entidades.Select(CuentaRequisitoMapper.ToDto).ToList();
        }

        public async Task<List<RequisitoCatalogoDto>> ObtenerCatalogoRequisitosAsync()
        {
            var entidades = await _repository.ObtenerCatalogoRequisitosAsync();
            return entidades.Select(CuentaRequisitoMapper.ToDto).ToList();
        }

        private static void ValidarDatos(double idCuenta, int idRequisito)
        {
            if (idCuenta <= 0)
                throw new ArgumentException("Debe seleccionar una cuenta válida.");

            if (idRequisito <= 0)
                throw new ArgumentException("Debe seleccionar un requisito válido.");
        }
    }
}