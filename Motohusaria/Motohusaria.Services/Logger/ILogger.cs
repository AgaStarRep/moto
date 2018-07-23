using Motohusaria.DomainClasses;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Motohusaria.Services.Logger;

namespace Motohusaria.Services
{
    public interface ILogger
    {
        void Log(LoggingLevel level, string shortMessage, Exception e = null,  object data = null);

        Task LogAsync(LoggingLevel level, string shortMessage, Exception e = null,  object data = null);

        void Trace(string shortMessage, object data = null);

        void Debug(string shortMessage, object data = null);

        void Information(string shortMessage, object data = null);

        void Warning(string shortMessage, object data = null);

        void Error(string shortMessage, object data = null);

        void Error(string shortMessage, Exception e,  object data = null);

        void Critical(string shortMessage, object data = null);

        bool ShouldLog(LoggingLevel level);
    }

}
