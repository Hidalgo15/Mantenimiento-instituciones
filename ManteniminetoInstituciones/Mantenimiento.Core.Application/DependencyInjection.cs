using FluentValidation;
using Mantenimiento.Core.Application.Validators.Capitulo;
using Microsoft.Extensions.DependencyInjection;

namespace Mantenimiento.Core.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddValidatorsFromAssemblyContaining<CrearCapituloValidator>();
            return services;
        }
    }
}
