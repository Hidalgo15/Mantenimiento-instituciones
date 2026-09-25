

using Mantenimiento.Core.Domain.Entities;
using Mantenimiento.Core.Domain.RepositoryInterfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Data;

namespace Mantenimiento.Infraestructure.Persistence.Repositories
{
    public class SubCapituloRepository : ISubCapituloRepository
    {
        private readonly ApplicationDbContext _context;
        public SubCapituloRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task ActualizarConSpAsync(SubCapitulo subCapitulo)
        {
            var pId = new SqlParameter("@id", subCapitulo.Id);
            var pCodigo = new SqlParameter("@codigo_sub_capitulo", subCapitulo.CodigoSubCapitulo);
            var pNombre = new SqlParameter("@sub_capitulo", subCapitulo.subcapitulo ?? (object)DBNull.Value);
            var pCapitulo = new SqlParameter("@id_capitulo", subCapitulo.IdCapitulo);

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC sp_subcapitulo_Actualizar @id, @codigo_sub_capitulo, @sub_capitulo", "@id_capitulo",
                pId, pCodigo, pNombre, pCapitulo);
        }

        public async Task EliminarConSpAsync(int id)
        {
            var pId = new SqlParameter("@id", id);
            await _context.Database.ExecuteSqlRawAsync("EXEC sp_subcapitulo_Eliminar @id", pId);
        }

        public async Task<SubCapitulo> GetByIdAsync(int id, int id_capitulo, string? buscar)
        {
            var pId = new SqlParameter("@id", id);
            var pIdCapitulo = new SqlParameter("@id_capitulo", id_capitulo <= 0 ? (object)DBNull.Value : id_capitulo);
            var pBuscar = new SqlParameter("@buscar", string.IsNullOrWhiteSpace(buscar) ? (object)DBNull.Value : buscar);

            var result = await _context.SubCapitulos
                .FromSqlRaw("EXEC sp_subcapitulo_Obtener @id = @id, @id_capitulo = @id_capitulo, @buscar = @buscar", pId, pIdCapitulo, pBuscar)
                .AsNoTracking()
                .ToListAsync();

            var subCapitulo = result.FirstOrDefault();
            if (subCapitulo == null)
            {
                throw new KeyNotFoundException($"No se encontró el subcapítulo con el ID {id}.");
            }

            return subCapitulo;
        }

        public async Task<int> InsertarConSpAsync(SubCapitulo subCapitulo)
        {
            var pCodigo = new SqlParameter("@codigo_sub_capitulo", subCapitulo.CodigoSubCapitulo);
            var pNombre = new SqlParameter("@sub_capitulo", subCapitulo.subcapitulo ?? (object)DBNull.Value);
            var pIdGenerado = new SqlParameter
            {
                ParameterName = "@id_generado",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.Output
            };

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC dbo.sp_capitulo_Insertar @codigo_capitulo, @capitulo, @id_generado OUTPUT",
                pCodigo, pNombre, pIdGenerado);

            return (int)pIdGenerado.Value;
        }

        public async Task<List<SubCapitulo>> ObtenerSubCapitulosAsync(string? buscar = null)
        {
            var pId = new SqlParameter("@id", DBNull.Value);
            var pIdCapitulo = new SqlParameter("@id_capitulo", DBNull.Value);
            var pBuscar = new SqlParameter("@buscar", string.IsNullOrWhiteSpace(buscar) ? (object)DBNull.Value : buscar);

            return await _context.SubCapitulos
                .FromSqlRaw("EXEC sp_subcapitulo_Obtener @id = @id, @id_capitulo = @id_capitulo, @buscar = @buscar", pId, pIdCapitulo, pBuscar)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<SubCapitulo>> ObtenerSubCapitulosPorCapituloAsync(int capituloId)
        {
            var pId = new SqlParameter("@id", DBNull.Value);
            var pIdCapitulo = new SqlParameter("@id_capitulo", capituloId);
            var pBuscar = new SqlParameter("@buscar", DBNull.Value);

            return await _context.SubCapitulos
                .FromSqlRaw("EXEC sp_subcapitulo_Obtener @id = @id, @id_capitulo = @id_capitulo, @buscar = @buscar", pId, pIdCapitulo, pBuscar)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<SubCapitulo>> ObtenerSubCapitulosPorCodigoCapituloAsync(string codigoCapitulo)
        {
            var pId = new SqlParameter("@id", DBNull.Value);
            var pIdCapitulo = new SqlParameter("@id_capitulo", DBNull.Value);
            var pBuscar = new SqlParameter("@buscar", string.IsNullOrWhiteSpace(codigoCapitulo) ? (object)DBNull.Value : codigoCapitulo);

            return await _context.SubCapitulos
                .FromSqlRaw("EXEC sp_subcapitulo_Obtener @id = @id, @id_capitulo = @id_capitulo, @buscar = @buscar", pId, pIdCapitulo, pBuscar)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<SubCapitulo>> ObtenerSubCapitulosPorNombreCapituloAsync(string nombreCapitulo)
        {
            var pId = new SqlParameter("@id", DBNull.Value);
            var pIdCapitulo = new SqlParameter("@id_capitulo", DBNull.Value);
            var pBuscar = new SqlParameter("@buscar", string.IsNullOrWhiteSpace(nombreCapitulo) ? (object)DBNull.Value : nombreCapitulo);

            return await _context.SubCapitulos
                .FromSqlRaw("EXEC sp_subcapitulo_Obtener @id = @id, @id_capitulo = @id_capitulo, @buscar = @buscar", pId, pIdCapitulo, pBuscar)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<SubCapitulo>> ObtenerSubCapitulosPorCodigoSubCapituloAsync(string codigoSubCapitulo)
        {
            var pId = new SqlParameter("@id", DBNull.Value);
            var pIdCapitulo = new SqlParameter("@id_capitulo", DBNull.Value);
            var pBuscar = new SqlParameter("@buscar", string.IsNullOrWhiteSpace(codigoSubCapitulo) ? (object)DBNull.Value : codigoSubCapitulo);

            return await _context.SubCapitulos
                .FromSqlRaw("EXEC sp_subcapitulo_Obtener @id = @id, @id_capitulo = @id_capitulo, @buscar = @buscar", pId, pIdCapitulo, pBuscar)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<SubCapitulo>> ObtenerSubCapitulosPorNombreSubCapituloAsync(string nombreSubCapitulo)
        {
            var pId = new SqlParameter("@id", DBNull.Value);
            var pIdCapitulo = new SqlParameter("@id_capitulo", DBNull.Value);
            var pBuscar = new SqlParameter("@buscar", string.IsNullOrWhiteSpace(nombreSubCapitulo) ? (object)DBNull.Value : nombreSubCapitulo);

            return await _context.SubCapitulos
                .FromSqlRaw("EXEC sp_subcapitulo_Obtener @id = @id, @id_capitulo = @id_capitulo, @buscar = @buscar", pId, pIdCapitulo, pBuscar)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
