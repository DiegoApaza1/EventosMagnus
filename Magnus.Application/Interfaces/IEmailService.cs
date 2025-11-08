namespace Magnus.Application.Interfaces
{
    /// <summary>
    /// Puerto para enviar emails — implementado en Infrastructure.
    /// </summary>
    public interface IEmailService
    {
        Task SendEmailAsync(string to, string subject, string body);
    }
}