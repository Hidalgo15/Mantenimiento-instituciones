using Mantenimiento.Core.Domain.Entities;

namespace Mantenimiento.Core.Domain.RepositoryInterfaces
{
    public interface ICuentaInstitucionRepository
    {
        Task<CuentaInstitucion> GetByIdAsync(int id);
        Task<List<Institucion>> ObtenerInstitucionesAsync();
        Task<List<CuentaInstitucion>> ObtenerCuentasPorEstructuraAsync(string insCodigo);
        Task<bool> ExisteCuentaAsync(string insCodigo, string ctaCtaBanco, int? idExcluir = null);
        Task<int> InsertarConSpAsync(CuentaInstitucion cuenta);
        Task ActualizarConSpAsync(CuentaInstitucion cuenta);
        Task EliminarConSpAsync(int id);
    }
}