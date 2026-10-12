using Mantenimiento.Core.Application.DTOs.Cuenta.CtCuentasDTO;
using Mantenimiento.Core.Application.DTOs.Cuenta.DboCuentasDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mantenimiento.Core.Application.InterfaceServices
{
    public interface ICuentaCtService
    {
        Task<List<CuentaCtDto>> ObtenerCuentasAsync(string? busqueda = null);
        Task<CuentaCtDto?> ObtenerPorIdAsync(int cuentaId);
        Task CrearCuentaAsync(CrearCuentaCtDto dto);
        Task ActualizarCuentaAsync(ActualizarCuentaCtDto dto);
        Task EliminarCuentaAsync(int cuentaId);
    }
}
