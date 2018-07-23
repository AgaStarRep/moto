using System;
using System.Collections.Generic;
using System.Text;

namespace Motohusaria.DomainClasses
{
    /// <summary>
    /// Atrybut oznaczający property które nie będzie wyświetlany na liście ani przesyłany do modelu.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class ListIgnoreAttribute : Attribute
    {
    }
}
