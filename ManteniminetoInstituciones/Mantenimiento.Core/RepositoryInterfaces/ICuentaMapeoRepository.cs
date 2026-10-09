using Mantenimiento.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mantenimiento.Core.Domain.RepositoryInterfaces
{
    public interface ICuentaMapeoRepository
    {
        Task<List<CuentaDbo>> ObtenerCuentasAsync(int? idCuenta = null, int? tipoTramite = null);
        Task<List<CuentaCt>> ObtenerCuentasBaseCatalogAsync();
        Task<CuentaDbo?> ObtenerPorIdAsync(int idCuenta);
        Task CrearCuentaAsync(int idCuentaBase, string? descripcion, int tipoTramite);
        Task ActualizarCuentaAsync(int idCuenta, string descripcion, int tipoTramite);
        Task EliminarCuentaAsync(int idCuenta);
    }
}
