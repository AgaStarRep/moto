using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Motohusaria.DomainClasses;
using Motohusaria.DTO;
using Motohusaria.Services.Emailing;
using MailKit;
using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using System.Threading;

namespace Motohusaria.Services.Emailing
{
    [InjectableService(typeof(IEmailSender))]
    class EmailSender : IEmailSender
    {
        private readonly EmailingConfig _config;

        public EmailSender(IOptionsSnapshot<EmailingConfig> emaiingconfig)
        {
            _config = emaiingconfig.Value;
        }

        public async Task Send(MailConfigType type, EmailMessageModel message)
        { 
            var email = new MimeMessage();
            email.To.AddRange(message.Recipients.Select(r => new MailboxAddress(r.Name, r.Address)));
            email.From.AddRange(
                message.From.Select(a => new MailboxAddress(a.Name, a.Address))
                );
            email.Sender = new MailboxAddress(message.SenderAddress);
            if (message.Cc != null)
            {
                email.Cc.AddRange(message.Cc.Select(r => new MailboxAddress(r.Name, r.Address)));
            }
            if (message.Bcc != null)
            {
                email.Bcc.AddRange(message.Bcc.Select(r => new MailboxAddress(r.Name, r.Address)));
            }
            email.Subject = message.Subject;

            var bodyBuilder = new BodyBuilder();
            bodyBuilder.TextBody = message.Content;
            email.Body = bodyBuilder.ToMessageBody();

            switch (type)
            {
                case MailConfigType.SMTP:
                    using (var client = new SmtpClient())
                    {
                        var config = _config.SMTP;
                        client.Connect(config.ServerName, config.Port, config.IsUsingSSL);
                        client.AuthenticationMechanisms.Remove("XOAUTH2");
                        client.Authenticate(config.User, config.Password);
                        await client.SendAsync(email);
                        client.Disconnect(true);
                    }
                    break;
                case MailConfigType.POP3:
                    throw new NotImplementedException("POP3 not implemented!");
                    break;
                case MailConfigType.IMAP:
                    throw new NotImplementedException("IMAP not implemented!");
                    break;
                default:
                    throw new ArgumentException("Wrong mail protocol!");
                    break;
            }
        }

        public async Task Send(MailConfigType type, Mail messageFromDb)
        {
            var message = new EmailMessageModel
            {
                From = messageFromDb.From.Split(';').Select(f => new EmailAdress
                {
                    Name = f.Split('@')[0],
                    Address = f.Split('@')[1],
                })
                    .ToList(),

                Cc = messageFromDb.CarbonCopyReceivers.Split(';').Select(f => new EmailAdress
                {
                    Name = f.Split('@')[0],
                    Address = f.Split('@')[1],
                })
                    .ToList(),

                Bcc = messageFromDb.BlindCarponCopyReceivers.Split(';').Select(f => new EmailAdress
                {
                    Name = f.Split('@')[0],
                    Address = f.Split('@')[1],
                })
                    .ToList(),

                Recipients = messageFromDb.Receivers.Split(';').Select(f => new EmailAdress
                {
                    Name = f.Split('@')[0],
                    Address = f.Split('@')[1],
                })
                    .ToList(),

                Content = messageFromDb.Content,
                Subject = messageFromDb.Subject,
            };

            await Send(type, message);
        }
    }
}
