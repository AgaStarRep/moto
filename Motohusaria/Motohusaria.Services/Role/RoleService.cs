using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Motohusaria.DomainClasses;
using Motohusaria.DTO;
using System.Threading.Tasks;
using Motohusaria.DataLayer.Repositories;
using Microsoft.EntityFrameworkCore;
using Motohusaria.Services.Events;

namespace Motohusaria.Services
{
    [InjectableService(typeof(IRoleService))]
    public class RoleService : BaseCachedService<Role>, IRoleService
    {
        public RoleService(IRepository<Role> repository, ICacheService cacheService) : base(repository, cacheService)
        {
        }

        protected override string GetAllKey => "Role.All";

        protected override string GetByIdKey => "Role.Id.";
    }
}
