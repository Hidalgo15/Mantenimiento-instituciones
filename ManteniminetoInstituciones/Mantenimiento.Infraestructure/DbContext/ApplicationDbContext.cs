using Mantenimiento.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.SqlServer;

namespace Mantenimiento.Infraestructure.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Institucion> Instituciones { get; set; }
        public DbSet<CuentaInstitucion> CuentasInstitucion { get; set; }
        public DbSet<Capitulo> Capitulos { get; set; }
        public DbSet<SubCapitulo> SubCapitulos { get; set; }
        public DbSet<Daf> Dafs { get; set; }
        public DbSet<UnidadEjecutora> UnidadesEjecutoras { get; set; }
        public DbSet<AnalistaSupervisorConsulta> AnalistaSupervisorConsultas { get; set; }
        public DbSet<SupervisorOption> SupervisorOptions { get; set; }
        public DbSet<CategoriaTramite> CategoriasTramite { get; set; }
        public DbSet<FondosEspeciales> FondosEspeciales { get; set; }
        public DbSet<TramiteContrato> TipoTramiteContratos { get; set; }

        public DbSet<CuentaDbo> TipoCuentaDbo { get; set; }
        public DbSet<CuentaCt> TipoCuentaCt { get; set; }
        public DbSet<TipoTramitePago> TipoTramitePago { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region CategoriaTramite

            // CategoriaTramite

            modelBuilder.Entity<CategoriaTramite>(entity =>
            {
                entity.HasNoKey();
                entity.ToView("v_categoria_tramite", "dbo");
            });

            #endregion

            #region AnalistaSupervisor
            // AnalistaSupervisor

            modelBuilder.Entity<AnalistaSupervisorConsulta>().HasNoKey();
            modelBuilder.Entity<SupervisorOption>().HasNoKey();

            #endregion

            #region UnidadEjecutora
            // Mapeo Tabla UnidadEjecutiva

            modelBuilder.Entity<UnidadEjecutora>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.ToTable("unidad_ejecutora", "dbo");

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.IdPadre).HasColumnName("id_padre");
                entity.Property(e => e.IdDaf).HasColumnName("id_daf");
                entity.Property(e => e.CodigoDaf).HasColumnName("codigo_daf");
                entity.Property(e => e.NombreDaf).HasColumnName("daf");
                entity.Property(e => e.CodigoUnidadEjecutora).HasColumnName("codigo_unidad_ejecutora");
                entity.Property(e => e.NombreUnidadEjecutora).HasColumnName("unidad_ejecutora");
                entity.Property(e => e.Rnc).HasColumnName("rnc");
                entity.Property(e => e.Estado).HasColumnName("estado");
                entity.Property(e => e.PortalCompra).HasColumnName("portal_compra");
                entity.Property(e => e.Creado).HasColumnName("creado");
                entity.Property(e => e.Modificado).HasColumnName("modificado");
                entity.Property(e => e.UsuarioCreacion).HasColumnName("usuario_creacion");
                entity.Property(e => e.UsuarioModificacion).HasColumnName("usuario_modificacion");

                entity.Property(e => e.IdCategoria).HasColumnName("id_categoria");
                entity.Property(e => e.Estructura).HasColumnName("estructura");
                entity.Property(e => e.DescripcionCategoria).HasColumnName("descripcion");
            });

            #endregion

            #region Daf
            // Mapeo Tabla Daf

            modelBuilder.Entity<Daf>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.ToTable("daf", "dbo");
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.IdSubCapitulo).HasColumnName("id_sub_capitulo");
                entity.Property(e => e.CodigoDaf).HasColumnName("codigo_daf");
                entity.Property(e => e.NombreDaf).HasColumnName("daf");
            });

            #endregion

            #region capitulo
            // Mapeo Tabla capitulo
            modelBuilder.Entity<Capitulo>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.ToTable("capitulo", "dbo");
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.CodigoCapitulo).HasColumnName("codigo_capitulo");
                entity.Property(e => e.NombreCapitulo).HasColumnName("capitulo");
            });

            #endregion

            #region SubCapitul
            // Mapeo Tabla SubCapitulo
            modelBuilder.Entity<SubCapitulo>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.ToTable("sub_capitulo", "dbo");

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.IdCapitulo).HasColumnName("id_capitulo");
                entity.Property(e => e.CodigoSubCapitulo).HasColumnName("codigo_sub_capitulo");
                entity.Property(e => e.subcapitulo).HasColumnName("sub_capitulo");

                // FALTABAN ESTOS DOS MAPEOS PARA EL SP:
                entity.Property(e => e.CodigoCapitulo).HasColumnName("codigo_capitulo");
                entity.Property(e => e.Capitulo).HasColumnName("capitulo");
            });

            #endregion

            #region v_institucion
            // Mapeo Vista v_institucion
            modelBuilder.Entity<Institucion>(entity =>
            {
                entity.HasNoKey();
                entity.ToView("v_institucion", "dbo");
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.IdInstitucion).HasColumnName("id_institucion");
                entity.Property(e => e.Estatus).HasColumnName("estatus");
                entity.Property(e => e.Estructura).HasColumnName("estructura");
                entity.Property(e => e.Capitulo).HasColumnName("capitulo");
                entity.Property(e => e.SubCapitulo).HasColumnName("sub_capitulo");
                entity.Property(e => e.Daf).HasColumnName("daf");
                entity.Property(e => e.UnidadEjecutora).HasColumnName("unidad_ejecutora");
                entity.Property(e => e.Rnc).HasColumnName("rnc");
                entity.Property(e => e.PortalCompra).HasColumnName("portal_compra");
            });

            #endregion

            #region t_cta_institucion
            // Mapeo Tabla t_cta_institucion
            modelBuilder.Entity<CuentaInstitucion>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.ToTable("t_cta_institucion", "dbo");
                entity.Property(e => e.Id).HasColumnName("ID");
                entity.Property(e => e.InsCodigo).HasColumnName("ins_codigo");
                entity.Property(e => e.Institucion).HasColumnName("Institucion");
                entity.Property(e => e.CtaCtaBanco).HasColumnName("cta_cta_banco");
                entity.Property(e => e.CtaDescripcion).HasColumnName("cta_descripcion");
            });

            #endregion

            #region FondosEspeciales
            // Mapeo Tabla FondosEspeciales
            modelBuilder.Entity<FondosEspeciales>(entity =>
            {
             
                entity.HasKey(e => e.Id);
                entity.ToTable("Fondo_especiales", "dbo");

                entity.Property(e => e.Id)
                      .HasColumnName("Id");

                entity.Property(e => e.Fondo)
                      .HasColumnName("Fondo");

                entity.Property(e => e.Descripcion)
                      .HasColumnName("descripcion");

                entity.Property(e => e.Estructura)
                      .HasColumnName("estructura_institucion");

                // Mapeo simple como columna de texto
                entity.Property(e => e.TipoTramite)
                      .HasColumnName("tipo_tramite");
            });
            #endregion

            #region TipoTramiteContrato
            // Mapeo Tabla TipoTramiteContrato
            modelBuilder.Entity<TramiteContrato>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.ToTable("tipo_tramite_contrato", "dbo");
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.TipoTramite).HasColumnName("tipo_tramite");
            });
            #endregion


            #region dbo.cuenta

            modelBuilder.Entity<CuentaDbo>(entity =>
            {
                entity.HasKey(e => e.IdCuenta);
                entity.ToTable("cuentas", "dbo");
                entity.Property(e => e.IdCuenta).HasColumnName("id_cuenta");
                entity.Property(e => e.CodigoCuenta).HasColumnName("codigo_cuenta");
                entity.Property(e => e.DescripcionCuenta).HasColumnName("descripcion_cuenta");
                entity.Property(e => e.TipoTramiteId).HasColumnName("tipo_tramite");

                // Ignorar la propiedad auxiliar que viene del SP / JOIN
                entity.Ignore(e => e.TipoTramiteNombre);
            });

            #endregion

            #region ct.cuenta
            modelBuilder.Entity<CuentaCt>(entity =>
            {
                entity.HasKey(e => e.CuentaID);
                entity.ToTable("cuentas", "ct");
                entity.Property(e => e.CuentaID).HasColumnName("CuentaID");
                entity.Property(e => e.IdCuenta).HasColumnName("id_cuenta");
                entity.Property(e => e.CodigoCuenta).HasColumnName("codigo_cuenta");
                entity.Property(e => e.DescripcionCuenta).HasColumnName("descripcion_cuenta");
            });
            #endregion

            #region TipoTramitePago
            modelBuilder.Entity<TipoTramitePago>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.ToTable("tipo_tramite_pago", "dbo"); // Corregido a la tabla real dbo.tipo_tramite_pago
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Tipo).HasColumnName("tipo");
            });
            #endregion
        }
    }
}