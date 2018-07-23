using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Motohusaria.DTO;
using Motohusaria.DataLayer.Repositories;
using System.Threading.Tasks;
using Motohusaria.DomainClasses;

namespace Motohusaria.Services
{
     [InjectableService(typeof(IUserQueryService))]
public class UserQueryService : IUserQueryService
{

    private readonly IRepository<User> _repository;
    private readonly IMapper _mapper;
    private readonly IUserService _userService;

    public UserQueryService(IRepository<User> repository, IMapper mapper, IUserService userService)
    {
        _repository = repository;
        _mapper = mapper;
        _userService = userService;
    }
    public TableItems<UserListViewModel[]> Search(TableRequest request, UserListViewModel searchModel)
    {
        var query = _repository.TableAsNoTracking;
        IQueryable<TEntity> GetOrderedQuery<TEntity, TKey>(IQueryable<TEntity> loccalQuery, System.Linq.Expressions.Expression<Func<TEntity, TKey>> expression, string sortOrder)
        {
            return sortOrder == "desc" ? loccalQuery.OrderByDescending(expression) : loccalQuery.OrderBy(expression);
        }
        switch (request.sidx)
        {
            case nameof(User.Login):
                query = GetOrderedQuery(query, q => q.Login, request.sord);
                break;
            case nameof(User.PasswordHash):
                query = GetOrderedQuery(query, q => q.PasswordHash, request.sord);
                break;
            case nameof(User.Salt):
                query = GetOrderedQuery(query, q => q.Salt, request.sord);
                break;
            case nameof(User.Id):
                query = GetOrderedQuery(query, q => q.Id, request.sord);
                break;
            default:
                query = GetOrderedQuery(query, q => q.Login, request.sord);
                break;
        }
        if (!string.IsNullOrEmpty(searchModel.Login))
        {
            query = query.Where(w => w.Login.Contains(searchModel.Login));
        }
        if (searchModel.Id.HasValue)
        {
            query = query.Where(w => w.Id == searchModel.Id.Value);
        }
        var rows = query.Select(s => new UserListViewModel
        {
            Login = s.Login,
            Id = s.Id,
        });
        var total = rows.Count();
        var toTake = request.rows;
        if (request.includePrevousPages)
        {
            toTake = request.page * request.rows;
        }
        else
        {
            rows = rows.Skip((request.page - 1) * request.rows);
        }
        var data = rows.Take(toTake).ToArray();
        return new TableItems<UserListViewModel[]>(data, total, request.page, (int)Math.Ceiling((decimal)total / request.rows));
    }

    public async Task<UserViewModel> PrepareViewModelAsync(Guid? id = null)
    {
        if (!id.HasValue)
        {
            return new UserViewModel();
        }
        UserViewModel model = null;
        try
        {
            model = await _repository.TableAsNoTracking.Select(s => new UserViewModel
            {
                Login = s.Login,
                UserRolesSelectedIds = s.UserRoles.Select(other => other.RoleId).ToArray(),
                Id = s.Id,
            })
            .FirstOrDefaultAsync(f => f.Id == id);
        }
        catch (Exception e)
        {
        }
        return model;
    }

    public async Task<OptionsResponse> GetSelectOptionsAsync(string search = null, int page = 1, int size = int.MaxValue)
    {
        var query = _repository.TableAsNoTracking;
        if (!string.IsNullOrEmpty(search))
        {
            query = query.Where(w => w.Login.Contains(search));
        }
        query = query.OrderBy(o => o.Login);
        var total = await query.CountAsync();
        return new OptionsResponse
        {
            Results = query.Skip((page - 1) * size).Take(size).Select(s => new OptionsResponse.OptionsItem { Id = s.Id.ToString(), Text = s.Login }).ToArray(),
            Pagination = new OptionsResponse.OptionsPagination { More = total > page * size }
        };
    }

    public async Task<User> GetByLoginAsync(string login)
    {
        return await _repository.TableAsNoTracking.SingleOrDefaultAsync(s => s.Login == login);
    }
}
}