using Mantenimiento.Core.Application.DTOs.CuentaInstitucion;
using Mantenimiento.Core.Application.InterfaceServices;
using Mantenimiento.Core.Domain.Entities;
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

        public async Task<List<InstitucionSeleccionDto>> ObtenerInstitucionesSelectorAsync()
        {
            var lista = await _repo.ObtenerInstitucionesAsync();
            return lista
                .Where(x => x.Estatus)
                .Select(x => new InstitucionSeleccionDto
                {
                    Estructura = x.Estructura,
                    UnidadEjecutora = x.UnidadEjecutora
                })
                .DistinctBy(x => x.Estructura)
                .ToList();
        }

        public async Task<List<CuentaInstitucionDto>> ObtenerCuentasPorEstructuraAsync(string insCodigo)
        {
            var cuentas = await _repo.ObtenerCuentasPorEstructuraAsync(insCodigo);
            return cuentas.Select(MapToDto).ToList();
        }

        public async Task<CuentaInstitucionDto> ObtenerPorIdAsync(int id)
        {
            var entidad = await _repo.GetByIdAsync(id);
            if (entidad == null) throw new KeyNotFoundException("La cuenta no existe.");
            return MapToDto(entidad);
        }

        public async Task CrearCuentaAsync(CrearCuentaInstitucionDto dto)
        {
            ValidarCuentaBanco(dto.CtaCtaBanco);

            bool existe = await _repo.ExisteCuentaAsync(dto.InsCodigo, dto.CtaCtaBanco);
            if (existe)
                throw new InvalidOperationException("La cuenta bancaria ya se encuentra registrada para esta institución.");

            var entidad = new CuentaInstitucion
            {
                InsCodigo = dto.InsCodigo,
                Institucion = dto.Institucion,
                CtaCtaBanco = dto.CtaCtaBanco,
                CtaDescripcion = dto.CtaDescripcion
            };

            await _repo.InsertarConSpAsync(entidad);
        }

        public async Task ActualizarCuentaAsync(ActualizarCuentaInstitucionDto dto)
        {
            ValidarCuentaBanco(dto.CtaCtaBanco);

            bool existe = await _repo.ExisteCuentaAsync(dto.InsCodigo, dto.CtaCtaBanco, dto.Id);
            if (existe)
                throw new InvalidOperationException("Ya existe otra cuenta registrada con el mismo número para esta institución.");

            var entidad = new CuentaInstitucion
            {
                Id = dto.Id,
                InsCodigo = dto.InsCodigo,
                Institucion = dto.Institucion,
                CtaCtaBanco = dto.CtaCtaBanco,
                CtaDescripcion = dto.CtaDescripcion
            };

            await _repo.ActualizarConSpAsync(entidad);
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

        private static CuentaInstitucionDto MapToDto(CuentaInstitucion c) => new()
        {
            Id = c.Id ?? 0,
            InsCodigo = c.InsCodigo,
            Institucion = c.Institucion,
            CtaCtaBanco = c.CtaCtaBanco,
            CtaDescripcion = c.CtaDescripcion
        };
    }
}