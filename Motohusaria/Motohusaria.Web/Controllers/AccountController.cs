using Motohusaria.DTO;
using Motohusaria.Services;
using Motohusaria.Web.Utils.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Motohusaria.Web.Controllers
{
        public class AccountController : Controller
{
    private readonly ITokenService _tokenService;
    private readonly IUserQueryService _userQueryService;
    private readonly IPasswordService _passwordService;
    private readonly IUserProcessingService _userProcessingService;
    private readonly ILogger _logger;

    public AccountController(ITokenService tokenService, IUserQueryService userQueryService, IPasswordService passwordService, IUserProcessingService userProcessingService, ILogger logger)
    {
        _tokenService = tokenService;
        _userQueryService = userQueryService;
        _passwordService = passwordService;
        _userProcessingService = userProcessingService;
        _logger = logger;
    }

    [HttpPost]
    [ValidateModel]
    [AllowAnonymous]
    public async Task<IActionResult> Token([FromBody] LoginModel model)
    {
        try
        {
            var user = await _userQueryService.GetByLoginAsync(model.Login);
            if (user != null && _passwordService.VerifyPassword(user, model.Password))
            {
                var token = await _tokenService.GenerateUserTokenAsync(user);
                return new ApiResult(token);
            }
            return ApiResult.Error("Nieprawidłowe dane logowania.");
        }
        catch (Exception e)
        {
            _logger.Error("Błąd logowania użytkownika", e, model);
            return ApiResult.Error("Wystąpił błąd logowania");
        }
    }

    [HttpPost]
    [ValidateModel]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegisterModel model)
    {
        try
        {
            var user = await _userProcessingService.InsertAsync(model);
            var token = await _tokenService.GenerateUserTokenAsync(user);
            return new ApiResult(token);
        }
        catch (Exception e)
        {
            _logger.Error("Błąd rejestracji użytkownika", e, model);
            return ApiResult.Error("Wystąpił błąd rejestracji");
        }
    }
}
}
