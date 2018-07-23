using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection.Metadata;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Motohusaria.DomainClasses;
using Motohusaria.Services.Emailing;

namespace Motohusaria.Services.Emailing
{
    public static class EmailingConfigResolver
    {
        public static SmtpConfig GetSmtpConfig(IOptionsSnapshot<EmailingConfig> config)
        {
            return config.Value.SMTP;
        }
    }
}