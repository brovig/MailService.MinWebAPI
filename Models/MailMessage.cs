namespace MailService.MinWebAPI
{
    public class MailMessage
    {
        public int Id { get; set; }
        public string? Subject { get; set; }
        public string? Body { get; set; }
        public ICollection<string>? Recipients { get; set; }

        public DateTime CreationDate { get; set; }
        public bool SendResult { get; set; }
        public string? FailedMessage { get; set; }

    }
}