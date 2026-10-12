using Mantenimiento.Core.Application.DTOs.TipoTramite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mantenimiento.Core.Application.InterfaceServices
{
    public interface ITipoTramitePagoService
    {
        Task<List<TipoTramiteDto>> ObtenerTodosAsync();
        Task<TipoTramiteDto?> ObtenerPorIdAsync(int id);
    }
}
