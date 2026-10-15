using Mantenimiento.Core.Domain.Entities;
using Mantenimiento.Core.Domain.RepositoryInterfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mantenimiento.Infraestructure.Persistence.Repositories
{
    public class RequisitoContratoRepository : IRequisitoContratoRepository
    {
        private readonly ApplicationDbContext _context;

        public RequisitoContratoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtiene de forma asíncrona los requisitos de contratos, con opción de filtrar por término de búsqueda.
        /// </summary>
        public async Task<List<RequisitoContrato>> ObtenerRequisitosAsync(string? busqueda = null)
        {
            var pCodigo = new SqlParameter("@codigo_requisito", SqlDbType.Int) { Value = DBNull.Value };
            var pBusqueda = new SqlParameter("@busqueda", string.IsNullOrWhiteSpace(busqueda) ? (object)DBNull.Value : busqueda.Trim());

            return await _context.Set<RequisitoContrato>()
                .FromSqlRaw("EXEC dbo.sp_requisitos_contratos_Obtener @codigo_requisito = @codigo_requisito, @busqueda = @busqueda", pCodigo, pBusqueda)
                .AsNoTracking()
                .ToListAsync();
        }

        /// <summary>
        /// Obtiene un requisito de contrato por su código único.
        /// </summary>
        public async Task<RequisitoContrato> GetByIdAsync(int codigoRequisito)
        {
            var pCodigo = new SqlParameter("@codigo_requisito", codigoRequisito);
            var pBusqueda = new SqlParameter("@busqueda", SqlDbType.NVarChar) { Value = DBNull.Value };

            var result = await _context.Set<RequisitoContrato>()
                .FromSqlRaw("EXEC dbo.sp_requisitos_contratos_Obtener @codigo_requisito = @codigo_requisito, @busqueda = @busqueda", pCodigo, pBusqueda)
                .AsNoTracking()
                .ToListAsync();

            var requisito = result.FirstOrDefault();
            if (requisito == null)
            {
                throw new KeyNotFoundException($"No se encontró el requisito de contrato con el código {codigoRequisito}.");
            }

            return requisito;
        }

        /// <summary>
        /// Inserta un nuevo requisito de contrato utilizando el procedimiento almacenado y devuelve el código generado automáticamente.
        /// </summary>
        public async Task<int> InsertarConSpAsync(RequisitoContrato requisito)
        {
            var pDescripcion = new SqlParameter("@descripcion_requisito", requisito.DescripcionRequisito ?? (object)DBNull.Value);
            var pCodigoTre = new SqlParameter("@codigo_documento_tre", requisito.CodigoDocumentoTre.HasValue ? (object)requisito.CodigoDocumentoTre.Value : DBNull.Value);

            var pCodigoGenerado = new SqlParameter
            {
                ParameterName = "@codigo_requisito",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.Output
            };

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC dbo.sp_requisitos_contratos_Crear @descripcion_requisito, @codigo_documento_tre, @codigo_requisito OUTPUT",
                pDescripcion, pCodigoTre, pCodigoGenerado);

            return (int)pCodigoGenerado.Value;
        }

        /// <summary>
        /// Actualiza un requisito de contrato existente en la base de datos.
        /// </summary>
        public async Task ActualizarConSpAsync(RequisitoContrato requisito)
        {
            var pCodigo = new SqlParameter("@codigo_requisito", requisito.CodigoRequisito);
            var pDescripcion = new SqlParameter("@descripcion_requisito", requisito.DescripcionRequisito ?? (object)DBNull.Value);
            var pCodigoTre = new SqlParameter("@codigo_documento_tre", requisito.CodigoDocumentoTre.HasValue ? (object)requisito.CodigoDocumentoTre.Value : DBNull.Value);

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC dbo.sp_requisitos_contratos_Actualizar @codigo_requisito, @descripcion_requisito, @codigo_documento_tre",
                pCodigo, pDescripcion, pCodigoTre);
        }

        /// <summary>
        /// Elimina un requisito de contrato por su código.
        /// </summary>
        public async Task EliminarConSpAsync(int codigoRequisito)
        {
            var pCodigo = new SqlParameter("@codigo_requisito", codigoRequisito);
            await _context.Database.ExecuteSqlRawAsync("EXEC dbo.sp_requisitos_contratos_Eliminar @codigo_requisito", pCodigo);
        }
    }
}

