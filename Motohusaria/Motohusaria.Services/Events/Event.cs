using System;
using System.Collections.Generic;
using System.Text;

namespace Motohusaria.Services.Events
{
    public class Event<T> : IEvent
    {
        public Event(T entity)
        {
            Entity = entity;
        }

        public T Entity { get; protected set; }

        public bool IsCancelled { get; set; } = false;
    }
}
