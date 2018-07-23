using System;
using System.Collections.Generic;
using System.Text;

namespace Motohusaria.DomainClasses
{
    /// <summary>
    /// Atrybut oznaczjacy wlasciwosc ktora stanie sie linkiem do edycji
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
        public class EditLinkAttribute : Attribute
    {
    }   
}
