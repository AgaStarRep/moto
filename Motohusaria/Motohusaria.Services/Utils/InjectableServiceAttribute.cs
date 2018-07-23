using System;
using System.Collections.Generic;
using System.Text;

namespace Motohusaria.Services
{
    public class InjectableServiceAttribute : Attribute
    {
        public Type Type { get; set; }

        public InjectableServiceAttribute(Type t)
        {
            Type = t;
        }
    }
}
