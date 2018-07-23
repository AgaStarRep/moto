using System;
using System.Collections.Generic;
using System.Text;
using Motohusaria.DomainClasses;

namespace Motohusaria.Services.Events
{
    public class BeforeEntityInsertTransaction<T> : Event<T> where T : BaseEntity
    {
        public BeforeEntityInsertTransaction(T entity) : base(entity)
        {
        }
    }
}
