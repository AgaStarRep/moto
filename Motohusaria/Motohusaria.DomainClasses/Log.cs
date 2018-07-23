using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Motohusaria.DomainClasses
{
    public enum LoggingLevel
    {
        //
        // Summary:
        //     Logs that contain the most detailed messages. These messages may contain sensitive
        //     application data. These messages are disabled by default and should never be
        //     enabled in a production environment.
        Trace = 0,
        //
        // Summary:
        //     Logs that are used for interactive investigation during development. These logs
        //     should primarily contain information useful for debugging and have no long-term
        //     value.
        Debug = 1,
        //
        // Summary:
        //     Logs that track the general flow of the application. These logs should have long-term
        //     value.
        Information = 2,
        //
        // Summary:
        //     Logs that highlight an abnormal or unexpected event in the application flow,
        //     but do not otherwise cause the application execution to stop.
        Warning = 3,
        //
        // Summary:
        //     Logs that highlight when the current flow of execution is stopped due to a failure.
        //     These should indicate a failure in the current activity, not an application-wide
        //     failure.
        Error = 4,
        //
        // Summary:
        //     Logs that describe an unrecoverable application or system crash, or a catastrophic
        //     failure that requires immediate attention.
        Critical = 5,
    }

    public class Log : BaseEntity
    {
        public DateTime CreateDate { get; set; }
        
        [MaxLength(255)]
        public string ShortMessage { get; set; }

        [MaxLength(255)]
        public string Controller { get; set; }

        [MaxLength(255)]
        public string Action { get; set; }

        public Guid? UserId { get; set; }

        public LoggingLevel Level { get; set; }

        [Column(TypeName = "varchar(MAX)")]
        public string Data { get; set; }
    }
}
