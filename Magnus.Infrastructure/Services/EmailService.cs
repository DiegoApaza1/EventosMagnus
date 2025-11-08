using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Magnus.Infrastructure.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string to, string subject, string body);
        
    }

    public class EmailService : IEmailService
    {
        private readonly SmtpClient _smtpClient;
        private readonly string _fromAddress;

        public EmailService()
        {
            _fromAddress = "noreply@proyectomagnus.com";
            _smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("tu_correo@gmail.com", "tu_contraseña"),
                EnableSsl = true
            };
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var mail = new MailMessage(_fromAddress, to, subject, body)
            {
                IsBodyHtml = true
            };

            await _smtpClient.SendMailAsync(mail);
        }
    }
}