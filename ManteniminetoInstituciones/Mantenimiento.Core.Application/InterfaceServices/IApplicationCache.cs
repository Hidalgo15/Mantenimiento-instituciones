namespace Mantenimiento.Core.Application.InterfaceServices
{
    public interface IApplicationCache
    {
        Task<TValue> GetOrCreateAsync<TValue>(
            string key,
            Func<Task<TValue>> factory,
            TimeSpan expiration) where TValue : class;
    }
}
