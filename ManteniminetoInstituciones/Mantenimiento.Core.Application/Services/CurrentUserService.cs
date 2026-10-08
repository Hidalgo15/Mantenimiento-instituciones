using System.Security.Claims;
using Mantenimiento.Core.Application.InterfaceServices;
using Microsoft.AspNetCore.Http;

namespace Mantenimiento.Core.Application.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetUsername()
        {
            var httpContext = _httpContextAccessor.HttpContext;

            // Cuando integren el Login (Identity / Active Directory / JWT / Cookies)
            var identityName = httpContext?.User?.Identity?.Name;
            if (!string.IsNullOrWhiteSpace(identityName))
            {
                return identityName;
            }

            // O buscar en los Claims
            var claimName = httpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;
            if (!string.IsNullOrWhiteSpace(claimName))
            {
                return claimName;
            }

            // Si está en red corporativa/dominio sin login formal aún
            var windowsUser = httpContext?.User?.Identity?.Name;
            if (!string.IsNullOrWhiteSpace(windowsUser))
            {
                return windowsUser;
            }

            // Mientras tanto en Desarrollo / Red Local
            var envUser = Environment.UserName; // Usuario del SO local
            var machineName = Environment.MachineName; // Nombre del equipo en la red

            return !string.IsNullOrWhiteSpace(envUser) ? $"{machineName}\\{envUser}" : "SISTEMA";
        }

        public string GetIpAddress()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            var ip = httpContext?.Connection?.RemoteIpAddress?.ToString();
            return string.IsNullOrWhiteSpace(ip) ? "127.0.0.1" : ip;
        }
    }
}