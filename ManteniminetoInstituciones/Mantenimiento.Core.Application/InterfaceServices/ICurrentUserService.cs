namespace Mantenimiento.Core.Application.InterfaceServices
{
    public interface ICurrentUserService
    {
        string GetUsername();
        string GetIpAddress();
    }
}
