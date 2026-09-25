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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Mapeo Tabla capitulo
            modelBuilder.Entity<Capitulo>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.ToTable("capitulo", "dbo");
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.CodigoCapitulo).HasColumnName("codigo_capitulo");
                entity.Property(e => e.NombreCapitulo).HasColumnName("capitulo");
            });


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
        }
    }
}