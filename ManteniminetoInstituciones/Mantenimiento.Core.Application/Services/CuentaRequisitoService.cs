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

        /// <summary>
        /// Obtiene de forma asíncrona las relaciones entre cuentas y requisitos y las devuelve como DTOs.
        /// </summary>
        /// <remarks>Consulta el repositorio mediante _repository.ObtenerRelacionAsync y convierte las
        /// entidades a DTOs usando CuentaRequisitoMapper.ToDto.</remarks>
        /// <param name="idCuenta">Identificador de la cuenta; si es null, no se aplica filtro por cuenta.</param>
        /// <param name="idRequisito">Identificador del requisito; si es null, no se aplica filtro por requisito.</param>
        /// <param name="estado">Estado de la relación; si es null, no se aplica filtro por estado.</param>
        /// <returns>Lista de CuentaRequisitoDto que cumplen los criterios de filtrado.</returns>
        public async Task<List<CuentaRequisitoDto>> ObtenerRelacionAsync(double? idCuenta, int? idRequisito, bool? estado)
        {
            var entidades = await _repository.ObtenerRelacionAsync(idCuenta, idRequisito, estado);
            return entidades.Select(CuentaRequisitoMapper.ToDto).ToList();
        }

        /// <summary>
        /// Asigna un requisito a una cuenta según los datos proporcionados y devuelve el resultado de la operación.
        /// </summary>
        /// <remarks>Valida los identificadores antes de delegar la asignación al repositorio de forma
        /// asíncrona.</remarks>
        /// <param name="dto">Objeto DTO que contiene IdCuenta, IdRequisito y Estado que indican qué requisito asignar y su estado.</param>
        /// <returns>ResultadoOperacionDto con el resultado de la asignación.</returns>
        public async Task<ResultadoOperacionDto> AsignarAsync(CrearCuentaRequisitoDto dto)
        {
            ValidarDatos(dto.IdCuenta, dto.IdRequisito);
            var res = await _repository.AsignarAsync(dto.IdCuenta, dto.IdRequisito, dto.Estado);
            return ResultadoOperacionMapper.ToDto(res);
        }

        /// <summary>
        /// Actualiza de forma asíncrona la relación entre una cuenta y un requisito.
        /// </summary>
        /// <param name="dto">Datos de actualización que incluyen CuentaRequisitosID, IdCuenta, IdRequisito y Estado.</param>
        /// <returns>Un ResultadoOperacionDto que representa el resultado de la operación de actualización.</returns>
        /// <exception cref="ArgumentException">Se lanza cuando CuentaRequisitosID es menor o igual a cero.</exception>
        public async Task<ResultadoOperacionDto> ActualizarAsync(ActualizarCuentaRequisitoDto dto)
        {
            if (dto.CuentaRequisitosID <= 0)
                throw new ArgumentException("El ID de la relación es inválido.");

            ValidarDatos(dto.IdCuenta, dto.IdRequisito);
            var res = await _repository.ActualizarAsync(dto.CuentaRequisitosID, dto.IdCuenta, dto.IdRequisito, dto.Estado);
            return ResultadoOperacionMapper.ToDto(res);
        }

        /// <summary>
        /// Elimina de forma asíncrona la relación entre cuenta y requisitos identificada por su ID y devuelve el
        /// resultado de la operación.
        /// </summary>
        /// <remarks>La operación se delega al repositorio y el resultado se mapea a DTO mediante
        /// ResultadoOperacionMapper.</remarks>
        /// <param name="cuentaRequisitosID">ID de la relación cuenta-requisitos; debe ser mayor que cero.</param>
        /// <returns>Un ResultadoOperacionDto que indica el resultado de la operación de eliminación.</returns>
        /// <exception cref="ArgumentException">Si cuentaRequisitosID es menor o igual que cero.</exception>
        public async Task<ResultadoOperacionDto> EliminarAsync(int cuentaRequisitosID)
        {
            if (cuentaRequisitosID <= 0)
                throw new ArgumentException("El ID de la relación es inválido.");

            var res = await _repository.EliminarAsync(cuentaRequisitosID);
            return ResultadoOperacionMapper.ToDto(res);
        }

        /// <summary>
        /// Obtiene de forma asíncrona el catálogo de cuentas y lo transforma a instancias de CuentaCatalogoDto.
        /// </summary>
        /// <remarks>Los elementos se recuperan desde el repositorio y se mapean mediante
        /// CuentaRequisitoMapper.ToDto.</remarks>
        /// <returns>Una tarea cuyo resultado es una lista de CuentaCatalogoDto que contiene el catálogo de cuentas.</returns>
        public async Task<List<CuentaCatalogoDto>> ObtenerCatalogoCuentasAsync()
        {
            var entidades = await _repository.ObtenerCatalogoCuentasAsync();
            return entidades.Select(CuentaRequisitoMapper.ToDto).ToList();
        }

        /// <summary>
        /// Obtiene el catálogo de requisitos y lo devuelve como una lista de RequisitoCatalogoDto.
        /// </summary>
        /// <returns>Una tarea que representa la operación asincrónica. El resultado contiene una lista de RequisitoCatalogoDto
        /// con el catálogo de requisitos.</returns>
        public async Task<List<RequisitoCatalogoDto>> ObtenerCatalogoRequisitosAsync()
        {
            var entidades = await _repository.ObtenerCatalogoRequisitosAsync();
            return entidades.Select(CuentaRequisitoMapper.ToDto).ToList();
        }

        /// <summary>
        /// Valida que idCuenta e idRequisito sean mayores que cero.
        /// </summary>
        /// <param name="idCuenta">Identificador de la cuenta; debe ser mayor que cero.</param>
        /// <param name="idRequisito">Identificador del requisito; debe ser mayor que cero.</param>
        /// <exception cref="ArgumentException">Se lanza si idCuenta o idRequisito son menores o iguales a cero.</exception>
        private static void ValidarDatos(double idCuenta, int idRequisito)
        {
            if (idCuenta <= 0)
                throw new ArgumentException("Debe seleccionar una cuenta válida.");

            if (idRequisito <= 0)
                throw new ArgumentException("Debe seleccionar un requisito válido.");
        }
    }
}