using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Motohusaria.DomainClasses;

namespace Motohusaria.Web.Utils.Authorization
{
    public interface ITokenService
    {
        Task<string> GenerateUserTokenAsync(User user);
    }
}
