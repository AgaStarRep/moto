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
    [InjectableService(typeof(IRoleQueryService))]
public partial class RoleQueryService : IRoleQueryService
{

    private readonly IRepository<Role> _repository;
    private readonly IMapper _mapper;
    private readonly IRoleService _roleService;

    public RoleQueryService(IRepository<Role> repository, IMapper mapper, IRoleService roleService)
    {
        _repository = repository;
        _mapper = mapper;
        _roleService = roleService;
    }
    public TableItems<RoleListViewModel[]> Search(TableRequest request, RoleListViewModel searchModel)
    {
        var query = _repository.TableAsNoTracking;
        IQueryable<TEntity> GetOrderedQuery<TEntity, TKey>(IQueryable<TEntity> localQuery, System.Linq.Expressions.Expression<Func<TEntity, TKey>> expression, string sortOrder)
        {
            return sortOrder == "desc" ? localQuery.OrderByDescending(expression) : localQuery.OrderBy(expression);
        }
        //Zmień pierwszy znak kolumny do sortowania na wielką literę
        if (string.IsNullOrEmpty(request.sidx))
            request.sidx = string.Empty;
        else
        {
            char[] chars = request.sidx.ToCharArray();
            chars[0] = char.ToUpper(chars[0]);
            request.sidx = new string(chars);
        }
        switch (request.sidx)
        {
            case nameof(Role.Name):
                query = GetOrderedQuery(query, q => q.Name, request.sord);
                break;
            case nameof(Role.Id):
                query = GetOrderedQuery(query, q => q.Id, request.sord);
                break;
            default:
                query = GetOrderedQuery(query, q => q.Name, request.sord);
                break;
        }

        if (!string.IsNullOrEmpty(searchModel.Name))
        {
            query = query.Where(w => w.Name.Contains(searchModel.Name));
        }
        if (searchModel.Id.HasValue)
        {
            query = query.Where(w => w.Id == searchModel.Id.Value);
        }

        var rows = query.Select(s => new RoleListViewModel
        {
            Name = s.Name,
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
        return new TableItems<RoleListViewModel[]>(data, total, request.page, (int)Math.Ceiling((decimal)total / request.rows));
    }

    public async Task<RoleViewModel> PrepareViewModelAsync(Guid? id = null)
    {
        if (!id.HasValue)
        {
            return new RoleViewModel();
        }
        RoleViewModel model = null;
        try
        {
            model = await _repository.TableAsNoTracking.Select(s => new RoleViewModel
            {
                Name = s.Name,
                RoleUsersSelectedIds = s.RoleUsers.Select(other => other.UserId).ToArray(),
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
            query = query.Where(w => w.Name.Contains(search));
        }
        query = query.OrderBy(o => o.Name);
        var total = await query.CountAsync();
        return new OptionsResponse
        {
            Results = query.Skip((page - 1) * size).Take(size).Select(s => new OptionsResponse.OptionsItem { Id = s.Id.ToString(), Text = s.Name }).ToArray(),
            Pagination = new OptionsResponse.OptionsPagination { More = total > page * size }
        };
    }


    public async Task<Role[]> GetRolesByUserId(Guid userId)
    {
        return await _repository.TableAsNoTracking.Where(s => s.RoleUsers.Any(a => a.UserId == userId)).ToArrayAsync();
    }

}
}