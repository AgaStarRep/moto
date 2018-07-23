using System;
using System.Collections.Generic;
using System.Text;
using Motohusaria.DomainClasses;

namespace Motohusaria.Services.Events
{
    public class AfterEntityInsert<T> : Event<T> where T : BaseEntity
    {
        public AfterEntityInsert(T entity) : base(entity)
        {
        }
    }
}
