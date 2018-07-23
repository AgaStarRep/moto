using System.Linq;
using Motohusaria.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Motohusaria.Services
{
    [InjectableService(typeof(INotificationService))]
    public class NotificationService : INotificationService
    {
        IList<Notification> _notifications = new List<Notification>();

        public void AddNotification(Notification notification)
        {
            _notifications.Add(notification);
        }

        public void AddNotification(string title, string content, NotificationType type)
        {
            var notification = new Notification
            {
                Content = content,
                Title = title,
                Type = type,
            };
            AddNotification(notification);
        }

        public void Alert(string content, string title = null)
        {
            AddNotification(title, content, NotificationType.Alert);
        }

        public void Error(string content, string title = null)
        {
            AddNotification(title, content, NotificationType.Error);
        }

        public void Info(string content, string title = null)
        {
            AddNotification(title, content, NotificationType.Info);
        }

        public void Success(string content, string title = null)
        {
            AddNotification(title, content, NotificationType.Success);
        }

        public void Warning(string content, string title = null)
        {
            AddNotification(title, content, NotificationType.Warning);
        }

        public Notification[] GetNotifications()
        {
            return _notifications.ToArray();
        }
    }
}
