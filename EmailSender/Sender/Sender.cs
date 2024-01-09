using Microsoft.Extensions.Options;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using MimeKit;
using System;
using System.Security.Authentication;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;

namespace Infrastructure.EmailSender.Sender
{
    public class Sender : ISender
    {
        private readonly EmailSenderOptions _emailSenderOptions;

        public Sender(IOptions<EmailSenderOptions> emailSenderOptions)
        {
            _emailSenderOptions = emailSenderOptions.Value;
        }
        public MimeMessage CreateEmailMessage(EmailMessage message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(MailboxAddress.Parse(_emailSenderOptions.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;
            var bodyBuilder = new BodyBuilder { HtmlBody = message.Content };
            emailMessage.Body = bodyBuilder.ToMessageBody();
            return emailMessage;
        }

        public MailMessage CreateEmailMessageWithDefaultType(EmailMessage message)
        {
            var emailMessage = new MailMessage()
            {
                Subject = message.Subject,
                From = new MailAddress(_emailSenderOptions.From, "Salone OrientaMenti 2020"),

            };

            foreach (var to in message.To)
            {
                var addr = new MailAddress(to.Address);
                emailMessage.To.Add(addr);
            }
            emailMessage.Body = message.Content;
            emailMessage.IsBodyHtml = true;
            return emailMessage;
        }

        public async Task SendAsync(MimeMessage message)
        {
            using var client = new MailKit.Net.Smtp.SmtpClient();
            try
            {
                client.SslProtocols = SslProtocols.Tls | SslProtocols.Tls11 | SslProtocols.Tls12 | SslProtocols.Tls13;

                if(_emailSenderOptions.EnableSsl)
                {
                    await client.ConnectAsync(_emailSenderOptions.SmtpServer,
                        _emailSenderOptions.Port,
                        _emailSenderOptions.EnableSsl);
                } else
                {
                    bool socketOptions = Enum.TryParse(_emailSenderOptions.SecureSocketOptions, true, out SecureSocketOptions secureSocketOptions);
                    if (!socketOptions)
                    {
                        secureSocketOptions = SecureSocketOptions.Auto;
                    }

                    await client.ConnectAsync(_emailSenderOptions.SmtpServer,
                        _emailSenderOptions.Port,
                        secureSocketOptions);
                }

                if(!string.IsNullOrEmpty(_emailSenderOptions.UserName) &&
                    !string.IsNullOrEmpty(_emailSenderOptions.Password))
                {
                    await client.AuthenticateAsync(_emailSenderOptions.UserName, _emailSenderOptions.Password);
                }

                await client.SendAsync(message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                await client.DisconnectAsync(true);
                client.Dispose();
            }
        }

        public void SendAsync(MailMessage message)
        {
            using var client = new System.Net.Mail.SmtpClient(_emailSenderOptions.SmtpServer)
            {
                Port = _emailSenderOptions.Port,
                Credentials = new NetworkCredential(_emailSenderOptions.UserName, _emailSenderOptions.Password)
            };
            try
            {
                client.SendCompleted += (s, e) =>
                {
                    client.Dispose();
                    message.Dispose();
                };

                client.Send(message);
            }
            catch (Exception)
            {
                throw;
            }

        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var emailMessage = new EmailMessage(new string[] { email }, subject, message);
            var created = CreateEmailMessageWithDefaultType(emailMessage);
            SendAsync(created);
        }
    }
}
