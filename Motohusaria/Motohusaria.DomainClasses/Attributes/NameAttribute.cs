using System;
using System.Collections.Generic;
using System.Text;

namespace Motohusaria.DomainClasses
{
    /// <summary>
    /// Atrybut z nazwą encji. W query można wykorzystać parametre @@Emnitity@@ i @@Project@@ np. [Name(Query = "@@Entity@@." + nameof(Name))]
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public class NameAttribute : Attribute
{

        public NameAttribute()
        {
        }

        public NameAttribute(string query)
        {
            Query = query;
        }

        public string Query { get; set; }
    }
}
