using Mantenimiento.Core.Application.DTOs.CuentaInstitucion;
using Mantenimiento.Core.Domain.Entities;

namespace Mantenimiento.Core.Application.Mappings
{
    public static class CuentaInstitucionMapper
    {
        public static CuentaInstitucionDto ToDto(CuentaInstitucion entity) => new()
        {
            Id = entity.Id ?? 0,
            InsCodigo = entity.InsCodigo,
            Institucion = entity.Institucion,
            CtaCtaBanco = entity.CtaCtaBanco,
            CtaDescripcion = entity.CtaDescripcion
        };

        public static CuentaInstitucion ToEntity(CrearCuentaInstitucionDto dto) => new()
        {
            InsCodigo = dto.InsCodigo,
            Institucion = dto.Institucion,
            CtaCtaBanco = dto.CtaCtaBanco,
            CtaDescripcion = dto.CtaDescripcion
        };

        public static CuentaInstitucion ToEntity(ActualizarCuentaInstitucionDto dto) => new()
        {
            Id = dto.Id,
            InsCodigo = dto.InsCodigo,
            Institucion = dto.Institucion,
            CtaCtaBanco = dto.CtaCtaBanco,
            CtaDescripcion = dto.CtaDescripcion
        };
    }
}
