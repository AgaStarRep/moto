using System;
using System.Collections.Generic;
using System.Text;

namespace Motohusaria.DomainClasses
{
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class ListFilterAttribute : Attribute
    {
    }
}
