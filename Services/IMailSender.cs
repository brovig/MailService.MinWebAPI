namespace MailService.MinWebAPI.Services
{
    public interface IMailSender
    {
        Task SendEmailAsync(MailMessage mailMessage);
    }
}