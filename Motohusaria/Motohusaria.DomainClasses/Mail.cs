using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Motohusaria.DomainClasses
{

    public enum MailStatus
    {
        //Waiting for set time
        Pending,
        //Will be sent ASAP
        Queued,
        //Already sent
        Sent,
        //Something has gone wrong
        Failed,
    }

    public enum MailPriority
    {
        Critical,
        Important,
        Preferred,
        Normal
    }

    public class Mail : BaseEntity
    {
        [Required]
        public MailPriority Priority { get; set; }

        public MailStatus Status { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }

        public DateTime ToBeSentOn { get; set; }

        public DateTime SentOn { get; set; }

        //Multiple adresses possible. Use ; to divide them
        [Required]
        public string From { get; set; }

        //Multiple adresses possible. Use ; to divide them
        [Required]
        public string Receivers { get; set; }

        //Multiple adresses possible. Use ; to divide them
        public string CarbonCopyReceivers { get; set; }

        //Multiple adresses possible. Use ; to divide them
        public string BlindCarponCopyReceivers { get; set; }

        [Required]
        [MaxLength(78)] //Safe limit for single-line subject - according to RFC 2822
        public string Subject { get; set; }

        [Required]
        public string Content { get; set; }
    }
}
