using Mantenimiento.Core.Application.InterfaceServices;
using Microsoft.Extensions.Caching.Memory;

namespace Mantenimiento.Infraestructure.ExternalServices.Services
{
    public sealed class MemoryApplicationCache : IApplicationCache
    {
        private readonly IMemoryCache _memoryCache;

        public MemoryApplicationCache(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public async Task<TValue> GetOrCreateAsync<TValue>(
            string key,
            Func<Task<TValue>> factory,
            TimeSpan expiration) where TValue : class
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(key);
            ArgumentNullException.ThrowIfNull(factory);

            if (expiration <= TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(expiration), "La expiración debe ser mayor que cero.");

            var value = await _memoryCache.GetOrCreateAsync(key, async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = expiration;
                return await factory();
            });

            return value ?? throw new InvalidOperationException("La fábrica de caché devolvió un valor nulo.");
        }
    }
}
