using System;
using System.Collections.Generic;
using System.Text;

namespace Motohusaria.DomainClasses.Motohusaria.DomainClasses.Attributes.Aggregation
{
    [AttributeUsage(AttributeTargets.Property)]
    public class DimentionAttribute : Attribute
    {
        public DimentionAttribute(bool nullable = false)
        {
            Nullable = nullable;
        }

        public bool Nullable { get; private set; }


    }
}
