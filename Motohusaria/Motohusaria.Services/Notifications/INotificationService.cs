using Motohusaria.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Motohusaria.Services
{
    public interface INotificationService
    {
        /// <summary>
        /// Dodaje powiadomienie do odpowiedzi z serwera.
        /// </summary>
        /// <param name="notification"></param>
        void AddNotification(Notification notification);

        /// <summary>
        /// Dodaje powiadomienie do odpowiedzi z serwera.
        /// </summary>
        /// <param name="notification"></param>
        void AddNotification(string title, string content, NotificationType type);

        /// <summary>
        /// Dodaje powiadomienie o statusie Success do odpowiedzi z serwera.
        /// </summary>
        /// <param name="notification"></param>
        void Success(string content, string title = null);

        /// <summary>
        /// Dodaje powiadomienie o statusie Info do odpowiedzi z serwera.
        /// </summary>
        /// <param name="notification"></param>
        void Info(string content, string title = null);

        /// <summary>
        /// Dodaje powiadomienie o statusie Warning do odpowiedzi z serwera.
        /// </summary>
        /// <param name="notification"></param>
        void Warning(string content, string title = null);

        /// <summary>
        /// Dodaje powiadomienie o statusie Alert do odpowiedzi z serwera.
        /// </summary>
        /// <param name="notification"></param>
        void Alert(string content, string title = null);

        /// <summary>
        /// Dodaje powiadomienie o statusie Error do odpowiedzi z serwera.
        /// </summary>
        /// <param name="notification"></param>
        void Error(string content, string title = null);

        /// <summary>
        /// Zwraca tablicę z aktualnymi powiadomieniami
        /// </summary>
        /// <param name="notification"></param>
        Notification[] GetNotifications();
    }
}
