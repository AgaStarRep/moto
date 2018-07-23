using System;
using System.Collections.Generic;
using System.Text;
using Motohusaria.DomainClasses;

namespace Motohusaria.Services.Events
{
    public class BeforeEntityUpdate<T> : Event<T>
    {
        public BeforeEntityUpdate(T entity) : base(entity)
        {
        }
    }
}
