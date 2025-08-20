using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Threading.Tasks;

namespace DentisAPI.Services
{
    public class EmailSender : IEmailSender
    {
        public IConfiguration Configuration { get; }
        public EmailSender(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            SmtpClient client = new SmtpClient()
            {
                Port = int.Parse(Configuration["NetMail:port"] ?? throw new InvalidOperationException("NetMail:port missing")),
                Host = Configuration["NetMail:smtpHost"] ?? throw new InvalidOperationException("NetMail:smtpHost missing"), //or another email sender provider
                EnableSsl = bool.Parse(Configuration["NetMail:EnableSsl"] ?? throw new InvalidOperationException("NetMail:EnableSsl missing")),
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(Configuration["NetMail:sender"], Configuration["NetMail:senderpassword"])
            };
            MailMessage mm = new MailMessage(Configuration["NetMail:sender"] ?? throw new InvalidOperationException("NetMail missing"), email, subject, htmlMessage)
            {
                IsBodyHtml = true
            };
           return client.SendMailAsync(mm);
        }
    }
}
