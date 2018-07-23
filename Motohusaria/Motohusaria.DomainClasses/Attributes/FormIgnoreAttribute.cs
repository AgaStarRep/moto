using System;
using System.Collections.Generic;
using System.Text;

namespace Motohusaria.DomainClasses
{
    /// <summary>
    /// Atrybut oznaczający property które nie będzie wyświetlany na formularzu ani przesyłany do modelu formularza
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class FormIgnoreAttribute : Attribute
    {
    }
}
