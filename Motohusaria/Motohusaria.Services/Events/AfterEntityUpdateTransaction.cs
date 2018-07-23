using System;
using System.Collections.Generic;
using System.Text;
using Motohusaria.DomainClasses;

namespace Motohusaria.Services.Events
{
    public class AfterEntityUpdateTransaction<T> : Event<T> where T : BaseEntity
    {
        public AfterEntityUpdateTransaction(T entity) : base(entity)
        {
        }
    }
}
