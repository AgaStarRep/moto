using System;
using System.Collections.Generic;
using System.Text;

namespace Motohusaria.DomainClasses.Motohusaria.DomainClasses.Attributes.Aggregation
{
    [AttributeUsage(AttributeTargets.Class)]
    public class AggregateTableAttribute : Attribute
    {
        public AggregateTableAttribute(Type turnover, Type balance, int precision = 18, int scale = 4)
        {
            Turnover = turnover;
            Balance = balance;
            Precision = precision;
            Scale = scale;
        }

        public Type Turnover { get; private set; }
        public Type Balance { get; private set; }

        public int Precision { get; private set; }

        public int Scale { get; private set; }
    }
}
