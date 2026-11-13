using Mantenimiento.Core.Domain.Entities;
using Mantenimiento.Core.Domain.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mantenimiento.Core.Domain.RepositoryInterfaces
{
    public interface ICuentaCtRepository
    {
        Task<List<CuentaCt>> ObtenerCuentasAsync(string? busqueda = null);
        Task<PagedResult<CuentaCt>> ObtenerPaginadoAsync(string? busqueda, int pagina, int tamanoPagina);
        Task<CuentaCt?> ObtenerPorIdAsync(int cuentaId);
        Task CrearCuentaAsync(double? idCuenta, string codigoCuenta, string descripcionCuenta);
        Task ActualizarCuentaAsync(int cuentaId, double? idCuenta, string codigoCuenta, string descripcionCuenta);
        Task EliminarCuentaAsync(int cuentaId);
    }
}
