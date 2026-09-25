using Mantenimiento.Core.Application.InterfaceServices;
using Mantenimiento.Core.Application.Services;
using Mantenimiento.Core.Domain.RepositoryInterfaces;
using Mantenimiento.Infraestructure.Persistence;
using Mantenimiento.Infraestructure.Persistence.Repositories;
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

            // Repositorios y Servicios
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

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}