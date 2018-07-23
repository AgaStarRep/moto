using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Motohusaria.DTO;
using Motohusaria.DataLayer.Utils;
using Motohusaria.DataLayer.Repositories;
using System.Threading.Tasks;
using Motohusaria.DomainClasses;
using Motohusaria.Services.Events;

namespace Motohusaria.Services
{
    [InjectableService(typeof(IUserProcessingService))]
public class UserProcessingService : IUserProcessingService
{
    private readonly IMapper _mapper;
    private readonly IUserService _userService;
    private readonly ITransactionProvider _transactionProvider;
    private readonly IRepository<UserRole> _userRoleRepository;
    private readonly IUserRoleService _userRoleService;
    private readonly IPasswordService _passwordService;
    private readonly INotificationService _notificationService;
    private readonly IEventPublisher _eventPublisher;

    public UserProcessingService(IMapper mapper, IUserService userService, ITransactionProvider transactionProvider, IRepository<UserRole> userRoleRepository, IUserRoleService userRoleService, IPasswordService passwordService, INotificationService notificationService, IEventPublisher eventPublisher)
    {
        _mapper = mapper;
        _userService = userService;
        _transactionProvider = transactionProvider;
        _userRoleRepository = userRoleRepository;
        _userRoleService = userRoleService;
        _passwordService = passwordService;
        _notificationService = notificationService;
        _eventPublisher = eventPublisher;
    }

    public async Task<User> InsertAsync(RegisterModel model)
    {
        string salt = string.Empty;
        var passwordHash = _passwordService.HashPassword(model.Password, ref salt);
        var entity = new User
        {
            Id = Guid.NewGuid(),
            Login = model.Login,
            PasswordHash = passwordHash,
            Salt = salt,
        };
        await _userService.InsertAsync(entity);
        _notificationService.Success("Poprawnie zarejestroawno użytkownika");
        return entity;
    }

    public async Task InsertAsync(UserViewModel model)
    {
        if (!_eventPublisher.Publish(new BeforeEntityInsert<UserViewModel>(model)))
            return;

        var entity = _mapper.Map<User>(model);
        entity.Id = Guid.NewGuid();
        if (model.UserRolesSelectedIds != null)
        {
            entity.UserRoles = model.UserRolesSelectedIds.Select(s => new UserRole { Id = Guid.NewGuid(), UserId = entity.Id, RoleId = s }).ToArray();
        }
        var salt = "";
        entity.PasswordHash = _passwordService.HashPassword(model.Password, ref salt);
        entity.Salt = salt;
        using (var transaction = _transactionProvider.BeginTransaction())
        {
            if (!_eventPublisher.Publish(new BeforeEntityInsertTransaction<User>(entity)))
                return;
            await _userService.InsertAsync(entity);
            if (!_eventPublisher.Publish(new AfterEntityInsertTransaction<User>(entity)))
                return;
            transaction.Commit();
        }
        if (!_eventPublisher.Publish(new AfterEntityInsert<User>(entity)))
            return;
        _notificationService.Success("Uzytkownik został dodany");
    }

    public async Task UpdateAsync(UserViewModel model)
    {
        if (!_eventPublisher.Publish(new BeforeEntityUpdate<UserViewModel>(model)))
            return;

        var entity = await _userService.GetByIdAsync(model.Id.Value);
        _mapper.Map(model, entity);
        if (model.UserRolesSelectedIds == null)
        {
            model.UserRolesSelectedIds = new Guid[0];
        }
        var oldUserRolesRelations = _userRoleRepository.TableAsNoTracking.Where(w => w.UserId == entity.Id).ToDictionary(d => d.RoleId);
        var userRolesToAdd = model.UserRolesSelectedIds.Where(w => !oldUserRolesRelations.ContainsKey(w));
        var userRolesToRemove = oldUserRolesRelations.Where(w => !model.UserRolesSelectedIds.Contains(w.Key));

        if (model.Password != "******")
        {
            var salt = "";
            entity.PasswordHash = _passwordService.HashPassword(model.Password, ref salt);
            entity.Salt = salt;
        }

        using (var transaction = _transactionProvider.BeginTransaction())
        {
            if (!_eventPublisher.Publish(new BeforeEntityUpdateTransaction<User>(entity)))
                return;
            foreach (var toAdd in userRolesToAdd)
            {
                await _userRoleService.InsertAsync(new UserRole { Id = Guid.NewGuid(), UserId = entity.Id, RoleId = toAdd });
            }
            foreach (var toRemove in userRolesToRemove)
            {
                await _userRoleService.DeleteAsync(toRemove.Value);
            }

            await _userService.UpdateAsync(entity);
            if (!_eventPublisher.Publish(new AfterEntityUpdateTransaction<User>(entity)))
                return;
            transaction.Commit();
        }
        if (!_eventPublisher.Publish(new AfterEntityUpdate<User>(entity)))
            return;
        _notificationService.Success("Uzytkownik został aktualizowany.");
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await _userService.GetByIdAsync(id);
        if (!_eventPublisher.Publish(new BeforeEntityDelete<User>(entity)))
            return;
        await _userService.DeleteAsync(entity);
        if (!_eventPublisher.Publish(new AfterEntityDelete<User>(entity)))
            return;
        _notificationService.Success("Uzytkownik został usunięty.");
    }
}
}