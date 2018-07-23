using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Motohusaria.DomainClasses;

namespace Motohusaria.Services.Emailing
{
    public interface IEmailingService : IService<Mail>
    {
        Task<Mail> GetFirstAsync();
    }
}
