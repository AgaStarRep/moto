using System;
using System.Collections.Generic;
using System.Text;
using Motohusaria.DomainClasses;

namespace Motohusaria.Services.Events
{
    public class BeforeEntityDelete<T> : Event<T>
    {
        public BeforeEntityDelete(T entity) : base(entity)
        {
        }
    }
}
