using Motohusaria.Services;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Motohusaria.Web.Utils.Logging
{
    public class ExceptionLoggingFilter : IExceptionFilter
    {
        private readonly ILogger _logger;

        public ExceptionLoggingFilter(ILogger logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            if (!context.ExceptionHandled)
            {
                _logger.Error("Nieosbłużony wyjątek", context.Exception);
            }
        }
    }
}
