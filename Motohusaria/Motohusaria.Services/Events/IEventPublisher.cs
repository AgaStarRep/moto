using Motohusaria.DomainClasses;

namespace Motohusaria.Services.Events
{
    public interface IEventPublisher
    {
        bool Publish<TEvent>(TEvent eventMessage) where TEvent : IEvent;
    }
}