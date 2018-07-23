using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Motohusaria.DomainClasses;

namespace Motohusaria.Services.Events
{
    [InjectableService(typeof(IEventPublisher))]
public class EventPublisher : IEventPublisher
{
    private readonly IServiceProvider _serviceProvider;

    public EventPublisher(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public bool Publish<TEvent>(TEvent eventMessage) where TEvent : IEvent
    {
        var subscribers = _serviceProvider.GetServices<IEventSubscriber<TEvent>>().OrderBy(o => o.Order);
        foreach (var sub in subscribers)
        {
            if (eventMessage.IsCancelled)
            {
                return eventMessage.IsCancelled;
            }
            sub.HandleEvent(eventMessage);
        }
        return !eventMessage.IsCancelled;
    }

}
}
