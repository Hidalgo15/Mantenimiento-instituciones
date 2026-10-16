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

        /// <summary>
        /// Obtiene de forma asíncrona una lista de RequisitoDto filtrada por la cadena de búsqueda opcional.
        /// </summary>
        /// <remarks>Los requisitos se obtienen del repositorio y se convierten a DTO mediante
        /// RequisitoMapper.</remarks>
        /// <param name="busqueda">Cadena de búsqueda opcional para filtrar los requisitos; null o vacía devuelve todos.</param>
        /// <returns>Una tarea que representa la operación asíncrona. El resultado es la lista de RequisitoDto que coincide con
        /// la búsqueda.</returns>
        public async Task<List<RequisitoDto>> ObtenerRequisitosAsync(string? busqueda)
        {
            var entidades = await _repository.ObtenerRequisitosAsync(busqueda);
            return entidades.Select(RequisitoMapper.ToDto).ToList();
        }

        /// <summary>
        /// Crea un requisito de forma asíncrona y devuelve el resultado de la operación.
        /// </summary>
        /// <remarks>La descripción se recorta antes de persistir.</remarks>
        /// <param name="dto">Datos para crear el requisito; RequisitoDescripcion debe contener texto no vacío.</param>
        /// <returns>Un ResultadoOperacionDto que indica el resultado de la operación de creación.</returns>
        /// <exception cref="ArgumentException">Si RequisitoDescripcion es nula, vacía o contiene solo espacios en blanco.</exception>
        public async Task<ResultadoOperacionDto> CrearAsync(CrearRequisitoDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.RequisitoDescripcion))
                throw new ArgumentException("La descripción del requisito es obligatoria.");

            var res = await _repository.CrearAsync(dto.RequisitoDescripcion.Trim());
            return ResultadoOperacionMapper.ToDto(res);
        }

        /// <summary>
        /// Actualiza de forma asíncrona la descripción de un requisito identificado por dto.RequisitosID.
        /// </summary>
        /// <remarks>La descripción se recorta y la actualización se realiza mediante el repositorio
        /// (_repository.ActualizarAsync).</remarks>
        /// <param name="dto">Objeto con los datos de actualización; debe contener RequisitosID > 0 y RequisitoDescripcion no vacío; la
        /// descripción se recorta antes de actualizar.</param>
        /// <returns>ResultadoOperacionDto que representa el resultado de la operación de actualización.</returns>
        /// <exception cref="ArgumentException">Se lanza si RequisitosID es menor o igual a cero o si RequisitoDescripcion es nulo, vacío o contiene solo
        /// espacios.</exception>
        public async Task<ResultadoOperacionDto> ActualizarAsync(ActualizarRequisitoDto dto)
        {
            if (dto.RequisitosID <= 0)
                throw new ArgumentException("El ID del requisito es inválido.");

            if (string.IsNullOrWhiteSpace(dto.RequisitoDescripcion))
                throw new ArgumentException("La descripción del requisito no puede estar vacía.");

            var res = await _repository.ActualizarAsync(dto.RequisitosID, dto.RequisitoDescripcion.Trim());
            return ResultadoOperacionMapper.ToDto(res);
        }

        /// <summary>
        /// Elimina de forma asíncrona el requisito identificado por su ID y devuelve el resultado de la operación.
        /// </summary>
        /// <remarks>La operación delega en el repositorio para realizar la eliminación y mapea el
        /// resultado a ResultadoOperacionDto.</remarks>
        /// <param name="requisitosID">Identificador del requisito a eliminar. Debe ser mayor que cero.</param>
        /// <returns>Resultado de la operación de eliminación convertido a DTO, indicando éxito o fallo y detalles de la
        /// operación.</returns>
        /// <exception cref="ArgumentException">Se lanza si requisitosID es menor o igual a cero.</exception>
        public async Task<ResultadoOperacionDto> EliminarAsync(int requisitosID)
        {
            if (requisitosID <= 0)
                throw new ArgumentException("El ID del requisito es inválido.");

            var res = await _repository.EliminarAsync(requisitosID);
            return ResultadoOperacionMapper.ToDto(res);
        }
    }
}