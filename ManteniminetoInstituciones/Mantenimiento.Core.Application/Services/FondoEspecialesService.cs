using Mantenimiento.Core.Application.DTOs.Fondos;
using Mantenimiento.Core.Application.InterfaceServices;
using Mantenimiento.Core.Application.Mappings;
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

        public async Task<List<FondoDto>> ObtenerFondosEspecialesAsync(string? estructura = null, string? tipoTramite = null)
        {
            var lista = await _fondoRepo.GetAllFondosEspecialesAsync();
            var query = lista.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(estructura))
            {
                query = query.Where(f => string.Equals(
                    f.Estructura,
                    estructura,
                    StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace(tipoTramite))
            {
                query = query.Where(f => f.TipoTramite != null &&
                    f.TipoTramite.Equals(tipoTramite, StringComparison.OrdinalIgnoreCase));
            }

            return query.Select(FondoEspecialesMapper.ToDto).ToList();
        }

        public async Task<FondoDto?> ObtenerPorIdAsync(int id)
        {
            var entidad = await _fondoRepo.GetFondoEspecialByIdAsync(id);
            return entidad != null ? FondoEspecialesMapper.ToDto(entidad) : null;
        }

        public async Task CrearFondoEspecialAsync(CrearFondoDto dto)
        {
            int tipoTramiteId = dto.tipoTramiteContrato?.Id ?? 0;
            ValidarFondo(dto.Fondo);

            if (dto.IdInstitucion <= 0)
                throw new ArgumentException("Debe seleccionar una institución válida.");

            string tipoTramiteNombre = "Pendiente";

            if (tipoTramiteId > 0)
            {
                var tipoTramiteDto = await _tipoTramiteService.ObtenerPorIdAsync(tipoTramiteId);
                if (tipoTramiteDto != null && !string.IsNullOrWhiteSpace(tipoTramiteDto.Tipo))
                {
                    tipoTramiteNombre = tipoTramiteDto.Tipo;
                }
            }

            await _fondoRepo.CreateFondosEspecialesAsync(
                FondoEspecialesMapper.ToEntity(dto, tipoTramiteNombre),
                dto.IdInstitucion);
        }

        public async Task ActualizarFondoEspecialAsync(FondoDto dto)
        {
            int tipoTramiteId = dto.TipoTramiteContrato?.Id ?? 0;
            ValidarFondo(dto.Fondo);

            if (dto.IdInstitucion <= 0)
                throw new ArgumentException("Debe seleccionar una institución válida.");

            string tipoTramiteNombre = "Pendiente";

            if (tipoTramiteId > 0)
            {
                var tipoTramiteDto = await _tipoTramiteService.ObtenerPorIdAsync(tipoTramiteId);
                if (tipoTramiteDto != null && !string.IsNullOrWhiteSpace(tipoTramiteDto.Tipo))
                {
                    tipoTramiteNombre = tipoTramiteDto.Tipo;
                }
            }

            await _fondoRepo.UpdateFondosEspecialesAsync(
                FondoEspecialesMapper.ToEntity(dto, tipoTramiteNombre),
                dto.IdInstitucion);
        }

        public async Task EliminarFondoEspecialAsync(int id)
        {
            await _fondoRepo.DeleteFondosEspecialesAsync(id);
        }

        private static void ValidarFondo(string fondo)
        {
            if (string.IsNullOrWhiteSpace(fondo))
                throw new ArgumentException("El campo fondo es requerido.");

            if (int.TryParse(fondo, out var fondoValue) && fondoValue <= 0)
                throw new ArgumentException("El campo fondo debe ser un número positivo.");
        }
    }

}
