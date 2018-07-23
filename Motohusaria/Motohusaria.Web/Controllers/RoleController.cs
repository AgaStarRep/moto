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
public class RoleController : Controller
{
    private readonly IRoleService _roleService;
    private readonly IRoleQueryService _roleQueryService;
    private readonly IRoleProcessingService _roleProcessingService;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;

    public RoleController(IRoleService roleService, IRoleQueryService roleQueryService, IRoleProcessingService roleProcessingService, IMapper mapper, ILogger logger)
    {
        _roleService = roleService;
        _roleQueryService = roleQueryService;
        _roleProcessingService = roleProcessingService;
        _mapper = mapper;
        _logger = logger;
    }


    [HttpGet("lookup")]
    public async Task<IActionResult> GetRoleOptions(string search, int page = 1, int size = int.MaxValue)
    {
        var options = await _roleQueryService.GetSelectOptionsAsync(search, page, size);
        return new ApiResult(options);
    }

    [HttpGet]
    public IActionResult GetList(TableRequest request, RoleListViewModel searchModel)
    {
        var response = _roleQueryService.Search(request, searchModel);
        return new ApiResult(response);
    }

    [ValidateModel]
    [HttpPost]
    public async Task<IActionResult> Add([FromBody] RoleViewModel model)
    {
        try
        {
            await _roleProcessingService.InsertAsync(model);
            return ApiResult.Empty;
        }
        catch (Exception e)
        {
            _logger.Error("Błąd dodawania roli.", e, model);
            return ApiResult.Error("Wystąpił błąd dodawania roli.");
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var model = await _roleQueryService.PrepareViewModelAsync(id);
        return new ApiResult(model);
    }

    [ValidateModel]
    [HttpPut("{id}")]
    public async Task<IActionResult> Edit([FromBody] RoleViewModel model)
    {
        try
        {
            await _roleProcessingService.UpdateAsync(model);
            return ApiResult.Empty;
        }
        catch (Exception e)
        {
            _logger.Error("Błąd aktualizacji roli.", e, model);
            return ApiResult.Error("Wystąpił błąd aktualizacji roli.");

        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            await _roleProcessingService.DeleteAsync(id);
            return ApiResult.Empty;
        }
        catch (Exception e)
        {
            _logger.Error("Błąd usunięcia roli.", e, id);
            return ApiResult.Error("Wystąpił błąd usunięcia roli.");
        }
    }
}
}