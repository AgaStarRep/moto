using System;
using System.Collections.Generic;
using System.Text;
using Motohusaria.DomainClasses;

namespace Motohusaria.Services.Events
{
    public class BeforeEntityUpdateTransaction<T> : Event<T> where T : BaseEntity
    {
        public BeforeEntityUpdateTransaction(T entity) : base(entity)
        {
        }
    }
}
