using System;
using System.Collections.Generic;
using System.Text;

namespace Motohusaria.DTO
{
    public class Notification
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public NotificationType Type { get; set; }
    }

    public enum NotificationType : byte
    {
        Success, Info, Warning, Alert, Error
    }
}
