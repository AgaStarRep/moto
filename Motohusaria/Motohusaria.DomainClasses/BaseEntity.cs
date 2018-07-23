using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Motohusaria.DomainClasses
{
    public abstract class BaseEntity
    {
        [Key]
        public Guid Id { get; set; }

    }
}
