
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Motohusaria.DTO;
using Motohusaria.DomainClasses;
using System.Threading.Tasks;

namespace Motohusaria.Services
{
    public interface IRoleQueryService
    {
        Task<RoleViewModel> PrepareViewModelAsync(Guid? id = null);

        TableItems<RoleListViewModel[]> Search(TableRequest request, RoleListViewModel searchModel);

        Task<OptionsResponse> GetSelectOptionsAsync(string search = null, int page = 1, int size = int.MaxValue);

        Task<Role[]> GetRolesByUserId(Guid userId);
    }
}
