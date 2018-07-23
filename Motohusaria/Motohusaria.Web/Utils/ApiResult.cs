using Motohusaria.Services;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Motohusaria.DTO;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Motohusaria.Web
{

    /// <summary>
    /// Odpowiedź json z api do której przed przesłaniem zostną dodane powiadomienia
    /// </summary>
    public class ApiResult : IActionResult
    {
        private JsonResult jsonResult;

        private ApiResultJson result;

        public static ApiResult Error(string content = "", string title = "")
        {
            Notification[] notifications = null;
            if(new[] { content, title }.Any(a => !string.IsNullOrEmpty(a)))
            {
                notifications = new[] { new Notification { Content = content, Title = title, Type = NotificationType.Error } };
            }
            ApiResultJson result = new ApiResultJson
            {
                Success = false,
                Notifications = notifications,
            };
            var jsonResult = new JsonResult(result);
            return new ApiResult(jsonResult, result);
        }

        /// <summary>
        /// Przesyła jedynie informacje o poprawnym wykonaniu i powiadomienia
        /// </summary>
        public static ApiResult Empty
        {
            get
            {
                var json = new ApiResultJson { Success = true };
                return new ApiResult(new JsonResult(json), json);
            }
        }

        private ApiResult(JsonResult jsonResult, ApiResultJson result)
        {
            this.jsonResult = jsonResult;
            this.result = result;
        }

        public ApiResult(object result, bool success = true)
        {
            Init(result, success, null);
        }

        public ApiResult(object result, bool success, JsonSerializerSettings serializerSettings)
        {
            Init(result, success, serializerSettings);
        }

        private void Init(object data, bool success, JsonSerializerSettings serializerSettings)
        {
            result = new ApiResultJson
            {
                Result = data,
                Success = success,
            };
            jsonResult = serializerSettings == null ? new JsonResult(result) : new JsonResult(result, serializerSettings);
        }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            var notificationService = context.HttpContext.RequestServices.GetService<INotificationService>();
            Notification[] notifications = notificationService.GetNotifications();
            if (result.Notifications != null && result.Notifications.Length > 0)
            {
                notifications = result.Notifications.Concat(notifications).ToArray();
            }
            result.Notifications = notifications;
            await jsonResult.ExecuteResultAsync(context);
        }
    }

    public class ApiResultJson
    {
        public object Result { get; set; }

        public bool Success { get; set; }

        public Notification[] Notifications { get; set; }
    }

}
