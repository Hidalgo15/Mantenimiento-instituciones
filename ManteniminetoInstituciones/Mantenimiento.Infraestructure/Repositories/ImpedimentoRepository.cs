using System.Data;
using Mantenimiento.Core.Domain.Entities;
using Mantenimiento.Core.Domain.Pagination;
using Mantenimiento.Core.Domain.RepositoryInterfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Mantenimiento.Infraestructure.Persistence.Repositories
{
    /// <summary>
    /// Accede a los impedimentos por medio de los procedimientos almacenados del módulo.
    /// </summary>
    public class ImpedimentoRepository : IImpedimentoRepository
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Inicializa el repositorio de impedimentos.
        /// </summary>
        /// <param name="context">Contexto de base de datos.</param>
        public ImpedimentoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtiene una página de impedimentos y el total de resultados del filtro.
        /// </summary>
        /// <param name="filtroTexto">Texto opcional a buscar.</param>
        /// <param name="pagina">Página solicitada.</param>
        /// <param name="tamanoPagina">Cantidad de filas por página.</param>
        /// <returns>Resultado paginado de impedimentos.</returns>
        public async Task<PagedResult<Impedimento>> ObtenerPaginadoAsync(string? filtroTexto, int pagina, int tamanoPagina)
        {
            pagina = Math.Max(pagina, 1);
            tamanoPagina = Math.Clamp(tamanoPagina, 1, 100);
            filtroTexto = string.IsNullOrWhiteSpace(filtroTexto) ? null : filtroTexto.Trim();

            var consulta = _context.ImpedimentosTbl.AsNoTracking().AsQueryable();
            if (filtroTexto != null)
            {
                consulta = consulta.Where(impedimento =>
                    (impedimento.Nombres != null && impedimento.Nombres.Contains(filtroTexto)) ||
                    (impedimento.CedulaRnc != null && impedimento.CedulaRnc.Contains(filtroTexto)) ||
                    (impedimento.Motivo != null && impedimento.Motivo.Contains(filtroTexto)));
            }

            var total = await consulta.CountAsync();
            var totalPaginas = total == 0 ? 0 : (int)Math.Ceiling(total / (double)tamanoPagina);
            pagina = totalPaginas == 0 ? 1 : Math.Min(pagina, totalPaginas);

            var registros = await consulta
                .OrderByDescending(impedimento => impedimento.Id)
                .Skip((pagina - 1) * tamanoPagina)
                .Take(tamanoPagina)
                .ToListAsync();

            return new PagedResult<Impedimento>(registros, pagina, tamanoPagina, total);
        }

        /// <summary>
        /// Busca un impedimento por su identificador.
        /// </summary>
        /// <param name="id">Identificador del impedimento.</param>
        /// <returns>El registro encontrado o null.</returns>
        public async Task<Impedimento?> ObtenerPorIdAsync(int id)
        {
            var parametroId = new SqlParameter("@ID", SqlDbType.Int) { Value = id };
            var resultado = await _context.ImpedimentosTbl
                .FromSqlRaw("EXEC dbo.sp_t_Impedimentos_Obtener @ID", parametroId)
                .AsNoTracking()
                .ToListAsync();

            return resultado.FirstOrDefault();
        }

        /// <summary>
        /// Crea un impedimento, incluyendo el usuario registrado en el mismo módulo.
        /// </summary>
        /// <param name="impedimento">Datos del impedimento a crear.</param>
        /// <returns>Identificador generado por la base de datos.</returns>
        public async Task<int> CrearAsync(Impedimento impedimento)
        {
            var parametroNombres = new SqlParameter("@NOMBRES", SqlDbType.NVarChar, 255)
            {
                Value = (object?)impedimento.Nombres ?? DBNull.Value
            };
            var parametroCedulaRnc = new SqlParameter("@cedula_rnc", SqlDbType.NVarChar, 15)
            {
                Value = (object?)impedimento.CedulaRnc ?? DBNull.Value
            };
            var parametroMotivo = new SqlParameter("@Motivo", SqlDbType.VarChar, 500)
            {
                Value = (object?)impedimento.Motivo ?? DBNull.Value
            };
            var parametroEstado = new SqlParameter("@Estado", SqlDbType.Bit) { Value = impedimento.Estado ?? true };
            var parametroUsuario = new SqlParameter("@Usuario_Agrego", SqlDbType.NVarChar, 50)
            {
                Value = (object?)impedimento.UsuarioAgrego ?? DBNull.Value
            };
            var parametroIdGenerado = new SqlParameter("@ID_Generado", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC dbo.sp_t_Impedimentos_Crear @NOMBRES, @cedula_rnc, @Motivo, @Estado, @Usuario_Agrego, @ID_Generado OUTPUT",
                parametroNombres, parametroCedulaRnc, parametroMotivo, parametroEstado, parametroUsuario, parametroIdGenerado);

            return Convert.ToInt32(parametroIdGenerado.Value);
        }

        /// <summary>
        /// Actualiza los datos de un impedimento existente.
        /// </summary>
        /// <param name="impedimento">Datos actualizados.</param>
        public async Task ActualizarAsync(Impedimento impedimento)
        {
            var parametroId = new SqlParameter("@ID", SqlDbType.Int) { Value = impedimento.Id };
            var parametroNombres = new SqlParameter("@NOMBRES", SqlDbType.NVarChar, 255)
            {
                Value = (object?)impedimento.Nombres ?? DBNull.Value
            };
            var parametroCedulaRnc = new SqlParameter("@cedula_rnc", SqlDbType.NVarChar, 15)
            {
                Value = (object?)impedimento.CedulaRnc ?? DBNull.Value
            };
            var parametroMotivo = new SqlParameter("@Motivo", SqlDbType.VarChar, 500)
            {
                Value = (object?)impedimento.Motivo ?? DBNull.Value
            };
            var parametroEstado = new SqlParameter("@Estado", SqlDbType.Bit) { Value = impedimento.Estado ?? false };

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC dbo.sp_t_Impedimentos_Actualizar @ID, @NOMBRES, @cedula_rnc, @Motivo, @Estado",
                parametroId, parametroNombres, parametroCedulaRnc, parametroMotivo, parametroEstado);
        }

        /// <summary>
        /// Realiza la baja lógica de un impedimento.
        /// </summary>
        /// <param name="id">Identificador del impedimento.</param>
        public async Task EliminarAsync(int id)
        {
            var parametroId = new SqlParameter("@ID", SqlDbType.Int) { Value = id };
            var parametroBorradoLogico = new SqlParameter("@BorradoLogico", SqlDbType.Bit) { Value = true };
            await _context.Database.ExecuteSqlRawAsync(
                "EXEC dbo.sp_t_Impedimentos_Eliminar @ID, @BorradoLogico",
                parametroId, parametroBorradoLogico);
        }

    }
}
