using System;
using System.Collections.Generic;
using System.Text;
using Motohusaria.DomainClasses;

namespace Motohusaria.Services.Events
{
    public class AfterEntityDelete<T> : Event<T> where T : BaseEntity
    {
        public AfterEntityDelete(T id) : base(id)
        {
        }
    }
}
