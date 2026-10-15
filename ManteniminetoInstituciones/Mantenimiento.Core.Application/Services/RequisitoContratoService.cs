using Mantenimiento.Core.Application.DTOs.RequisitoContrato;
using Mantenimiento.Core.Application.InterfaceServices;
using Mantenimiento.Core.Application.Mappings;
using Mantenimiento.Core.Domain.RepositoryInterfaces;

namespace Mantenimiento.Core.Application.Services
{
    public class RequisitoContratoService : IRequisitoContratoService
    {
        private readonly IRequisitoContratoRepository _repo;

        public RequisitoContratoService(IRequisitoContratoRepository repo)
        {
            _repo = repo;
        }

        /// <summary>
        /// Obtiene una lista de requisitos de contratos, opcionalmente filtrada por un término de búsqueda.
        /// </summary>
        public async Task<List<RequisitoContratoDto>> ObtenerRequisitosAsync(string? busqueda = null)
        {
            var lista = await _repo.ObtenerRequisitosAsync(busqueda);
            return lista.Select(RequisitoContratoMapper.ToDto).ToList();
        }

        /// <summary>
        /// Obtiene un requisito de contrato por su código único.
        /// </summary>
        public async Task<RequisitoContratoDto> ObtenerPorIdAsync(int codigoRequisito)
        {
            var entidad = await _repo.GetByIdAsync(codigoRequisito);
            return RequisitoContratoMapper.ToDto(entidad);
        }

        /// <summary>
        /// Valida y crea un nuevo requisito de contrato en la base de datos.
        /// </summary>
        public async Task<int> CrearRequisitoContratoAsync(CrearRequisitoContratoDto dto)
        {
            ValidarRequisito(dto.DescripcionRequisito);

            var entidad = RequisitoContratoMapper.ToEntity(dto);
            return await _repo.InsertarConSpAsync(entidad);
        }

        /// <summary>
        /// Valida y actualiza un requisito de contrato existente.
        /// </summary>
        public async Task ActualizarRequisitoContratoAsync(ActualizarRequisitoContratoDto dto)
        {
            if (dto.CodigoRequisito <= 0)
                throw new ArgumentException("El código del requisito no es válido para actualizar.");

            ValidarRequisito(dto.DescripcionRequisito);

            var entidad = RequisitoContratoMapper.ToEntity(dto);
            await _repo.ActualizarConSpAsync(entidad);
        }

        /// <summary>
        /// Elimina un requisito de contrato por su código.
        /// </summary>
        public async Task EliminarRequisitoContratoAsync(int codigoRequisito)
        {
            if (codigoRequisito <= 0)
                throw new ArgumentException("Código de requisito inválido para eliminar.");

            await _repo.EliminarConSpAsync(codigoRequisito);
        }

        /// <summary>
        /// Valida las reglas de negocio básicas para el requisito de contrato.
        /// </summary>
        private static void ValidarRequisito(string? descripcion)
        {
            if (string.IsNullOrWhiteSpace(descripcion))
                throw new ArgumentException("La descripción del requisito es requerida.");

            if (descripcion.Trim().Length > 300)
                throw new ArgumentException("La descripción del requisito no puede exceder los 300 caracteres.");
        }
    }
}
