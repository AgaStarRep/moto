using System;
using System.Collections.Generic;
using System.Text;

namespace Motohusaria.Services.Emailing
{
    public class EmailingConfig
    {
        public SmtpConfig SMTP { get; set; }
    }

    public class SmtpConfig
    {
        public string ServerName { get; set; } = null;
        public int Port { get; set; } = 0;
        public string User { get; set; } = null;
        public string Password { get; set; } = null;
        public bool IsUsingSSL { get; set; } = false;
    }

    public enum MailConfigType
    {
        SMTP,
        POP3,
        IMAP
    }
}