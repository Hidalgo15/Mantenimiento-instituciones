using FluentValidation;
using Mantenimiento.Core.Application.DTOs.AnalistaSupervisor;
using Mantenimiento.Core.Application.InterfaceServices;
using Mantenimiento.Core.Application.Mappings;
using Mantenimiento.Core.Domain.RepositoryInterfaces;

namespace Mantenimiento.Core.Application.Services
{
    public class AnalistaSupervisorService : IAnalistaSupervisorService
    {
        private readonly IAnalistaSupervisorRepository _repository;
        private readonly IValidator<AsignarSupervisorDto> _asignarValidator;
        private readonly IValidator<DesvincularAnalistaDto> _desvincularValidator;

        public AnalistaSupervisorService(
            IAnalistaSupervisorRepository repository,
            IValidator<AsignarSupervisorDto> asignarValidator,
            IValidator<DesvincularAnalistaDto> desvincularValidator)
        {
            _repository = repository;
            _asignarValidator = asignarValidator;
            _desvincularValidator = desvincularValidator;
        }

        /// <summary>
        /// Obtiene la lista de analistas con sus supervisores según los filtros proporcionados.
        /// </summary>
        /// <param name="nombreAnalista"></param>
        /// <param name="estadoAnalista"></param>
        /// <param name="despachoSupervisor"></param>
        /// <returns></returns>
        public async Task<List<AnalistaSupervisorConsultaDto>> ObtenerMantenimientoAsync(string? nombreAnalista, string? estadoAnalista, string? despachoSupervisor)
        {
            var entidades = await _repository.ObtenerMantenimientoAsync(nombreAnalista, estadoAnalista, despachoSupervisor);
            return entidades.Select(AnalistaSupervisorMapper.ToDto).ToList();
        }

        /// <summary>
        /// Obtiene de forma asincrónica una lista de objetos SupervisorOptionDto que representan los supervisores
        /// disponibles.
        /// </summary>
        /// <returns>Tarea que representa la operación asincrónica cuyo resultado es una lista de SupervisorOptionDto con los
        /// supervisores disponibles.</returns>
        public async Task<List<SupervisorOptionDto>> ObtenerSupervisoresDisponiblesAsync()
        {
            var entidades = await _repository.ObtenerSupervisoresDisponiblesAsync();
            return entidades.Select(AnalistaSupervisorMapper.ToDto).ToList();
        }

        /// <summary>
        /// Asigna un supervisor a un despacho de analista y persiste la asignación de forma asíncrona.
        /// </summary>
        /// <remarks>Valida la asignación y delega la persistencia al repositorio; recorta espacios en los
        /// identificadores de despacho.</remarks>
        /// <param name="dto">DTO que contiene los identificadores de despacho del analista y del supervisor.</param>
        /// <returns>Una tarea que representa la operación asíncrona.</returns>
        public async Task AsignarAsync(AsignarSupervisorDto dto)
        {
            ArgumentNullException.ThrowIfNull(dto);
            await _asignarValidator.ValidateAndThrowAsync(dto);
            await _repository.AsignarSupervisorAsync(dto.DespachoAnalista.Trim(), dto.DespachoSupervisor.Trim());
        }

        /// <summary>
        /// Reasigna el supervisor de un despacho de analista.
        /// </summary>
        /// <remarks>Valida la asignación y delega la operación al repositorio para realizar la
        /// reasignación.</remarks>
        /// <param name="dto">Objeto que contiene los identificadores del despacho de analista (DespachoAnalista) y del despacho del
        /// supervisor (DespachoSupervisor).</param>
        /// <returns>Tarea que representa la operación asincrónica.</returns>
        public async Task ReasignarAsync(AsignarSupervisorDto dto)
        {
            ArgumentNullException.ThrowIfNull(dto);
            await _asignarValidator.ValidateAndThrowAsync(dto);
            await _repository.ReasignarSupervisorAsync(dto.DespachoAnalista.Trim(), dto.DespachoSupervisor.Trim());
        }

        /// <summary>
        /// Desvincula un analista de su despacho.
        /// </summary>
        /// <remarks>Recorta espacios en los extremos del despacho y delega la eliminación al repositorio
        /// mediante EliminarAsignacionAsync.</remarks>
        /// <param name="dto">Información del despacho del analista a desvincular.</param>
        /// <returns>Una tarea que representa la operación asíncrona.</returns>
        /// <exception cref="ArgumentException">Se lanza si DespachoAnalista es nulo, vacío o contiene solo espacios en blanco.</exception>
        public async Task DesvincularAsync(DesvincularAnalistaDto dto)
        {
            ArgumentNullException.ThrowIfNull(dto);
            await _desvincularValidator.ValidateAndThrowAsync(dto);
            await _repository.EliminarAsignacionAsync(dto.DespachoAnalista.Trim());
        }
    }
}