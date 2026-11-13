using FluentValidation;
using Mantenimiento.Core.Application.DTOs.RequisitoContrato;
using Mantenimiento.Core.Application.InterfaceServices;
using Mantenimiento.Core.Application.Mappings;
using Mantenimiento.Core.Domain.Pagination;
using Mantenimiento.Core.Domain.RepositoryInterfaces;

namespace Mantenimiento.Core.Application.Services
{
    public class RequisitoContratoService : IRequisitoContratoService
    {
        private readonly IRequisitoContratoRepository _repo;
        private readonly IValidator<CrearRequisitoContratoDto> _crearValidator;
        private readonly IValidator<ActualizarRequisitoContratoDto> _actualizarValidator;

        public RequisitoContratoService(
            IRequisitoContratoRepository repo,
            IValidator<CrearRequisitoContratoDto> crearValidator,
            IValidator<ActualizarRequisitoContratoDto> actualizarValidator)
        {
            _repo = repo;
            _crearValidator = crearValidator;
            _actualizarValidator = actualizarValidator;
        }

        /// <summary>
        /// Obtiene una lista de requisitos de contratos, opcionalmente filtrada por un término de búsqueda.
        /// </summary>
        public async Task<List<RequisitoContratoDto>> ObtenerRequisitosAsync(string? busqueda = null)
        {
            var lista = await _repo.ObtenerRequisitosAsync(busqueda);
            return lista.Select(RequisitoContratoMapper.ToDto).ToList();
        }

        public async Task<PagedResult<RequisitoContratoDto>> ObtenerPaginadoAsync(
            string? busqueda = null,
            int pagina = 1,
            int tamanoPagina = 10)
        {
            var resultado = await _repo.ObtenerPaginadoAsync(busqueda, pagina, tamanoPagina);
            return new PagedResult<RequisitoContratoDto>(
                resultado.Items.Select(RequisitoContratoMapper.ToDto).ToList(),
                resultado.PageNumber,
                resultado.PageSize,
                resultado.TotalCount);
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
            ArgumentNullException.ThrowIfNull(dto);
            await _crearValidator.ValidateAndThrowAsync(dto);

            var entidad = RequisitoContratoMapper.ToEntity(dto);
            return await _repo.InsertarConSpAsync(entidad);
        }

        /// <summary>
        /// Valida y actualiza un requisito de contrato existente.
        /// </summary>
        public async Task ActualizarRequisitoContratoAsync(ActualizarRequisitoContratoDto dto)
        {
            ArgumentNullException.ThrowIfNull(dto);
            await _actualizarValidator.ValidateAndThrowAsync(dto);

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

    }
}
