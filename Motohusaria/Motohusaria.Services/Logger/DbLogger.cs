using Motohusaria.DataLayer;
using Motohusaria.DomainClasses;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Motohusaria.Services.Logger
{
    [InjectableService(typeof(ILogger))]
    public class DbLogger : ILogger
    {
        private readonly LoggingDbContext _db;
        private readonly IEnumerable<ILoggerDataProvider> _dataProviders;
        private readonly ILogger<DbLogger> _aspCoreLogger;
        private LoggerOptions _options;

        public DbLogger(LoggingDbContext db, IEnumerable<ILoggerDataProvider> dataProviders, ILogger<DbLogger> aspCoreLogger, IOptionsSnapshot<LoggerOptions> logggerOptions)
        {
            _db = db;
            _dataProviders = dataProviders;
            _aspCoreLogger = aspCoreLogger;
            _options = logggerOptions.Value;
        }

        public void Log(LoggingLevel level, string shortMessage, Exception exception = null, object data = null)
        {
            if (!ShouldLog(level))
            {
                return;
            }
            try
            {
                Log log = GetLog(level, shortMessage, exception, data);
                _db.Log.Add(log);
                _db.SaveChanges();
            }
            catch (Exception e)
            {
                _aspCoreLogger.LogError(e, "Bład logowania");
            }
        }

        public async Task LogAsync(LoggingLevel level, string shortMessage, Exception exception = null, object data = null)
        {
            if (!ShouldLog(level))
            {
                return;
            }
            try
            {
                Log log = GetLog(level, shortMessage, exception, data);
                _db.Log.Add(log);
                await _db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _aspCoreLogger.LogError(e, "Bład logowania");
            }
        }

        private Log GetLog(LoggingLevel level, string shortMessage, Exception exception = null, object data = null)
        {
            var aditionalData = _dataProviders.SelectMany(s => s.GetData()).ToArray();
            var controller = aditionalData.FirstOrDefault(a => a.Key.ToLowerInvariant() == "controller").Value?.ToString() ?? "";
            var action = aditionalData.FirstOrDefault(a => a.Key.ToLowerInvariant() == "action").Value?.ToString() ?? "";
            var userId = aditionalData.FirstOrDefault(a => a.Key.ToLowerInvariant() == "userid").Value?.ToString() ?? "";
            Guid userIdGuid = Guid.Empty;
            Guid.TryParse(userId, out userIdGuid);

            var dataObj = new Dictionary<string, object> { ["data"] = data };
            if(exception != null)
            {
                dataObj["exception"] = exception.ToString();
            }
            var aditionalDataObj = new Dictionary<string, object>();
            foreach (var otherData in aditionalData)
            {
                var key = otherData.Key;
                if (aditionalDataObj.ContainsKey(otherData.Key))
                {
                    //Jeżeli już jest to niech doda nowy unikalny;
                    key = otherData.Key + "_" + Guid.NewGuid().ToString();
                }
                aditionalDataObj[key] = otherData.Value;
            }
            dataObj["_aditionalData"] = aditionalDataObj;
            string dataJson = "";
            try
            {
                dataJson = Newtonsoft.Json.JsonConvert.SerializeObject(dataObj);
            }
            catch (Exception e)
            {
                dataJson = "Wystąpił błąd serializacji data w loggerze. Data - " + string.Join(";", dataObj.Select(s => s.Key + ":" + s.Value.ToString()));
            }
            var log = new Log
            {
                Id = Guid.NewGuid(),
                Action = action,
                Controller = controller,
                CreateDate = DateTime.Now,
                Data = dataJson,
                Level = level,
                ShortMessage = shortMessage,
                UserId = userIdGuid == Guid.Empty ? null : (Guid?)userIdGuid,
            };
            return log;
        }

        public void Critical(string shortMessage, object data = null)
        {
            Log(LoggingLevel.Critical, shortMessage, null, data);
        }

        public void Debug(string shortMessage, object data = null)
        {
            Log(LoggingLevel.Debug, shortMessage, null, data);
        }

        public void Error(string shortMessage, object data = null)
        {
            Log(LoggingLevel.Error, shortMessage, null, data);
        }

        public void Information(string shortMessage, object data = null)
        {
            Log(LoggingLevel.Information, shortMessage, null, data);
        }

        public void Trace(string shortMessage, object data = null)
        {
            Log(LoggingLevel.Trace, shortMessage, null, data);
        }

        public void Warning(string shortMessage, object data = null)
        {
            Log(LoggingLevel.Warning, shortMessage, null, data);
        }

        public bool ShouldLog(LoggingLevel level)
        {
            return (int)level >= (int)this._options.LogLevel;
        }

        public void Error(string shortMessage, Exception e, object data = null)
        {
            Log(LoggingLevel.Error, shortMessage, e, data);
        }
    }
}
