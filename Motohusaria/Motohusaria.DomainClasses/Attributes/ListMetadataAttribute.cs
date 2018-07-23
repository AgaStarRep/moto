using System;
using System.Collections.Generic;
using System.Text;

namespace Motohusaria.DomainClasses
{
    /// <summary>
    /// Atrybut oznaczający klase która ma informacje o dodtakowych polach mających się znaleść na liście
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class ListMetadataAttribute : Attribute
    {
        public ListMetadataAttribute(Type type)
        {
            Type = type;
        }

        public Type Type { get; private set; }
    }
}
