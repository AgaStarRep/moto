using System;
using System.Collections.Generic;
using System.Text;
using Motohusaria.DomainClasses;
using Motohusaria.DTO;

namespace Motohusaria.Services.Events
{
    public class BeforeEntityInsert<T> : Event<T> where T : IViewModel
    {
        public BeforeEntityInsert(T entity) : base(entity)
        {
        }
    }
}
