using Motohusaria.Services;
using Motohusaria.Services.Logger;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Motohusaria.Web.Utils.Logging
{
    [InjectableService(typeof(ILoggerDataProvider))]
public class LoggingDbContextDataProvider : ILoggerDataProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    IActionContextAccessor _actionContextAccessor;
    KeyValuePair<string, object>[] _cachedData;

    /// <summary>
    /// Konstruktor dla requestów z web
    /// </summary>
    /// <param name="httpContextAccessor"></param>
    /// <param name="actionContextAccessor"></param>
    public LoggingDbContextDataProvider(IHttpContextAccessor httpContextAccessor, IActionContextAccessor actionContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
        _actionContextAccessor = actionContextAccessor;
    }

    /// <summary>
    /// Konstruktor dla scope'ów które nie są powiązane z requestami
    /// </summary>
    /// <param name="httpContextAccessor"></param>
    public LoggingDbContextDataProvider(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public KeyValuePair<string, object>[] GetData()
    {
        //Dane z httpContextu podczas jednego requeustu się nie zmienią więc można je cache'ować
        if (_cachedData == null)
        {
            _cachedData = CreateRequestData().ToArray();
        }
        return _cachedData;
    }

    private List<KeyValuePair<string, object>> CreateRequestData()
    {
        var data = new List<KeyValuePair<string, object>>();
        var context = _httpContextAccessor.HttpContext;
        if (context == null || _actionContextAccessor == null)
        {
            return data;
        }
        var aContext = _actionContextAccessor.ActionContext;
        try
        {
            data.Add(new KeyValuePair<string, object>("controller", aContext.RouteData.Values.FirstOrDefault(f => f.Key == "controller").Value));
            data.Add(new KeyValuePair<string, object>("action", aContext.RouteData.Values.FirstOrDefault(f => f.Key == "action").Value));

            data.Add(new KeyValuePair<string, object>("userId", context.User?.Claims?.FirstOrDefault(f => f.Type == ClaimTypes.NameIdentifier)?.Value));
            data.Add(new KeyValuePair<string, object>("localIpAddress", context.Connection.LocalIpAddress.ToString()));
            data.Add(new KeyValuePair<string, object>("remoteIpAddress", context.Connection.RemoteIpAddress.ToString()));
            var claims = context.User?.Claims?.Select(s => new { s.Type, s.Value });
            data.Add(new KeyValuePair<string, object>("claims", claims));

            data.Add(new KeyValuePair<string, object>("path", context.Request.Path));
            data.Add(new KeyValuePair<string, object>("request-body", ReadRequestBodyString(context)));
            var query = context.Request.Query.Select(s => new { s.Key, s.Value });
            data.Add(new KeyValuePair<string, object>("request-query", query));
            data.Add(new KeyValuePair<string, object>("request-method", context.Request.Method));
        }
        catch (Exception e)
        {
            data.Add(new KeyValuePair<string, object>("błąd zapisu dodatkowych danych do loggera", e.ToString()));
        }
        return data;
    }

    private static string ReadRequestBodyString(HttpContext context)
    {
        var bodyStr = "";
        var req = context.Request;

        // Allows using several time the stream in ASP.Net Core
        req.EnableRewind();
        if (req.Body.Position != 0)
        {
            req.Body.Position = 0;
        }

        using (StreamReader reader = new StreamReader(req.Body, Encoding.UTF8, true, 1024, true))
        {
            bodyStr = reader.ReadToEnd();
        }

        // Rewind, so the core is not lost when it looks the body for the request
        req.Body.Position = 0;
        return bodyStr;
    }
}
}
