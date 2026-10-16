using Mantenimiento.Core.Application.DTOs.Cuenta.CtCuentasDTO;
using Mantenimiento.Core.Application.InterfaceServices;
using Mantenimiento.Core.Application.Mappings;
using Mantenimiento.Core.Domain.RepositoryInterfaces;

namespace Mantenimiento.Core.Application.Services
{
    public class CuentaCtService : ICuentaCtService
    {
        private readonly ICuentaCtRepository _repo;

        public CuentaCtService(ICuentaCtRepository repo)
        {
            _repo = repo;
        }

        /// <summary>
        /// Obtiene de forma asíncrona las cuentas y las devuelve como DTOs de tipo CuentaCtDto.
        /// </summary>
        /// <remarks>Los datos se obtienen del repositorio y se convierten a DTO mediante
        /// CuentaCtMapper.</remarks>
        /// <param name="busqueda">Filtro de búsqueda opcional que restringe los resultados; nulo devuelve todas las cuentas.</param>
        /// <returns>Una tarea que al completarse contiene la lista de cuentas como List<CuentaCtDto>.</returns>
        public async Task<List<CuentaCtDto>> ObtenerCuentasAsync(string? busqueda = null)
        {
            var lista = await _repo.ObtenerCuentasAsync(busqueda);
            return lista.Select(CuentaCtMapper.ToDto).ToList();
        }

        /// <summary>
        /// Obtiene de forma asíncrona la cuenta con el identificador especificado.
        /// </summary>
        /// <remarks>Consulta el repositorio para recuperar la entidad y la mapea a CuentaCtDto.</remarks>
        /// <param name="cuentaId">Identificador de la cuenta.</param>
        /// <returns>Tarea que contiene el CuentaCtDto de la cuenta si existe; en caso contrario, null.</returns>
        public async Task<CuentaCtDto?> ObtenerPorIdAsync(int cuentaId)
        {
            var entidad = await _repo.ObtenerPorIdAsync(cuentaId);
            return entidad != null ? CuentaCtMapper.ToDto(entidad) : null;
        }
        
        /// <summary>
        /// Crea una nueva cuenta de forma asíncrona.
        /// </summary>
        /// <param name="dto">DTO con los datos de la cuenta a crear.</param>
        /// <returns>Una tarea que representa la operación asíncrona.</returns>
        public async Task CrearCuentaAsync(CrearCuentaCtDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.CodigoCuenta))
                throw new ArgumentException("El código de cuenta es obligatorio.");

            if (string.IsNullOrWhiteSpace(dto.DescripcionCuenta))
                throw new ArgumentException("La descripción de la cuenta es obligatoria.");

            await _repo.CrearCuentaAsync(
                dto.IdCuenta,
                dto.CodigoCuenta.Trim(),
                dto.DescripcionCuenta.Trim()
            );
        }

        /// <summary>
        /// Actualiza una cuenta existente tras validar CuentaID, CodigoCuenta y DescripcionCuenta, y delega la
        /// operación al repositorio de datos.
        /// </summary>
        /// <remarks>CodigoCuenta y DescripcionCuenta se recortan (Trim) antes de la actualización; la
        /// operación se realiza mediante el repositorio.</remarks>
        /// <param name="dto">Objeto con los datos de actualización (CuentaID, IdCuenta, CodigoCuenta, DescripcionCuenta).</param>
        /// <returns>Una tarea que representa la operación asincrónica.</returns>
        /// <exception cref="ArgumentException">Se lanza cuando CuentaID es menor o igual a cero, o cuando CodigoCuenta o DescripcionCuenta son nulos,
        /// vacíos o contienen solo espacios en blanco.</exception>
        public async Task ActualizarCuentaAsync(ActualizarCuentaCtDto dto)
        {
            if (dto.CuentaID <= 0)
                throw new ArgumentException("El ID de la cuenta no es válido.");

            if (string.IsNullOrWhiteSpace(dto.CodigoCuenta))
                throw new ArgumentException("El código de cuenta no puede estar vacío.");

            if (string.IsNullOrWhiteSpace(dto.DescripcionCuenta))
                throw new ArgumentException("La descripción de la cuenta no puede estar vacía.");

            await _repo.ActualizarCuentaAsync(
                dto.CuentaID,
                dto.IdCuenta,
                dto.CodigoCuenta.Trim(),
                dto.DescripcionCuenta.Trim()
            );
        }

        /// <summary>
        /// Elimina de forma asíncrona la cuenta con el identificador especificado.
        /// </summary>
        /// <param name="cuentaId">Identificador de la cuenta a eliminar. Debe ser mayor que cero.</param>
        /// <returns>Tarea que representa la operación asíncrona.</returns>
        /// <exception cref="ArgumentException">Si cuentaId es menor o igual que cero.</exception>
        public async Task EliminarCuentaAsync(int cuentaId)
        {
            if (cuentaId <= 0)
                throw new ArgumentException("ID de cuenta inválido para eliminar.");

            await _repo.EliminarCuentaAsync(cuentaId);
        }
    }
}
