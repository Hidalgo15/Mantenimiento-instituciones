using Mantenimiento.Core.Domain.Entities;
using Mantenimiento.Core.Domain.RepositoryInterfaces.GenericRepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mantenimiento.Core.Domain.RepositoryInterfaces
{
    public interface ICuentaInstitucionRepository : IGenericRepository<CuentaInstitucion>
    {
        // Métodos específicos que no están en IGenericRepository
        Task<List<Institucion>> ObtenerInstitucionesAsync();
        Task<List<CuentaInstitucion>> ObtenerCuentasPorEstructuraAsync(string insCodigo);

        // Si usas los métodos personalizados con SP:
        Task<int> InsertarConSpAsync(CuentaInstitucion cuenta);
        Task ActualizarConSpAsync(CuentaInstitucion cuenta);
        Task EliminarConSpAsync(int id);
    }
}
