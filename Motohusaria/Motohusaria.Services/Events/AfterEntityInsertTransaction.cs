using System;
using System.Collections.Generic;
using System.Text;
using Motohusaria.DomainClasses;

namespace Motohusaria.Services.Events
{
    public class AfterEntityInsertTransaction<T> : Event<T> where T : BaseEntity
    {
        public AfterEntityInsertTransaction(T entity) : base(entity)
        {
        }
    }
}
