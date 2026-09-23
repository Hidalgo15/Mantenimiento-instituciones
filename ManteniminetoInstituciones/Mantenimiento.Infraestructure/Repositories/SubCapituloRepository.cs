

using Mantenimiento.Core.Domain.Entities;
using Mantenimiento.Core.Domain.RepositoryInterfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

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
            throw new NotImplementedException();
        }

        public async Task EliminarConSpAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<SubCapitulo> GetByIdAsync(int id, int id_capitulo, string buscar)
        {
            var paramId = new SqlParameter("@id, @id_capitulo, @buscar", id);
            var result = await _context.Capitulos
                .FromSqlRaw("EXEC sp_subcapitulo_Obtener  @id, @id_capitulo, @buscar", paramId)
                .AsNoTracking()
                .ToListAsync();

            var subCapitulo = result.FirstOrDefault();
            if (subCapitulo == null)
            {
                throw new KeyNotFoundException($"No se encontró el capítulo con el ID {id}.");
            }

            return subCapitulo;
        }

        public async Task<int> InsertarConSpAsync(SubCapitulo subCapitulo)
        {
            throw new NotImplementedException();
        }

        public async Task<List<SubCapitulo>> ObtenerSubCapitulosAsync()
        {
            throw new NotImplementedException();
        }
    }
}
