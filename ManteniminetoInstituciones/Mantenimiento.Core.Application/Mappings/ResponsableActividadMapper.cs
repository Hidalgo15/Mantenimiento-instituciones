using Mantenimiento.Core.Application.DTOs.ResponsableActividad;
using Mantenimiento.Core.Domain.Entities.ResponsableActividad;

namespace Mantenimiento.Core.Application.Mappings
{
    public static class ResponsableActividadMapper
    {
        public static ResponsableActividadDto ToDto(ResponsableActividad entity) => new()
        {
            CodigoDespa = entity.CodigoDespa,
            CodigoActividad = entity.CodigoActividad,
            DescripcionActividad = entity.DescripcionActividad
        };

        public static MoldeCatalogoDto ToDto(MoldeCatalogo entity) => new()
        {
            CodigoTre = entity.CodigoTre,
            ArbolAsociado = entity.ArbolAsociado,
            NombreTre = entity.NombreTre
        };

        public static SubtipoMoldeDto ToDto(SubtipoMolde entity) => new()
        {
            CodigoTipoTre = entity.CodigoTipoTre,
            CodigoTre = entity.CodigoTre,
            Descripcion = entity.Descripcion,
            Abreviatura = entity.Abreviatura
        };

        public static UsuarioSubtipoDto ToDto(UsuarioSubtipo entity) => new()
        {
            CodigoDespa = entity.CodigoDespa,
            Nombre = entity.Nombre,
            Institucion = entity.Institucion,
            DescArea = entity.DescArea,
            Cargo = entity.Cargo,
            Email = entity.Email
        };

        public static ActividadSubtipoDto ToDto(ActividadSubtipo entity) => new()
        {
            CodigoActividad = entity.CodigoActividad,
            CodigoTipoTre = entity.CodigoTipoTre,
            DescripcionActividad = entity.DescripcionActividad,
            DescripcionSubtipo = entity.DescripcionSubtipo
        };

        public static UsuarioActividadAsignacionDto ToDto(UsuarioActividadAsignacion entity) => new()
        {
            CodigoDespa = entity.CodigoDespa,
            Nombre = entity.Nombre,
            Institucion = entity.Institucion,
            DescArea = entity.DescArea,
            Cargo = entity.Cargo,
            Email = entity.Email,
            EstaAsignado = entity.EstaAsignado
        };
    }
}