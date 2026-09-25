using Mantenimiento.Core.Domain.Entities;
using Mantenimiento.Core.Domain.RepositoryInterfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Mantenimiento.Infraestructure.Persistence.Repositories
{
    public class DafRepository : IDafRepository
    {
        private readonly ApplicationDbContext _context;

        public DafRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Daf>> ObtenerAsync(int? id = null, int? idSubCapitulo = null, string? codigoDaf = null)
        {
            var pId = new SqlParameter("@id", id.HasValue ? id.Value : DBNull.Value);
            var pIdSubCapitulo = new SqlParameter("@id_sub_capitulo", idSubCapitulo.HasValue ? idSubCapitulo.Value : DBNull.Value);
            var pCodigoDaf = new SqlParameter("@codigo_daf", string.IsNullOrWhiteSpace(codigoDaf) ? DBNull.Value : codigoDaf);

            return await _context.Dafs
                .FromSqlRaw("EXEC dbo.sp_daf_Obtener @id, @id_sub_capitulo, @codigo_daf", pId, pIdSubCapitulo, pCodigoDaf)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}