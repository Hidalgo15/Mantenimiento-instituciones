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

            // Registro de AutoMapper
            //builder.Services.AddAutoMapper(typeof(MappingProfile));

            // Repositorios y Servicios
            builder.Services.AddScoped<ICuentaInstitucionRepository, CuentaInstitucionRepository>();
            builder.Services.AddScoped<CuentaInstitucionService>();

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
                pattern: "{controller=CuentaInstitucion}/{action=Index}/{id?}");

            app.Run();
        }
    }
}