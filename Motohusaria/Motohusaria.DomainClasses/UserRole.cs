using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;
using System.Text;

namespace Motohusaria.DomainClasses
{
    [BridgeTable]
    public class UserRole : BaseEntity
    {
        public virtual User User { get; set; }

        [ForeignKey(nameof(User))]
        public Guid UserId { get; set; }

        public virtual Role Role { get; set; }

        [ForeignKey(nameof(Role))]
        public Guid RoleId { get; set; }
    }
}
