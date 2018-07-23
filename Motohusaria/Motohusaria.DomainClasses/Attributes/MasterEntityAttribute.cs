using System;
using System.Collections.Generic;
using System.Text;

namespace Motohusaria.DomainClasses
{
    /// <summary>
    /// Atrybut oznaczający encje nadrzędną w relacji. Powinien się znajdować po drógej stronie MasterDetailEditorAttribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class MasterEntityAttribute : Attribute
    {
    }
}
