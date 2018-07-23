using System;
using System.Collections.Generic;
using System.Text;
using Motohusaria.DomainClasses;
using Motohusaria.DTO;
using System.Threading.Tasks;

namespace Motohusaria.Services.Emailing
{
    public interface IEmailSender
    {
        Task Send(MailConfigType type, EmailMessageModel message);
        Task Send(MailConfigType type, Mail messageFromDb);
    }
}