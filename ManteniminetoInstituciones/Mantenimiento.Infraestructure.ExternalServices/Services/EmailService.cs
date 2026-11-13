using Mantenimiento.Core.Application.InterfaceServices;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace Mantenimiento.Infraestructure.ExternalServices.Services
{
    public sealed class EmailService : IEmailService
    {
        private readonly EmailSettings _settings;

        public EmailService(IOptions<EmailSettings> options)
        {
            _settings = options.Value;
        }

        public async Task EnviarCorreoAsync(
            string destinatario,
            string asunto,
            string mensajeHtml,
            CancellationToken cancellationToken = default)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(destinatario);
            ArgumentException.ThrowIfNullOrWhiteSpace(asunto);
            ArgumentNullException.ThrowIfNull(mensajeHtml);

            if (string.IsNullOrWhiteSpace(_settings.Host))
                throw new InvalidOperationException("No se ha configurado el servidor SMTP.");

            if (_settings.Port is < 1 or > 65535)
                throw new InvalidOperationException("El puerto SMTP configurado no es válido.");

            if (string.IsNullOrWhiteSpace(_settings.SenderEmail))
                throw new InvalidOperationException("No se ha configurado el remitente de correo.");

            var hasUser = !string.IsNullOrWhiteSpace(_settings.User);
            var hasPassword = !string.IsNullOrWhiteSpace(_settings.Password);
            if (hasUser != hasPassword)
                throw new InvalidOperationException("La configuración SMTP debe incluir usuario y contraseña.");

            using var client = new SmtpClient(_settings.Host, _settings.Port)
            {
                EnableSsl = _settings.EnableSsl
            };

            if (hasUser)
                client.Credentials = new NetworkCredential(_settings.User, _settings.Password);

            using var message = new MailMessage
            {
                From = new MailAddress(_settings.SenderEmail, _settings.SenderName),
                Subject = asunto,
                Body = mensajeHtml,
                IsBodyHtml = true
            };

            message.To.Add(new MailAddress(destinatario));
            await client.SendMailAsync(message, cancellationToken);
        }
    }
}
