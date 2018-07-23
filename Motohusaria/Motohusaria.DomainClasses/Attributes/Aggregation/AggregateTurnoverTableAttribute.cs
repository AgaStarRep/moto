using System;
using System.Collections.Generic;
using System.Text;

namespace Motohusaria.DomainClasses.Motohusaria.DomainClasses.Attributes.Aggregation
{
    [AttributeUsage(AttributeTargets.Class)]
    public class AggregateTurnoverTableAttribute : Attribute
    {
        public AggregateTurnoverTableAttribute(string aggregateKeyPropName)
        {
            AggregateKeyPropName = aggregateKeyPropName;
        }

        public string AggregateKeyPropName { get; private set; }
    }
}
