namespace Mantenimiento.Infraestructure.ExternalServices.Services
{
    public sealed class EmailSettings
    {
        public string Host { get; set; } = string.Empty;
        public int Port { get; set; } = 587;
        public bool EnableSsl { get; set; } = true;
        public string? User { get; set; }
        public string? Password { get; set; }
        public string SenderEmail { get; set; } = string.Empty;
        public string SenderName { get; set; } = string.Empty;
    }
}
