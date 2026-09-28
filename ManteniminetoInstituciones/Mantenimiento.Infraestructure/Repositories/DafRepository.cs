using Mantenimiento.Core.Domain.Entities;
using Mantenimiento.Core.Domain.RepositoryInterfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Mantenimiento.Infraestructure.Persistence.Repositories
{
    public class DafRepository : IDafRepository
    {
        private readonly ApplicationDbContext _context;

        public DafRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Daf> ActualizarAsync(Daf daf)
        {
            var pId = new SqlParameter("@id", SqlDbType.Int)
            {
                Value = daf.Id
            };

            var pIdSubCapitulo = new SqlParameter("@id_sub_capitulo", SqlDbType.Int)
            {
                Value = daf.IdSubCapitulo
            };

            var pCodigoDaf = new SqlParameter("@codigo_daf", SqlDbType.VarChar, 10)
            {
                Value = string.IsNullOrWhiteSpace(daf.CodigoDaf) ? (object)DBNull.Value : daf.CodigoDaf
            };

            var pNombreDaf = new SqlParameter("@daf", SqlDbType.VarChar, 200)
            {
                Value = string.IsNullOrWhiteSpace(daf.NombreDaf) ? (object)DBNull.Value : daf.NombreDaf
            };

            // Pasar los SqlParameter directamente como en los demás métodos
            var result = await _context.Dafs
                .FromSqlRaw("EXEC dbo.sp_daf_Actualizar @id, @id_sub_capitulo, @codigo_daf, @daf", pId, pIdSubCapitulo, pCodigoDaf, pNombreDaf)
                .AsNoTracking()
                .ToListAsync();

            return result.FirstOrDefault();
        }

        public async Task<Daf> CrearAsync(Daf daf)
        {
            var pIdSubCapitulo = new SqlParameter("@id_sub_capitulo", SqlDbType.Int)
            {
                Value = daf.IdSubCapitulo > 0 ? daf.IdSubCapitulo : DBNull.Value
            };
            var pCodigoDaf = new SqlParameter("@codigo_daf", SqlDbType.VarChar, 10) { Value = daf.CodigoDaf };
            var pNombreDaf = new SqlParameter("@daf", SqlDbType.VarChar, 200) { Value = daf.NombreDaf };
            var pIdGenerado = new SqlParameter
            {
                ParameterName = "@id_generado",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.Output
            };

            // CAMBIO CLAVE: Usar marcadores posicionales
            await _context.Database.ExecuteSqlRawAsync(
                "EXEC dbo.sp_daf_Crear {0}, {1}, {2}, {3} OUT",
                pIdSubCapitulo, pCodigoDaf, pNombreDaf, pIdGenerado
            );

            if (pIdGenerado.Value != DBNull.Value)
            {
                daf.Id = (int)pIdGenerado.Value;
            }

            return daf;
        }

        public async Task<bool> EliminarAsync(int id)
        {
            var pId = new SqlParameter("@id", id);
            await _context.Database.ExecuteSqlRawAsync("EXEC sp_daf_Eliminar @id", pId);
            return true;
        }

        public async Task<IEnumerable<Daf>> ObtenerAsync(int? id = null, int? idSubCapitulo = null, string? codigoDaf = null)
        {
            var pId = new SqlParameter("@id", SqlDbType.Int) { Value = id.HasValue ? id.Value : DBNull.Value };
            var pIdSubCapitulo = new SqlParameter("@id_sub_capitulo", SqlDbType.Int) { Value = idSubCapitulo.HasValue ? idSubCapitulo.Value : DBNull.Value };
            var pCodigoDaf = new SqlParameter("@codigo_daf", string.IsNullOrWhiteSpace(codigoDaf) ? DBNull.Value : codigoDaf);

            return await _context.Dafs
                .FromSqlRaw("EXEC dbo.sp_daf_Obtener @id, @id_sub_capitulo, @codigo_daf", pId, pIdSubCapitulo, pCodigoDaf)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Daf>> ObtenerPorCodigoDafAsync(string? codigoDaf)
        {
            var pId = new SqlParameter("@id", SqlDbType.Int) { Value = DBNull.Value };
            var pIdSubCapitulo = new SqlParameter("@id_sub_capitulo", SqlDbType.Int) { Value = DBNull.Value };

            var pCodigoDaf = new SqlParameter("@codigo_daf", string.IsNullOrWhiteSpace(codigoDaf) ? DBNull.Value : codigoDaf);

            return await _context.Dafs
                .FromSqlRaw("EXEC dbo.sp_daf_Obtener @id,@id_sub_capitulo, @codigo_daf", pId, pIdSubCapitulo, pCodigoDaf)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Daf> ObtenerPorIdAsync(int id, int? idSubCapitulo = null, string? codigoDaf = null)
        {
            var pId = new SqlParameter("@id", SqlDbType.Int) { Value = id };
            var pIdSubCapitulo = new SqlParameter("@id_sub_capitulo", SqlDbType.Int)
            {
                Value = (idSubCapitulo.HasValue && idSubCapitulo.Value > 0) ? (object)idSubCapitulo.Value : DBNull.Value
            };
            var pCodigoDaf = new SqlParameter("@codigo_daf", SqlDbType.VarChar, 10)
            {
                Value = string.IsNullOrWhiteSpace(codigoDaf) ? (object)DBNull.Value : codigoDaf
            };

            var result = await _context.Dafs
                .FromSqlRaw("EXEC dbo.sp_daf_Obtener @id, @id_sub_capitulo, @codigo_daf", pId, pIdSubCapitulo, pCodigoDaf)
                .AsNoTracking()
                .ToListAsync();

            var subCapitulo = result.FirstOrDefault();
            if (subCapitulo == null)
            {
                throw new KeyNotFoundException($"No se encontró el subcapítulo con el ID {id}.");
            }

            return subCapitulo;
        }

        public async Task<IEnumerable<Daf>> ObtenerPorSubCapituloAsync(int? idSubCapitulo)
        {
            var pId = new SqlParameter("@id", SqlDbType.Int) { Value = DBNull.Value };
            var pIdSubCapitulo = new SqlParameter("@id_sub_capitulo", SqlDbType.Int) { Value = idSubCapitulo.HasValue ? idSubCapitulo.Value : DBNull.Value };
            var pCodigoDaf = new SqlParameter("@codigo_daf", DBNull.Value);

            return await _context.Dafs
                .FromSqlRaw("EXEC dbo.sp_daf_Obtener @id, @id_sub_capitulo, @codigo_daf", pId, pIdSubCapitulo, pCodigoDaf)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}