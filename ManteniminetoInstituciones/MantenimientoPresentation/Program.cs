using Mantenimiento.Core.Application.InterfaceServices;
using Mantenimiento.Core.Application.Services;
using Mantenimiento.Core.Domain.RepositoryInterfaces;
using Mantenimiento.Infraestructure.Persistence;
using Mantenimiento.Infraestructure.Persistence.Repositories;
using MantenimientoPresentation.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

namespace MantenimientoPresentation
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // DbContext Corregido
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("ConexionSIGOB")));

            // HttpContextAccessor
            builder.Services.AddHttpContextAccessor();

            // Repositorios y Servicios
            builder.Services.AddScoped<IInstitucionRepository, InstitucionRepository>();
            builder.Services.AddScoped<IInstitucionService, InstitucionService>();

            builder.Services.AddScoped<ICuentaInstitucionRepository, CuentaInstitucionRepository>();
            builder.Services.AddScoped<ICuentaInstitucionService, CuentaInstitucionService>();

            builder.Services.AddScoped<ICapituloRepository, CapituloRepository>();
            builder.Services.AddScoped<ICapituloService, CapituloService>();

            builder.Services.AddScoped<ISubCapituloRepository, SubCapituloRepository>();
            builder.Services.AddScoped<ISubCapituloService, SubCapituloService>();

            builder.Services.AddScoped<IDafRepository, DafRepository>();
            builder.Services.AddScoped<IDafService, DafService>();

            builder.Services.AddScoped<IUnidadEjecutoraRepository, UnidadEjecutoraRepository>();
            builder.Services.AddScoped<IUnidadEjecutoraService, UnidadEjecutoraService>();

            builder.Services.AddScoped<IAnalistaSupervisorRepository, AnalistaSupervisorRepository>();
            builder.Services.AddScoped<IAnalistaSupervisorService, AnalistaSupervisorService>();

            builder.Services.AddScoped<ICategoriaTramiteRepository, CategoriaTramiteRepository>();
            builder.Services.AddScoped<ICategoriaTramiteService, CategoriaTramiteService>();

            builder.Services.AddScoped<IFondoEspecialesService, FondoEspecialesService>();
            builder.Services.AddScoped<IFondoEspecialesRepository, FondoEspecialesRepository>();

            builder.Services.AddScoped<ITipoTramiteService, TipoTramiteService>();
            builder.Services.AddScoped<ITipoTramiteRepository, TipoTramiteContratoRepository>();

            builder.Services.AddScoped<ITipoTramitePagoRepository, TipoTramitePagoRepository>();
            builder.Services.AddScoped<ITipoTramitePagoService, TipoTramitePagoService>();

            builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

            builder.Services.AddScoped<ICuentaMapeoRepository, CuentaMapeoRepository>();
            builder.Services.AddScoped<ICuentaMapeoService, CuentaMapeoService>();

            builder.Services.AddScoped<ICuentaCtRepository, CuentaCtRepository>();
            builder.Services.AddScoped<ICuentaCtService, CuentaCtService>();

            builder.Services.AddScoped<ICuentaRequisitoRepository, CuentaRequisitoRepository>();
            builder.Services.AddScoped<ICuentaRequisitoService, CuentaRequisitoService>();

            builder.Services.AddScoped<IRequisitoRepository, RequisitoRepository>();
            builder.Services.AddScoped<IRequisitoService, RequisitoService>();

            builder.Services.AddScoped<IClasificacionContratoRepository, ClasificacionContratoRepository>();
            builder.Services.AddScoped<IClasificacionContratoService, ClasificacionContratoService>();

            builder.Services.AddScoped<IRequisitoContratoRepository, RequisitoContratoRepository>();
            builder.Services.AddScoped<IRequisitoContratoService, RequisitoContratoService>();

            builder.Services.AddScoped<IRelacionContratoRequisitoRepository, RelacionContratoRequisitoRepository>();
            builder.Services.AddScoped<IRelacionContratoRequisitoService, RelacionContratoRequisitoService>();

            builder.Services.AddScoped<IResponsableActividadRepository, ResponsableActividadRepository>();
            builder.Services.AddScoped<IResponsableActividadService, ResponsableActividadService>();

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.LoginPath = "/Auth/Login";
                options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Auth}/{action=Login}/{id?}");

            app.Run();
        }
    }
}