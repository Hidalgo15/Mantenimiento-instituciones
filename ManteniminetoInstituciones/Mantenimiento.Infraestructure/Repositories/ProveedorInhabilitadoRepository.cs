using System.Data;
using Mantenimiento.Core.Domain.Entities;
using Mantenimiento.Core.Domain.Pagination;
using Mantenimiento.Core.Domain.RepositoryInterfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Mantenimiento.Infraestructure.Persistence.Repositories
{
    public class ProveedorInhabilitadoRepository : IProveedorInhabilitadoRepository
    {
        private readonly ApplicationDbContext _context;

        public ProveedorInhabilitadoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ProveedorInhabilitado>> ObtenerAsync(string? filtroTexto)
        {
            filtroTexto = string.IsNullOrWhiteSpace(filtroTexto) ? null : filtroTexto.Trim();
            var parametroId = new SqlParameter("@Id", SqlDbType.Int) { Value = DBNull.Value };
            var parametroDocumento = new SqlParameter("@NUMERO_DOCUMENTO", SqlDbType.VarChar, 50) { Value = DBNull.Value };
            var parametroFiltro = new SqlParameter("@FiltroTexto", SqlDbType.VarChar, 255)
            {
                Value = (object?)filtroTexto ?? DBNull.Value
            };

            return await _context.ProveedoresInhabilitadosTbl
                .FromSqlRaw(
                    "EXEC dbo.sp_cc_proveedores_inhabilitados_Obtener @Id, @NUMERO_DOCUMENTO, @FiltroTexto",
                    parametroId, parametroDocumento, parametroFiltro)
                .AsNoTracking()
                .ToListAsync();
        }

        /// <summary>
        /// Obtiene una página de proveedores filtrada por razón social, RPE o documento.
        /// </summary>
        /// <param name="filtroTexto">Texto opcional para filtrar los registros.</param>
        /// <param name="pagina">Número de página, comenzando en uno.</param>
        /// <param name="tamanoPagina">Cantidad de registros solicitados por página.</param>
        /// <returns>La página solicitada y el total de registros coincidentes.</returns>
        public async Task<PagedResult<ProveedorInhabilitado>> ObtenerPaginadoAsync(
            string? filtroTexto,
            int pagina,
            int tamanoPagina)
        {
            pagina = Math.Max(pagina, 1);
            tamanoPagina = Math.Clamp(tamanoPagina, 1, 100);
            filtroTexto = string.IsNullOrWhiteSpace(filtroTexto) ? null : filtroTexto.Trim();

            var parametroId = new SqlParameter("@Id", SqlDbType.Int) { Value = DBNull.Value };
            var parametroDocumento = new SqlParameter("@NUMERO_DOCUMENTO", SqlDbType.VarChar, 50) { Value = DBNull.Value };
            var parametroFiltro = new SqlParameter("@FiltroTexto", SqlDbType.VarChar, 255)
            {
                Value = (object?)filtroTexto ?? DBNull.Value
            };
            var parametroPagina = new SqlParameter("@PageNumber", SqlDbType.Int) { Value = pagina };
            var parametroTamanoPagina = new SqlParameter("@PageSize", SqlDbType.Int) { Value = tamanoPagina };
            var parametroTotal = new SqlParameter("@TotalCount", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };

            var resultado = await _context.ProveedoresInhabilitadosTbl
                .FromSqlRaw(
                    "EXEC dbo.sp_cc_proveedores_inhabilitados_ObtenerPaginado @Id, @NUMERO_DOCUMENTO, @FiltroTexto, @PageNumber, @PageSize, @TotalCount OUTPUT",
                    parametroId, parametroDocumento, parametroFiltro, parametroPagina, parametroTamanoPagina, parametroTotal)
                .AsNoTracking()
                .ToListAsync();

            var totalRegistros = Convert.ToInt32(parametroTotal.Value);
            var totalPaginas = totalRegistros == 0 ? 1 : (int)Math.Ceiling(totalRegistros / (double)tamanoPagina);
            pagina = Math.Min(pagina, totalPaginas);
            return new PagedResult<ProveedorInhabilitado>(resultado, pagina, tamanoPagina, totalRegistros);
        }

        /// <summary>
        /// Obtiene los géneros y estados RPE existentes para usarlos como opciones.
        /// </summary>
        /// <returns>Listas distintas de géneros y estados RPE.</returns>
        public async Task<(List<string> Generos, List<string> EstadosRpe)> ObtenerOpcionesAsync()
        {
            var generos = await _context.ProveedoresInhabilitadosTbl
                .AsNoTracking()
                .Where(proveedor => proveedor.Genero != null && proveedor.Genero.Trim() != string.Empty)
                .Select(proveedor => proveedor.Genero!.Trim())
                .Distinct()
                .OrderBy(genero => genero)
                .ToListAsync();

            var estadosRpe = await _context.ProveedoresInhabilitadosTbl
                .AsNoTracking()
                .Where(proveedor => proveedor.EstadoRpe != null && proveedor.EstadoRpe.Trim() != string.Empty)
                .Select(proveedor => proveedor.EstadoRpe!.Trim())
                .Distinct()
                .OrderBy(estado => estado)
                .ToListAsync();

            return (generos, estadosRpe);
        }

        /// <summary>
        /// Recupera un proveedor inhabilitado por su identificador.
        /// </summary>
        /// <param name="id">Identificador del registro.</param>
        /// <returns>El registro encontrado o null si no existe.</returns>
        public async Task<ProveedorInhabilitado?> ObtenerPorIdAsync(int id)
        {
            var parametroId = new SqlParameter("@Id", SqlDbType.Int) { Value = id };
            var resultado = await _context.ProveedoresInhabilitadosTbl
                .FromSqlRaw(
                    "EXEC dbo.sp_cc_proveedores_inhabilitados_Obtener @Id",
                    parametroId)
                .AsNoTracking()
                .ToListAsync();

            return resultado.FirstOrDefault();
        }

        /// <summary>
        /// Registra un proveedor inhabilitado mediante el procedimiento almacenado correspondiente.
        /// </summary>
        /// <param name="proveedor">Datos del proveedor que se creará.</param>
        /// <param name="usuario">Usuario responsable de la operación de auditoría.</param>
        /// <returns>Identificador asignado al nuevo registro.</returns>
        public async Task<int> CrearAsync(ProveedorInhabilitado proveedor, string usuario)
        {
            var parametros = CrearParametrosProveedor(proveedor);
            var parametroIdGenerado = new SqlParameter("@Id_Generado", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };
            parametros.Add(parametroIdGenerado);

            await EjecutarConUsuarioAuditoriaAsync(usuario, () =>
                _context.Database.ExecuteSqlRawAsync(
                    "EXEC dbo.sp_cc_proveedores_inhabilitados_Crear @RPE, @RAZON_SOCIAL, @GENERO, @NUMERO_DOCUMENTO, @TIPO_DOCUMENTO, @TIPO_PERSONA, @ESTADO_RPE, @MOTIVO_INHABILITACION, @Id_Generado OUTPUT",
                    parametros.ToArray()));

            return Convert.ToInt32(parametroIdGenerado.Value);
        }

        /// <summary>
        /// Actualiza los datos de un proveedor inhabilitado y registra su fecha de actualización.
        /// </summary>
        /// <param name="proveedor">Datos actualizados del proveedor.</param>
        /// <param name="usuario">Usuario responsable de la operación de auditoría.</param>
        public async Task ActualizarAsync(ProveedorInhabilitado proveedor, string usuario)
        {
            var parametros = CrearParametrosProveedor(proveedor);
            parametros.Insert(0, new SqlParameter("@Id", SqlDbType.Int) { Value = proveedor.Id });

            await EjecutarConUsuarioAuditoriaAsync(usuario, () =>
                _context.Database.ExecuteSqlRawAsync(
                    "EXEC dbo.sp_cc_proveedores_inhabilitados_Actualizar @Id, @RPE, @RAZON_SOCIAL, @GENERO, @NUMERO_DOCUMENTO, @TIPO_DOCUMENTO, @TIPO_PERSONA, @ESTADO_RPE, @MOTIVO_INHABILITACION",
                    parametros.ToArray()));
        }

        /// <summary>
        /// Elimina físicamente un proveedor inhabilitado.
        /// </summary>
        /// <param name="id">Identificador del registro que se eliminará.</param>
        /// <param name="usuario">Usuario responsable de la operación de auditoría.</param>
        public async Task EliminarAsync(int id, string usuario)
        {
            var parametroId = new SqlParameter("@Id", SqlDbType.Int) { Value = id };
            await EjecutarConUsuarioAuditoriaAsync(usuario, () =>
                _context.Database.ExecuteSqlRawAsync(
                    "EXEC dbo.sp_cc_proveedores_inhabilitados_Eliminar @Id",
                    parametroId));
        }

        private static List<SqlParameter> CrearParametrosProveedor(ProveedorInhabilitado proveedor) =>
        [
            CrearParametroTexto("@RPE", proveedor.Rpe),
            CrearParametroTexto("@RAZON_SOCIAL", proveedor.RazonSocial),
            CrearParametroTexto("@GENERO", proveedor.Genero),
            CrearParametroTexto("@NUMERO_DOCUMENTO", proveedor.NumeroDocumento),
            CrearParametroTexto("@TIPO_DOCUMENTO", proveedor.TipoDocumento),
            CrearParametroTexto("@TIPO_PERSONA", proveedor.TipoPersona),
            CrearParametroTexto("@ESTADO_RPE", proveedor.EstadoRpe),
            CrearParametroTexto("@MOTIVO_INHABILITACION", proveedor.MotivoInhabilitacion)
        ];

        private static SqlParameter CrearParametroTexto(string nombre, string? valor) =>
            new(nombre, SqlDbType.VarChar, -1) { Value = (object?)valor ?? DBNull.Value };

        private async Task EjecutarConUsuarioAuditoriaAsync(string usuario, Func<Task> operacion)
        {
            var connectionWasOpen = _context.Database.GetDbConnection().State == ConnectionState.Open;
            if (!connectionWasOpen)
                await _context.Database.OpenConnectionAsync();

            try
            {
                var parametroUsuario = new SqlParameter("@value", SqlDbType.NVarChar, 128)
                {
                    Value = string.IsNullOrWhiteSpace(usuario) ? DBNull.Value : usuario
                };
                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC sys.sp_set_session_context @key=N'UsuarioApp', @value=@value",
                    parametroUsuario);

                await operacion();
            }
            finally
            {
                try
                {
                    await _context.Database.ExecuteSqlRawAsync(
                        "EXEC sys.sp_set_session_context @key=N'UsuarioApp', @value=NULL");
                }
                finally
                {
                    if (!connectionWasOpen)
                        await _context.Database.CloseConnectionAsync();
                }
            }
        }
    }
}
