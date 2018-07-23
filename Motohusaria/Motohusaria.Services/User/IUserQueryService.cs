
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Motohusaria.DTO;
using Motohusaria.DomainClasses;
using System.Threading.Tasks;

namespace Motohusaria.Services
{
    public interface IUserQueryService
    {
        Task<UserViewModel> PrepareViewModelAsync(Guid? id = null);

        TableItems<UserListViewModel[]> Search(TableRequest request, UserListViewModel searchModel);

        Task<OptionsResponse> GetSelectOptionsAsync(string search = null, int page = 1, int size = int.MaxValue);

        Task<User> GetByLoginAsync(string login);
    }
}
