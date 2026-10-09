using Mantenimiento.Core.Application.DTOs.Cuenta;
using Mantenimiento.Core.Application.DTOs.CuentaInstitucion;
using Mantenimiento.Core.Application.InterfaceServices;
using Mantenimiento.Core.Domain.Entities;
using Mantenimiento.Core.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mantenimiento.Core.Application.Services
{
    public class CuentaMapeoService : ICuentaMapeoService
    {
        private readonly ICuentaMapeoRepository _repo;

        public CuentaMapeoService(ICuentaMapeoRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<CuentaDto>> ObtenerCuentasAsync(int? tipoTramite = null)
        {
            var lista = await _repo.ObtenerCuentasAsync(tipoTramite: tipoTramite);
            return lista.Select(MapToDto).ToList();
        }

        public async Task<List<CuentaCt>> ObtenerCatalogoBaseAsync()
        {
            return await _repo.ObtenerCuentasBaseCatalogAsync();
        }

        public async Task<CuentaDto?> ObtenerPorIdAsync(int idCuenta)
        {
            var entidad = await _repo.ObtenerPorIdAsync(idCuenta);
            return entidad != null ? MapToDto(entidad) : null;
        }

        public async Task CrearCuentaAsync(CrearCuentaDto dto)
        {
            if (dto.IdCuentaBase <= 0)
                throw new ArgumentException("Debe seleccionar una cuenta base válida.");

            if (dto.TipoTramite <= 0)
                throw new ArgumentException("Debe seleccionar un tipo de trámite de pago válido.");

            await _repo.CrearCuentaAsync(dto.IdCuentaBase, dto.DescripcionCuenta?.Trim(), dto.TipoTramite);
        }

        public async Task ActualizarCuentaAsync(ActualizarCuentaDto dto)
        {
            if (dto.IdCuenta <= 0)
                throw new ArgumentException("El ID de la cuenta no es válido.");

            if (string.IsNullOrWhiteSpace(dto.DescripcionCuenta))
                throw new ArgumentException("La descripción de la cuenta no puede estar vacía.");

            if (dto.TipoTramite <= 0)
                throw new ArgumentException("Debe seleccionar un tipo de trámite válido.");

            await _repo.ActualizarCuentaAsync(dto.IdCuenta, dto.DescripcionCuenta.Trim(), dto.TipoTramite);
        }

        public async Task EliminarCuentaAsync(int idCuenta)
        {
            if (idCuenta <= 0)
                throw new ArgumentException("ID de cuenta inválido para eliminar.");

            await _repo.EliminarCuentaAsync(idCuenta);
        }

        private static CuentaDto MapToDto(CuentaDbo c) => new()
        {
            IdCuenta = c.IdCuenta,
            CodigoCuenta = c.CodigoCuenta,
            DescripcionCuenta = c.DescripcionCuenta,
            TipoTramite = c.TipoTramiteId,
            TipoTramiteNombre = !string.IsNullOrWhiteSpace(c.TipoTramiteNombre)
        ? c.TipoTramiteNombre.Trim()
        : string.Empty
        };


    }
}