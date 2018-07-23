
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Motohusaria.DTO;
using Motohusaria.DomainClasses;
using System.Threading.Tasks;

namespace Motohusaria.Services
{
    public interface IRoleProcessingService
    {

        Task InsertAsync(RoleViewModel viewModel);

        Task UpdateAsync(RoleViewModel viewModel);

        Task DeleteAsync(Guid id);
    }
}
