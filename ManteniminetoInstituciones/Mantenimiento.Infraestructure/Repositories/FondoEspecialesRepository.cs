using Mantenimiento.Core.Domain.Entities;
using Mantenimiento.Core.Domain.RepositoryInterfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;


namespace Mantenimiento.Infraestructure.Persistence.Repositories
{
    public class FondoEspecialesRepository : IFondoEspecialesRepository
    {
        private readonly ApplicationDbContext _context;

        public FondoEspecialesRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<FondosEspeciales>> GetAllFondosEspecialesAsync()
        {
            /*
            var pFondo = new SqlParameter("@filtroFondo", DBNull.Value);
            var pEstructura = new SqlParameter("@filtroEstructura", DBNull.Value);

            return await _context.FondosEspeciales
                .FromSqlRaw("EXEC [dbo].[sp_FondoEspeciales_Obtener] @filtroFondo, @filtroEstructura", pFondo, pEstructura)
                .AsNoTracking()
                .ToListAsync();

            */

            return await _context.FondosEspeciales
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<FondosEspeciales> GetFondoEspecialByIdAsync(int id)
        {
            /*
            var pId = new SqlParameter("@Id", id);

            var resultado = await _context.FondosEspeciales
                .FromSqlRaw("SELECT [id], " +
                "[Fondo], " +
                "[estructura_institucion], " +
                "[descripcion], " +
                "[tipo_tramite] FROM [dbo].[Fondo_especiales] WHERE [id] = @Id", pId)
                .AsNoTracking()
                .ToListAsync();
            
            return resultado.FirstOrDefault()!;
            */

            return await _context.FondosEspeciales
            .AsNoTracking()
            .FirstOrDefaultAsync(f => f.Id == id);
        }


        public async Task<FondosEspeciales> CreateFondosEspecialesAsync(FondosEspeciales fondoEspecial, int idInstitucion)
        {
            var pIdInstitucion = new SqlParameter("@id_institucion", idInstitucion);
            var pFondo = new SqlParameter("@fondo", (object?)fondoEspecial.Fondo ?? DBNull.Value);
            var pTipoTramite = new SqlParameter("@tipo_tramite", (object?)fondoEspecial.TipoTramite ?? DBNull.Value);

            // USAR _context.Database.ExecuteSqlRawAsync
            await _context.Database.ExecuteSqlRawAsync(
                "EXEC [dbo].[sp_FondoEspeciales_Insertar] @id_institucion, @fondo, @tipo_tramite",
                pIdInstitucion, pFondo, pTipoTramite);

            return fondoEspecial;
        }

        public async Task<FondosEspeciales> UpdateFondosEspecialesAsync(FondosEspeciales fondoEspecial, int idInstitucion)
        {
            var pId = new SqlParameter("@id", fondoEspecial.Id);
            var pIdInstitucion = new SqlParameter("@id_institucion", idInstitucion);
            var pFondo = new SqlParameter("@fondo", (object?)fondoEspecial.Fondo ?? DBNull.Value);
            var pTipoTramite = new SqlParameter("@tipo_tramite", (object?)fondoEspecial.TipoTramite ?? DBNull.Value);

            // USAR _context.Database.ExecuteSqlRawAsync
            await _context.Database.ExecuteSqlRawAsync(
                "EXEC [dbo].[sp_FondoEspeciales_Actualizar] @id, @id_institucion, @fondo, @tipo_tramite",
                pId, pIdInstitucion, pFondo, pTipoTramite);

            return fondoEspecial;
        }

        public async Task DeleteFondosEspecialesAsync(int id)
        {
            var pId = new SqlParameter("@id", id);

            await _context.Database.ExecuteSqlRawAsync(
                "DELETE FROM [dbo].[Fondo_especiales] WHERE [Id] = @id", pId);
        }


    }
}
