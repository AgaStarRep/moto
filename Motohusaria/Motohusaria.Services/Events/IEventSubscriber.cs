using System;
using System.Collections.Generic;
using System.Text;
using Motohusaria.DomainClasses;

namespace Motohusaria.Services.Events
{
    /// <summary>
    /// Interfejs oznaczający subskrypcje na zdażenia <typeparamref name="TEvent"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IEventSubscriber<TEvent> where TEvent : IEvent
    {
        void HandleEvent(TEvent eventMessage);

        int Order { get; }
    }
}
