using MimeKit;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.EmailSender.Sender
{
    public interface ISender
    {
        Task SendEmailAsync(string email, string subject, string message);

        Task SendAsync(MimeMessage message);

        MimeMessage CreateEmailMessage(EmailMessage message);
        MailMessage CreateEmailMessageWithDefaultType(EmailMessage message);
    }
}
