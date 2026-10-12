using Mantenimiento.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mantenimiento.Core.Domain.RepositoryInterfaces
{
    public interface ITipoTramitePagoRepository
    {
        Task<List<TipoTramitePago>> ObtenerTodosAsync();
        Task<TipoTramitePago?> ObtenerPorIdAsync(int id);
    }
}
