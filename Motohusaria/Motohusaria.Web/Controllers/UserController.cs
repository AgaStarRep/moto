using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Motohusaria.DomainClasses;
using Motohusaria.DTO;
using Motohusaria.Services;

namespace Motohusaria.Web.Controllers
{
    [Route("api/[controller]")]
public class UserController : Controller
{
    private readonly IUserService _userService;
    private readonly IUserQueryService _userQueryService;
    private readonly IUserProcessingService _userProcessingService;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;

    public UserController(IUserService userService, IUserQueryService userQueryService, IUserProcessingService userProcessingService, IMapper mapper, ILogger logger)
    {
        _userService = userService;
        _userQueryService = userQueryService;
        _userProcessingService = userProcessingService;
        _mapper = mapper;
        _logger = logger;
    }


    [HttpGet("lookup")]
    public async Task<IActionResult> GetUserOptions(string search, int page = 1, int size = int.MaxValue)
    {
        var options = await _userQueryService.GetSelectOptionsAsync(search, page, size);
        return new ApiResult(options);
    }

    [HttpGet]
    public IActionResult GetList(TableRequest request, UserListViewModel searchModel)
    {
        var response = _userQueryService.Search(request, searchModel);
        return new ApiResult(response);
    }

    [ValidateModel]
    [HttpPost]
    public async Task<IActionResult> Add([FromBody] UserViewModel model)
    {
        try
        {
            await _userProcessingService.InsertAsync(model);
            return ApiResult.Empty;
        }
        catch (Exception e)
        {
            _logger.Error("Błąd dodawania użytkownika.", e, model);
            return ApiResult.Error("Wystąpił błąd dodawania użytkownika");
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var model = await _userQueryService.PrepareViewModelAsync(id);
        return new ApiResult(model);
    }

    [ValidateModel]
    [HttpPut("{id}")]
    public async Task<IActionResult> Edit([FromBody] UserViewModel model)
    {
        try
        {

            await _userProcessingService.UpdateAsync(model);
            return ApiResult.Empty;
        }
        catch (Exception e)
        {
            _logger.Error("Błąd edycji użytkownika.", e, model);
            return ApiResult.Error("Wystąpił błąd edycji użytkownika");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            await _userProcessingService.DeleteAsync(id);
            return ApiResult.Empty;
        }
        catch (Exception e)
        {
            _logger.Error("Błąd usunięcia użytkownika.", e, id);
            return ApiResult.Error("Wystąpił błąd usunięcia użytkownika");
        }
    }
}
}