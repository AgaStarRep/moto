using System;
using System.Collections.Generic;
using System.Text;

namespace Motohusaria.DTO
{
    public class AutoMapAttribute : Attribute
{
    public AutoMapAttribute(Type type)
    {
        Type = type;
    }

    public Type Type { get; set; }
}
}
