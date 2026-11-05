using FluentValidation;
using Mantenimiento.Core.Application.DTOs.CuentaInstitucion;
using Mantenimiento.Core.Application.InterfaceServices;
using Mantenimiento.Core.Application.Mappings;
using Mantenimiento.Core.Domain.RepositoryInterfaces;

namespace Mantenimiento.Core.Application.Services
{
    public class CuentaInstitucionService : ICuentaInstitucionService
    {
        private readonly ICuentaInstitucionRepository _repo;
        private readonly IValidator<CrearCuentaInstitucionDto> _crearValidator;
        private readonly IValidator<ActualizarCuentaInstitucionDto> _actualizarValidator;

        public CuentaInstitucionService(
            ICuentaInstitucionRepository repo,
            IValidator<CrearCuentaInstitucionDto> crearValidator,
            IValidator<ActualizarCuentaInstitucionDto> actualizarValidator)
        {
            _repo = repo;
            _crearValidator = crearValidator;
            _actualizarValidator = actualizarValidator;
        }

        /// <summary>
        /// Obtiene de forma asíncrona las cuentas asociadas a la estructura de la institución identificada por el
        /// código proporcionado.
        /// </summary>
        /// <remarks>Los datos se obtienen del repositorio y se convierten a DTO mediante
        /// CuentaInstitucionMapper.</remarks>
        /// <param name="insCodigo">Código de la institución cuya estructura se consultará.</param>
        /// <returns>Una tarea que representa la operación asíncrona. El resultado contiene una lista de CuentaInstitucionDto con
        /// las cuentas asociadas.</returns>
        public async Task<List<CuentaInstitucionDto>> ObtenerCuentasPorEstructuraAsync(string insCodigo)
        {
            var cuentas = await _repo.ObtenerCuentasPorEstructuraAsync(insCodigo);
            return cuentas.Select(CuentaInstitucionMapper.ToDto).ToList();
        }

        /// <summary>
        /// Obtiene de forma asíncrona el DTO de una cuenta de institución por su identificador.
        /// </summary>
        /// <param name="id">Identificador de la cuenta a obtener.</param>
        /// <returns>Tarea que devuelve un CuentaInstitucionDto con los datos de la cuenta.</returns>
        /// <exception cref="KeyNotFoundException">Se lanza si no existe una cuenta con el identificador especificado.</exception>
        public async Task<CuentaInstitucionDto> ObtenerPorIdAsync(int id)
        {
            var entidad = await _repo.GetByIdAsync(id);
            if (entidad == null) throw new KeyNotFoundException("La cuenta no existe.");
            return CuentaInstitucionMapper.ToDto(entidad);
        }

        /// <summary>
        /// Crea de forma asíncrona una cuenta bancaria para la institución indicada.
        /// </summary>
        /// <param name="dto">DTO con los datos de la cuenta y el código de la institución.</param>
        /// <returns>Tarea que representa la operación asíncrona.</returns>
        /// <exception cref="InvalidOperationException">Se lanza si la cuenta bancaria ya está registrada para la institución.</exception>
        public async Task CrearCuentaAsync(CrearCuentaInstitucionDto dto)
        {
            ArgumentNullException.ThrowIfNull(dto);
            await _crearValidator.ValidateAndThrowAsync(dto);

            bool existe = await _repo.ExisteCuentaAsync(dto.InsCodigo, dto.CtaCtaBanco);
            if (existe)
                throw new InvalidOperationException("La cuenta bancaria ya se encuentra registrada para esta institución.");

            await _repo.InsertarConSpAsync(CuentaInstitucionMapper.ToEntity(dto));
        }

        /// <summary>
        /// Actualiza de forma asíncrona una cuenta de institución, validando el número de cuenta y aplicando los
        /// cambios en el repositorio.
        /// </summary>
        /// <remarks>Valida el formato del número de cuenta y delega la actualización al repositorio
        /// mediante un procedimiento almacenado.</remarks>
        /// <param name="dto">DTO con los datos necesarios para la actualización (incluye InsCodigo, CtaCtaBanco e Id).</param>
        /// <returns>Tarea que representa la operación asíncrona de actualización.</returns>
        /// <exception cref="InvalidOperationException">Se lanza si ya existe otra cuenta registrada con el mismo número para la institución indicada.</exception>
        public async Task ActualizarCuentaAsync(ActualizarCuentaInstitucionDto dto)
        {
            ArgumentNullException.ThrowIfNull(dto);
            await _actualizarValidator.ValidateAndThrowAsync(dto);

            bool existe = await _repo.ExisteCuentaAsync(dto.InsCodigo, dto.CtaCtaBanco, dto.Id);
            if (existe)
                throw new InvalidOperationException("Ya existe otra cuenta registrada con el mismo número para esta institución.");

            await _repo.ActualizarConSpAsync(CuentaInstitucionMapper.ToEntity(dto));
        }

        /// <summary>
        /// Elimina de forma asíncrona la cuenta con el identificador especificado.
        /// </summary>
        /// <remarks>La eliminación se realiza mediante el repositorio que ejecuta un procedimiento
        /// almacenado.</remarks>
        /// <param name="id">Identificador de la cuenta a eliminar.</param>
        /// <returns>Tarea que representa la operación asíncrona.</returns>
        public async Task EliminarCuentaAsync(int id)
        {
            await _repo.EliminarConSpAsync(id);
        }

    }
}