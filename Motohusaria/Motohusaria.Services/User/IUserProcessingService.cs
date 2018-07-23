
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Motohusaria.DTO;
using Motohusaria.DomainClasses;
using System.Threading.Tasks;

namespace Motohusaria.Services
{
    public interface IUserProcessingService
    {
        Task<User> InsertAsync(RegisterModel model);

        Task InsertAsync(UserViewModel viewModel);

        Task UpdateAsync(UserViewModel viewModel);

        Task DeleteAsync(Guid id);
    }
}
