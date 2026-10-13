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

        public async Task<List<CuentaCtDto>> ObtenerCuentasAsync(string? busqueda = null)
        {
            var lista = await _repo.ObtenerCuentasAsync(busqueda);
            return lista.Select(CuentaCtMapper.ToDto).ToList();
        }

        public async Task<CuentaCtDto?> ObtenerPorIdAsync(int cuentaId)
        {
            var entidad = await _repo.ObtenerPorIdAsync(cuentaId);
            return entidad != null ? CuentaCtMapper.ToDto(entidad) : null;
        }

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

        public async Task EliminarCuentaAsync(int cuentaId)
        {
            if (cuentaId <= 0)
                throw new ArgumentException("ID de cuenta inválido para eliminar.");

            await _repo.EliminarCuentaAsync(cuentaId);
        }
    }
}
