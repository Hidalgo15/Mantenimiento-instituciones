using Mantenimiento.Core.Application.DTOs.CuentaInstitucion;
using Mantenimiento.Core.Application.InterfaceServices;
using Mantenimiento.Core.Application.Mappings;
using Mantenimiento.Core.Domain.RepositoryInterfaces;

namespace Mantenimiento.Core.Application.Services
{
    public class CuentaInstitucionService : ICuentaInstitucionService
    {
        private readonly ICuentaInstitucionRepository _repo;

        public CuentaInstitucionService(ICuentaInstitucionRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<CuentaInstitucionDto>> ObtenerCuentasPorEstructuraAsync(string insCodigo)
        {
            var cuentas = await _repo.ObtenerCuentasPorEstructuraAsync(insCodigo);
            return cuentas.Select(CuentaInstitucionMapper.ToDto).ToList();
        }

        public async Task<CuentaInstitucionDto> ObtenerPorIdAsync(int id)
        {
            var entidad = await _repo.GetByIdAsync(id);
            if (entidad == null) throw new KeyNotFoundException("La cuenta no existe.");
            return CuentaInstitucionMapper.ToDto(entidad);
        }

        public async Task CrearCuentaAsync(CrearCuentaInstitucionDto dto)
        {
            ValidarCuentaBanco(dto.CtaCtaBanco);

            bool existe = await _repo.ExisteCuentaAsync(dto.InsCodigo, dto.CtaCtaBanco);
            if (existe)
                throw new InvalidOperationException("La cuenta bancaria ya se encuentra registrada para esta institución.");

            await _repo.InsertarConSpAsync(CuentaInstitucionMapper.ToEntity(dto));
        }

        public async Task ActualizarCuentaAsync(ActualizarCuentaInstitucionDto dto)
        {
            ValidarCuentaBanco(dto.CtaCtaBanco);

            bool existe = await _repo.ExisteCuentaAsync(dto.InsCodigo, dto.CtaCtaBanco, dto.Id);
            if (existe)
                throw new InvalidOperationException("Ya existe otra cuenta registrada con el mismo número para esta institución.");

            await _repo.ActualizarConSpAsync(CuentaInstitucionMapper.ToEntity(dto));
        }

        public async Task EliminarCuentaAsync(int id)
        {
            await _repo.EliminarConSpAsync(id);
        }

        private static void ValidarCuentaBanco(string ctaCtaBanco)
        {
            if (string.IsNullOrWhiteSpace(ctaCtaBanco))
                throw new ArgumentException("El número de cuenta es requerido.");

            if (!ctaCtaBanco.All(char.IsDigit))
                throw new ArgumentException("La cuenta de banco debe contener únicamente números.");
        }
    }
}