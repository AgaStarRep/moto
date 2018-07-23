using Motohusaria.Services;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProgBarTest.Motohusaria.Web.Utils.Notifications
{
    public class NotificationActionFilter : IActionFilter
    {
        private INotificationService _notificationService;

        public NotificationActionFilter(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
        }
    }
}
