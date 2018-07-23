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
    [InjectableService(typeof(IUserRoleService))]
    public class UserRoleService : BaseCachedService<UserRole>, IUserRoleService
    {
        public UserRoleService(IRepository<UserRole> repository, ICacheService cacheService) : base(repository, cacheService)
        {
        }

        protected override string GetAllKey => "UserRole.All";

        protected override string GetByIdKey => "UserRole.Id.";
    }
}
