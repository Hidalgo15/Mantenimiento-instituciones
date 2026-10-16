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

        /// <summary>
        /// Obtiene una lista de relaciones entre clasificaciones de contrato y requisitos.
        /// </summary>
        /// <param name="codigoClasificacion"></param>
        /// <param name="codigoRequisito"></param>
        /// <returns></returns>
        public async Task<List<RelacionContratoRequisitoDto>> ObtenerAsync(int? codigoClasificacion, int? codigoRequisito)
        {
            var entidades = await _repository.ObtenerAsync(codigoClasificacion, codigoRequisito);
            return entidades.Select(RelacionContratoRequisitoMapper.ToDto).ToList();
        }

        /// <summary>
        /// Asigna de forma asíncrona un requisito a una clasificación de contrato.
        /// </summary>
        /// <remarks>Invoca el repositorio para realizar la asignación y mapea el resultado a un
        /// DTO.</remarks>
        /// <param name="dto">Contiene los códigos de clasificación y requisito necesarios para la asignación.</param>
        /// <returns>Un ResultadoOperacionDto que indica el resultado de la operación.</returns>
        /// <exception cref="ArgumentException">Se lanza si CodigoClasificacion o CodigoRequisito no son valores válidos (<= 0).</exception>
        public async Task<ResultadoOperacionDto> AsignarAsync(CrearRelacionContratoRequisitoDto dto)
        {
            if (dto.CodigoClasificacion <= 0)
                throw new ArgumentException("Debe seleccionar una clasificación de contrato válida.");
            if (dto.CodigoRequisito <= 0)
                throw new ArgumentException("Debe seleccionar un requisito válido.");

            var res = await _repository.AsignarAsync(dto.CodigoClasificacion, dto.CodigoRequisito);
            return ResultadoOperacionMapper.ToDto(res);
        }

        /// <summary>
        /// Actualiza de forma asíncrona la relación entre contrato y requisito.
        /// </summary>
        /// <remarks>Valida los parámetros, delega la actualización al repositorio y mapea el resultado a
        /// ResultadoOperacionDto.</remarks>
        /// <param name="dto">DTO con el identificador de la relación y los códigos de clasificación y requisito para la actualización.</param>
        /// <returns>ResultadoOperacionDto que indica el resultado de la operación de actualización.</returns>
        /// <exception cref="ArgumentException">Si el identificador de la relación, el código de clasificación o el código de requisito son menores o
        /// iguales a cero.</exception>
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

        /// <summary>
        /// Elimina de forma asíncrona la relación con el identificador especificado y devuelve el resultado de la
        /// operación.
        /// </summary>
        /// <param name="id">Identificador de la relación a eliminar. Debe ser mayor que cero.</param>
        /// <returns>ResultadoOperacionDto que representa el resultado de la operación de eliminación.</returns>
        /// <exception cref="ArgumentException">Se lanza si id es menor o igual que cero.</exception>
        public async Task<ResultadoOperacionDto> EliminarAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("El ID de la relación es inválido.");

            var res = await _repository.EliminarAsync(id);
            return ResultadoOperacionMapper.ToDto(res);
        }

        /// <summary>
        /// Obtiene de forma asincrónica el catálogo de clasificación y lo convierte a una lista de
        /// CatalogoClasificacionContratoDto.
        /// </summary>
        /// <remarks>Los elementos se recuperan del repositorio mediante
        /// _repository.ObtenerCatalogoClasificacionAsync y se mapean con
        /// RelacionContratoRequisitoMapper.ToDto.</remarks>
        /// <returns>Tarea que representa la operación asincrónica cuyo resultado es una lista de
        /// CatalogoClasificacionContratoDto.</returns>
        public async Task<List<CatalogoClasificacionContratoDto>> ObtenerCatalogoClasificacionAsync()
        {
            var catalogo = await _repository.ObtenerCatalogoClasificacionAsync();
            return catalogo.Select(RelacionContratoRequisitoMapper.ToDto).ToList();
        }

        /// <summary>
        /// Obtiene de forma asíncrona el catálogo de requisitos desde el repositorio y lo mapea a una lista de
        /// CatalogoRequisitoContratoDto.
        /// </summary>
        /// <remarks>Los elementos se obtienen mediante _repository.ObtenerCatalogoRequisitosAsync y se
        /// convierten usando RelacionContratoRequisitoMapper.ToDto.</remarks>
        /// <returns>Tarea que representa la operación asíncrona y devuelve la lista de CatalogoRequisitoContratoDto resultante.</returns>
        public async Task<List<CatalogoRequisitoContratoDto>> ObtenerCatalogoRequisitosAsync()
        {
            var catalogo = await _repository.ObtenerCatalogoRequisitosAsync();
            return catalogo.Select(RelacionContratoRequisitoMapper.ToDto).ToList();
        }
    }
}