using Motohusaria.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.Filters;
using Motohusaria.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Motohusaria.Web
{
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ModelState.IsValid)
            {
                return;
            }
            var notificationService = context.HttpContext.RequestServices.GetService<INotificationService>();
            context.ModelState
                .Select(s => s.Value.Errors)
                .SelectMany(s => s)
                .Select(s => new Notification
                {
                    Type = NotificationType.Error,
                    Content = s.ErrorMessage,
                })
                .ToList()
                .ForEach(f => notificationService.AddNotification(f));
            context.Result = ApiResult.Error();
        }
    }
}
