using Mantenimiento.Core.Application.DTOs.Fondos;
using Mantenimiento.Core.Application.InterfaceServices;
using Mantenimiento.Core.Domain.Entities;
using Mantenimiento.Core.Domain.RepositoryInterfaces;


namespace Mantenimiento.Core.Application.Services
{
    public class FondoEspecialesService : IFondoEspecialesService
    {
        private readonly IFondoEspecialesRepository _fondoRepo;
        private readonly ITipoTramiteService _tipoTramiteService;

        public FondoEspecialesService(
            IFondoEspecialesRepository fondoRepo,
            ITipoTramiteService tipoTramiteService)
        {
            _fondoRepo = fondoRepo;
            _tipoTramiteService = tipoTramiteService;
        }

        public async Task<List<FondoDto>> ObtenerFondosEspecialesAsync()
        {
            var lista = await _fondoRepo.GetAllFondosEspecialesAsync();
            return lista.Select(MapToDto).ToList();
        }

        public async Task<FondoDto> ObtenerPorIdAsync(int id)
        {
            var entidad = await _fondoRepo.GetFondoEspecialByIdAsync(id);
            return entidad != null ? MapToDto(entidad) : null!;
        }

        public async Task CrearFondoEspecialAsync(CrearFondoDto dto)
        {
            int tipoTramiteId = dto.TipoTramiteContrato?.Id ?? 0;
            ValidarFondo(dto.Fondo, tipoTramiteId);

            // Obtenemos el Tipo de Trámite a través del nuevo Servicio
            var tipoTramiteDto = await _tipoTramiteService.ObtenerPorIdAsync(tipoTramiteId);
            if (tipoTramiteDto == null)
                throw new ArgumentException("El tipo de trámite seleccionado no es válido.");

            var entidad = new FondosEspeciales
            {
                Fondo = dto.Fondo.Trim(),
                Descripcion = dto.Descripcion?.Trim(),
                Estructura = dto.Estructura?.Trim(),
                TipoTramiteContrato = new TipoTramiteContrato
                {
                    Id = tipoTramiteDto.Id,
                    TipoTramite = tipoTramiteDto.TipoTramite
                }
            };

            await _fondoRepo.CreateFondosEspecialesAsync(entidad);
        }

        public async Task ActualizarFondoEspecialAsync(FondoDto dto)
        {
            int tipoTramiteId = dto.TipoTramiteContrato?.Id ?? 0;
            ValidarFondo(dto.Fondo, tipoTramiteId);

            // Obtenemos el Tipo de Trámite a través del nuevo Servicio
            var tipoTramiteDto = await _tipoTramiteService.ObtenerPorIdAsync(tipoTramiteId);
            if (tipoTramiteDto == null)
                throw new ArgumentException("El tipo de trámite seleccionado no es válido.");

            var entidad = new FondosEspeciales
            {
                Id = dto.Id,
                Fondo = dto.Fondo.Trim(),
                Descripcion = dto.Descripcion?.Trim(),
                Estructura = dto.Estructura?.Trim(),
                TipoTramiteContrato = new TipoTramiteContrato
                {
                    Id = tipoTramiteDto.Id,
                    TipoTramite = tipoTramiteDto.TipoTramite
                }
            };

            await _fondoRepo.UpdateFondosEspecialesAsync(entidad);
        }

        public async Task EliminarFondoEspecialAsync(int id)
        {
            await _fondoRepo.DeleteFondosEspecialesAsync(id);
        }

        private static void ValidarFondo(string fondo, int? tipoTramiteId)
        {
            if (int.TryParse(fondo, out var fondoValue) && fondoValue <= 0)
                throw new ArgumentException("El campo debe de ser un número positivo.");
            if (string.IsNullOrWhiteSpace(fondo))
                throw new ArgumentException("El campo fondo es requerido.");
        }

        private static FondoDto MapToDto(FondosEspeciales f) => new()
        {
            Id = f.Id,
            Fondo = f.Fondo,
            Descripcion = f.Descripcion,
            Estructura = f.Estructura,
            TipoTramiteContrato = new TipoTramiteContrato
            {
                Id = f.TipoTramiteContrato?.Id ?? 0,
                TipoTramite = f.TipoTramiteContrato?.TipoTramite ?? string.Empty
            }
        };
    }
}

