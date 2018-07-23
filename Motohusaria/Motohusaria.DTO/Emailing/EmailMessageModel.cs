using System;
using System.Collections.Generic;
using System.Text;

namespace Motohusaria.DTO
{
    public class EmailMessageModel
    {
        public string SenderAddress { get; set; }
        public List<EmailAdress> From { get; set; }
        public List<EmailAdress> Recipients { get; set; }
        public List<EmailAdress> Cc { get; set; }
        public List<EmailAdress> Bcc { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
    }

    public class EmailAdress
    {
        public string Name { get; set; }
        public string Address { get; set; }
    }
}
