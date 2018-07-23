using System;
using System.Collections.Generic;
using System.Text;

namespace Motohusaria.DomainClasses
{
    /// <summary>
    /// Atrybut oznaczjacy kolekcja której elementy będą edytowane w tabeli inline. Powinien się znajdować po drógej stronie MasterEntityAttribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class MasterDetailEditorAttribute : Attribute
    {
    }
}
