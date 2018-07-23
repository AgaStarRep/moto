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
    [InjectableService(typeof(IUserService))]
    public class UserService : BaseCachedService<User>, IUserService
    {
        public UserService(IRepository<User> repository, ICacheService cacheService) : base(repository, cacheService)
        {
        }

        protected override string GetAllKey => "User.All";

        protected override string GetByIdKey => "User.Id.";

       
    }
}
