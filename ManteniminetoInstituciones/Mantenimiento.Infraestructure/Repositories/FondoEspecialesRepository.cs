using Mantenimiento.Core.Domain.Entities;
using Mantenimiento.Core.Domain.Pagination;
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

        public async Task<List<FondosEspeciales>> GetFondosEspecialesAsync(
            string? filtroFondo,
            string? estructura,
            string? tipoTramite)
        {
            var parametroFondo = CrearParametroTexto("@filtroFondo", filtroFondo, 50);
            var parametroEstructura = CrearParametroTexto("@filtroEstructura", estructura, 50);
            var parametroTipoTramite = CrearParametroTexto("@filtroTipoTramite", tipoTramite, 100);

            return await _context.FondosEspeciales
                .FromSqlRaw(
                    "EXEC dbo.sp_FondoEspeciales_Obtener @filtroFondo, @filtroEstructura, @filtroTipoTramite",
                    parametroFondo, parametroEstructura, parametroTipoTramite)
                .AsNoTracking()
                .ToListAsync();
        }

        /// <summary>
        /// Obtiene una página de fondos especiales, aplicando los filtros y el conteo en la base de datos.
        /// </summary>
        public async Task<PagedResult<FondosEspeciales>> GetFondosEspecialesPaginadosAsync(
            string? filtroFondo,
            string? estructura,
            string? tipoTramite,
            int pageNumber,
            int pageSize)
        {
            pageNumber = Math.Max(pageNumber, 1);
            pageSize = Math.Clamp(pageSize, 1, 100);
            var parametroFondo = CrearParametroTexto("@filtroFondo", filtroFondo, 50);
            var parametroEstructura = CrearParametroTexto("@filtroEstructura", estructura, 50);
            var parametroTipoTramite = CrearParametroTexto("@filtroTipoTramite", tipoTramite, 100);
            var parametroPagina = new SqlParameter("@PageNumber", SqlDbType.Int) { Value = pageNumber };
            var parametroTamanoPagina = new SqlParameter("@PageSize", SqlDbType.Int) { Value = pageSize };
            var parametroTotal = new SqlParameter("@TotalCount", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };

            var fondos = await _context.FondosEspeciales
                .FromSqlRaw(
                    "EXEC dbo.sp_FondoEspeciales_ObtenerPaginado @filtroFondo, @filtroEstructura, @filtroTipoTramite, @PageNumber, @PageSize, @TotalCount OUTPUT",
                    parametroFondo, parametroEstructura, parametroTipoTramite, parametroPagina, parametroTamanoPagina, parametroTotal)
                .AsNoTracking()
                .ToListAsync();

            var totalCount = Convert.ToInt32(parametroTotal.Value);
            var totalPages = totalCount == 0 ? 1 : (int)Math.Ceiling(totalCount / (double)pageSize);
            pageNumber = Math.Min(pageNumber, totalPages);
            return new PagedResult<FondosEspeciales>(fondos, pageNumber, pageSize, totalCount);
        }

        /// <summary>
        /// Obtiene de forma asíncrona un FondosEspeciales por su identificador.
        /// </summary>
        /// <remarks>La consulta se realiza con AsNoTracking para evitar el seguimiento de la entidad por
        /// el contexto de EF Core.</remarks>
        /// <param name="id">Identificador único del fondo especial.</param>
        /// <returns>Una tarea que representa la operación asincrónica. El resultado contiene el FondosEspeciales encontrado, o
        /// null si no existe.</returns>
        public async Task<FondosEspeciales?> GetFondoEspecialByIdAsync(int id)
        {
            return await _context.FondosEspeciales
            .AsNoTracking()
            .FirstOrDefaultAsync(f => f.Id == id);
        }

        /// <summary>
        /// Crea un registro de fondos especiales en la base de datos ejecutando el procedimiento almacenado
        /// correspondiente.
        /// </summary>
        /// <remarks>Se ejecuta el procedimiento almacenado 'sp_FondoEspeciales_Insertar' mediante
        /// ExecuteSqlRawAsync; no se recuperan valores generados por la base de datos.</remarks>
        /// <param name="fondoEspecial">Instancia con los valores del fondo especial a insertar; las propiedades nulas se envían como DBNull.Value.</param>
        /// <param name="idInstitucion">Identificador de la institución asociada al fondo.</param>
        /// <returns>La misma instancia de FondosEspeciales proporcionada.</returns>
        public async Task<FondosEspeciales> CreateFondosEspecialesAsync(FondosEspeciales fondoEspecial, int idInstitucion)
        {
            var pIdInstitucion = new SqlParameter("@id_institucion", idInstitucion);
            var pFondo = new SqlParameter("@fondo", (object?)fondoEspecial.Fondo ?? DBNull.Value);
            var pDescripcion = new SqlParameter("@descripcion", (object?)fondoEspecial.Descripcion ?? DBNull.Value);
            var pTipoTramite = new SqlParameter("@tipo_tramite", (object?)fondoEspecial.TipoTramite ?? DBNull.Value);

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC [dbo].[sp_FondoEspeciales_Insertar] @id_institucion, @fondo, @descripcion, @tipo_tramite",
                pIdInstitucion, pFondo, pDescripcion, pTipoTramite);

            return fondoEspecial;
        }

        /// <summary>
        /// Actualiza un registro de FondosEspeciales en la base de datos mediante el procedimiento almacenado
        /// correspondiente y devuelve la entidad actualizada.
        /// </summary>
        /// <remarks>Ejecuta el procedimiento almacenado dbo.sp_FondoEspeciales_Actualizar con parámetros
        /// para id, id_institucion, fondo, descripcion y tipo_tramite. No realiza una lectura posterior desde la base
        /// de datos; la instancia devuelta es la misma proporcionada.</remarks>
        /// <param name="fondoEspecial">Entidad FondosEspeciales con los valores a actualizar.</param>
        /// <param name="idInstitucion">Identificador de la institución asociada al fondo.</param>
        /// <returns>La instancia de FondosEspeciales proporcionada tras aplicar la actualización.</returns>
        public async Task<FondosEspeciales> UpdateFondosEspecialesAsync(FondosEspeciales fondoEspecial, int idInstitucion)
        {
            var pId = new SqlParameter("@id", fondoEspecial.Id);
            var pIdInstitucion = new SqlParameter("@id_institucion", idInstitucion);
            var pFondo = new SqlParameter("@fondo", (object?)fondoEspecial.Fondo ?? DBNull.Value);
            var pDescripcion = new SqlParameter("@descripcion", (object?)fondoEspecial.Descripcion ?? DBNull.Value);
            var pTipoTramite = new SqlParameter("@tipo_tramite", (object?)fondoEspecial.TipoTramite ?? DBNull.Value);

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC [dbo].[sp_FondoEspeciales_Actualizar] @id, @id_institucion, @fondo, @descripcion, @tipo_tramite",
                pId, pIdInstitucion, pFondo, pDescripcion, pTipoTramite);

            return fondoEspecial;
        }

        /// <summary>
        /// Elimina el registro de [Fondo_especiales] con el identificador especificado de la base de datos de forma
        /// asíncrona.
        /// </summary>
        /// <remarks>Ejecuta una sentencia SQL cruda que omite el seguimiento de entidades de EF Core y
        /// utiliza un parámetro SQL para evitar inyección; no comprueba la existencia previa del registro.</remarks>
        /// <param name="id">Identificador del registro a eliminar.</param>
        /// <returns>Tarea que representa la operación asíncrona; se completa cuando la eliminación en la base de datos finaliza.</returns>
        public async Task DeleteFondosEspecialesAsync(int id)
        {
            var pId = new SqlParameter("@id", id);

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC dbo.sp_FondoEspeciales_Eliminar @id", pId);
        }

        private static SqlParameter CrearParametroTexto(string nombre, string? valor, int longitud) =>
            new(nombre, SqlDbType.VarChar, longitud)
            {
                Value = string.IsNullOrWhiteSpace(valor) ? DBNull.Value : valor.Trim()
            };
    }
}
