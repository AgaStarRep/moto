using System;
using System.Collections.Generic;
using System.Text;
using Motohusaria.DomainClasses;

namespace Motohusaria.Services.Events
{
    public class AfterEntityUpdate<T> : Event<T> where T : BaseEntity
    {
        public AfterEntityUpdate(T entity) : base(entity)
        {
        }
    }
}
