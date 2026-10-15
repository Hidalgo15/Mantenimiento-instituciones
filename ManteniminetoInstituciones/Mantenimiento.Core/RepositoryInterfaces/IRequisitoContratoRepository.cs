using Mantenimiento.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mantenimiento.Core.Domain.RepositoryInterfaces
{
    public interface IRequisitoContratoRepository
    {
        Task<List<RequisitoContrato>> ObtenerRequisitosAsync(string? busqueda = null);
        Task<RequisitoContrato> GetByIdAsync(int codigoRequisito);
        Task<int> InsertarConSpAsync(RequisitoContrato requisito);
        Task ActualizarConSpAsync(RequisitoContrato requisito);
        Task EliminarConSpAsync(int codigoRequisito);
    }
}
