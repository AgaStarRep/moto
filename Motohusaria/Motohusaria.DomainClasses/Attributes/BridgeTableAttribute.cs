using System;
using System.Collections.Generic;
using System.Text;

namespace Motohusaria.DomainClasses
{
    /// <summary>
    /// Ten atrybut oznacza klasse która jest traktowana jako tabela intersekcji
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class BridgeTableAttribute : Attribute
{
    }
}
