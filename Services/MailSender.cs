using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;

namespace MailService.MinWebAPI.Services
{
    public class MailSender : IMailSender
    {
        private readonly MailSettings mailSettings;
        public MailSender(IOptions<MailSettings> mailSettings)
        {
            this.mailSettings = mailSettings.Value;
        }
        public async Task SendEmailAsync(MailMessage mailMessage)
        {
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(mailSettings.Mail);
            var recipients = mailMessage.Recipients ??= new List<string>();
                foreach (var r in recipients)
                {
                    email.To.Add(MailboxAddress.Parse(r));
                }
            email.Subject = mailMessage.Subject;
            var builder = new BodyBuilder();
            builder.HtmlBody = mailMessage.Body;
            email.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();
            smtp.Connect(mailSettings.Host, mailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(mailSettings.Mail, mailSettings.Password);
            
            try
            {
                await smtp.SendAsync(email);
            }
            catch (Exception ex)
            {
                mailMessage.FailedMessage = ex.Message;
            }
            smtp.Disconnect(true);
        }
    }
}