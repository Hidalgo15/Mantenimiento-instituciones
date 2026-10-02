using AutoMapper;
using Mantenimiento.Core.Application.DTOs.AnalistaSupervisor;
using Mantenimiento.Core.Application.InterfaceServices;
using Mantenimiento.Core.Domain.RepositoryInterfaces;

namespace Mantenimiento.Core.Application.Services
{
    public class AnalistaSupervisorService : IAnalistaSupervisorService
    {
        private readonly IAnalistaSupervisorRepository _repository;
        private readonly IMapper _mapper;

        public AnalistaSupervisorService(IAnalistaSupervisorRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<AnalistaSupervisorConsultaDto>> ObtenerMantenimientoAsync(string? nombreAnalista, string? estadoAnalista, string? despachoSupervisor)
        {
            var entidades = await _repository.ObtenerMantenimientoAsync(nombreAnalista, estadoAnalista, despachoSupervisor);
            return _mapper.Map<List<AnalistaSupervisorConsultaDto>>(entidades);
        }

        public async Task<List<SupervisorOptionDto>> ObtenerSupervisoresDisponiblesAsync()
        {
            var entidades = await _repository.ObtenerSupervisoresDisponiblesAsync();
            return _mapper.Map<List<SupervisorOptionDto>>(entidades);
        }

        public async Task AsignarAsync(AsignarSupervisorDto dto)
        {
            ValidarAsignacion(dto.DespachoAnalista, dto.DespachoSupervisor);
            await _repository.AsignarSupervisorAsync(dto.DespachoAnalista.Trim(), dto.DespachoSupervisor.Trim());
        }

        public async Task ReasignarAsync(AsignarSupervisorDto dto)
        {
            ValidarAsignacion(dto.DespachoAnalista, dto.DespachoSupervisor);
            await _repository.ReasignarSupervisorAsync(dto.DespachoAnalista.Trim(), dto.DespachoSupervisor.Trim());
        }

        public async Task DesvincularAsync(DesvincularAnalistaDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.DespachoAnalista))
                throw new ArgumentException("El despacho del analista es obligatorio.");

            await _repository.EliminarAsignacionAsync(dto.DespachoAnalista.Trim());
        }

        private static void ValidarAsignacion(string analista, string supervisor)
        {
            if (string.IsNullOrWhiteSpace(analista))
                throw new ArgumentException("El código de despacho del analista es obligatorio.");

            if (string.IsNullOrWhiteSpace(supervisor))
                throw new ArgumentException("Debe seleccionar un supervisor.");
        }
    }
}