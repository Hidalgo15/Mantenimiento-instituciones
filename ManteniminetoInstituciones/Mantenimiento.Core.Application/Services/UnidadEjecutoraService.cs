using FluentValidation;
using Mantenimiento.Core.Application.DTOs.UnidadEjecutora;
using Mantenimiento.Core.Application.InterfaceServices;
using Mantenimiento.Core.Application.Mappings;
using Mantenimiento.Core.Domain.RepositoryInterfaces;

namespace Mantenimiento.Core.Application.Services
{
    public class UnidadEjecutoraService : IUnidadEjecutoraService
    {
        private readonly IUnidadEjecutoraRepository _repo;
        private readonly IValidator<CrearUnidadEjecutoraDto> _crearValidator;
        private readonly IValidator<ActualizarUnidadEjecutoraDto> _actualizarValidator;

        public UnidadEjecutoraService(
            IUnidadEjecutoraRepository repo,
            IValidator<CrearUnidadEjecutoraDto> crearValidator,
            IValidator<ActualizarUnidadEjecutoraDto> actualizarValidator)
        {
            _repo = repo;
            _crearValidator = crearValidator;
            _actualizarValidator = actualizarValidator;
        }

        /// <summary>
        /// Obtiene una lista de unidades ejecutoras según los parámetros proporcionados.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="codigoUnidadEjecutora"></param>
        /// <param name="idDaf"></param>
        /// <returns></returns>
        public async Task<IEnumerable<UnidadEjecutoraDto>> ObtenerAsync(int? id = null, string? codigoUnidadEjecutora = null, int? idDaf = null)
        {
            var lista = await _repo.ObtenerAsync(id, codigoUnidadEjecutora, idDaf);
            return lista.Select(UnidadEjecutoraMapper.ToDto);
        }

        /// <summary>
        /// Obtiene una unidad ejecutora por su identificador.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<UnidadEjecutoraDto?> ObtenerPorIdAsync(int id)
        {
            if (id <= 0) return null;

            var res = await _repo.ObtenerAsync(id: id);
            var entidad = res.FirstOrDefault();
            return entidad != null ? UnidadEjecutoraMapper.ToDto(entidad) : null;
        }

        /// <summary>
        /// Crea una nueva unidad ejecutora en la base de datos.
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        /// <exception cref="FluentValidation.ValidationException">Se lanza cuando los datos de creación no son válidos.</exception>
        public async Task CrearAsync(CrearUnidadEjecutoraDto dto)
        {
            ArgumentNullException.ThrowIfNull(dto);
            await _crearValidator.ValidateAndThrowAsync(dto);

            var entidad = UnidadEjecutoraMapper.ToEntity(dto);

            await _repo.CrearConSpAsync(entidad, dto.Usuario);
        }

        /// <summary>
        /// Actualiza una unidad ejecutora existente en la base de datos.
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        /// <exception cref="FluentValidation.ValidationException">Se lanza cuando los datos de actualización no son válidos.</exception>
        public async Task ActualizarAsync(ActualizarUnidadEjecutoraDto dto)
        {
            ArgumentNullException.ThrowIfNull(dto);
            await _actualizarValidator.ValidateAndThrowAsync(dto);

            var entidad = UnidadEjecutoraMapper.ToEntity(dto);

            await _repo.ActualizarConSpAsync(entidad, dto.Usuario);
        }

        public async Task EliminarAsync(int id, string usuario)
        {
            if (id <= 0) throw new ArgumentException("El identificador a eliminar no es válido.");
            /// <summary>
            /// Elimina una unidad ejecutora de manera lógica. Si es true el registro se marca como eliminado, si es false se elimina físicamente.
            /// </summary>
            await _repo.EliminarConSpAsync(id, borradoLogico: true, usuario: usuario);
        }
    }
}