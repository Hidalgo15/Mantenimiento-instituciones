using Mantenimiento.Core.Application.DTOs.CuentaRequisito;
using Mantenimiento.Core.Domain.Entities;

namespace Mantenimiento.Core.Application.Mappings
{
    public static class CuentaRequisitoMapper
    {
        public static CuentaRequisitoDto ToDto(CuentaRequisitoConsulta entity) => new()
        {
            CuentaRequisitosID = entity.CuentaRequisitosID,
            IdCuenta = entity.IdCuenta,
            CodigoCuenta = entity.CodigoCuenta,
            DescripcionCuenta = entity.DescripcionCuenta,
            IdRequisito = entity.IdRequisito,
            DescripcionRequisito = entity.DescripcionRequisito,
            Estado = entity.Estado
        };

        public static CuentaCatalogoDto ToDto(CuentaCatalogo entity) => new()
        {
            IdCuenta = entity.IdCuenta,
            CodigoCuenta = entity.CodigoCuenta,
            DescripcionCuenta = entity.DescripcionCuenta
        };

        public static RequisitoCatalogoDto ToDto(RequisitoCatalogo entity) => new()
        {
            IdRequisitos = entity.IdRequisitos,
            Requisitos = entity.Requisitos
        };

    }
}