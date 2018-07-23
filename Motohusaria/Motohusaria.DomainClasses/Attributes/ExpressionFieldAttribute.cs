using System;
using System.Collections.Generic;
using System.Text;

namespace Motohusaria.DomainClasses
{
    /// <summary>
    /// Atrybut oznaczający że pole zostanie pobranie przez wyrażenie linku. Aktualne encje należy oznaczać przez @@Entity@@. np. @@Entity@@.Name
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class ExpressionFieldAttribute : Attribute
    {
        public ExpressionFieldAttribute(string expression)
        {
            Expression = expression;
        }

        public string Expression { get; private set; }
    }
}
