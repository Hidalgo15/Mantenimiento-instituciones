using Mantenimiento.Core.Application.DTOs.Cuenta;
using Mantenimiento.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mantenimiento.Core.Application.InterfaceServices
{
    public interface ICuentaMapeoService
    {
        Task ActualizarCuentaAsync(ActualizarCuentaDto dto);
        Task CrearCuentaAsync(CrearCuentaDto dto);
        Task EliminarCuentaAsync(int idCuenta);
        Task<List<CuentaCt>> ObtenerCatalogoBaseAsync();
        Task<CuentaDto?> ObtenerPorIdAsync(int idCuenta);
        Task<List<CuentaDto>> ObtenerCuentasAsync(int? tipoTramite = null);
    }
}
