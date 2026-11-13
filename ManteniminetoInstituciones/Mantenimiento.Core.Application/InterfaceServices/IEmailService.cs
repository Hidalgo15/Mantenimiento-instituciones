namespace Mantenimiento.Core.Application.InterfaceServices
{
    public interface IEmailService
    {
        Task EnviarCorreoAsync(string destinatario, string asunto, string mensajeHtml, CancellationToken cancellationToken = default);
    }
}
