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
    [InjectableService(typeof(IRoleProcessingService))]
public class RoleProcessingService : IRoleProcessingService
{
    private readonly IMapper _mapper;
    private readonly IRoleService _roleService;
    private readonly ITransactionProvider _transactionProvider;
    private readonly IRepository<UserRole> _userRoleRepository;
    private readonly IUserRoleService _userRoleService;
    private readonly INotificationService _notificationService;
    private readonly IEventPublisher _eventPublisher;

    public RoleProcessingService(IMapper mapper, IRoleService roleService, ITransactionProvider transactionProvider, IRepository<UserRole> userRoleRepository, IUserRoleService userRoleService, INotificationService notificationService, IEventPublisher eventPublisher)
    {
        _mapper = mapper;
        _roleService = roleService;
        _transactionProvider = transactionProvider;
        _userRoleRepository = userRoleRepository;
        _userRoleService = userRoleService;
        _notificationService = notificationService;
        _eventPublisher = eventPublisher;
    }

    public async Task InsertAsync(RoleViewModel model)
    {
        if (!_eventPublisher.Publish(new BeforeEntityInsert<RoleViewModel>(model)))
            return;

        var entity = _mapper.Map<Role>(model);
        entity.Id = Guid.NewGuid();
        if (model.RoleUsersSelectedIds != null)
        {
            entity.RoleUsers = model.RoleUsersSelectedIds.Select(s => new UserRole { Id = Guid.NewGuid(), RoleId = entity.Id, UserId = s }).ToArray();
        }

        using (var transaction = _transactionProvider.BeginTransaction())
        {
            if (!_eventPublisher.Publish(new BeforeEntityInsertTransaction<Role>(entity)))
                return;
            await _roleService.InsertAsync(entity);

            if (!_eventPublisher.Publish(new AfterEntityInsertTransaction<Role>(entity)))
                return;
            transaction.Commit();
        }
        if (!_eventPublisher.Publish(new AfterEntityInsert<Role>(entity)))
            return;
        _notificationService.Success("Encja została dodana");
    }

    public async Task UpdateAsync(RoleViewModel model)
    {
        if (!_eventPublisher.Publish(new BeforeEntityUpdate<RoleViewModel>(model)))
            return;

        var entity = await _roleService.GetByIdAsync(model.Id.Value);
        _mapper.Map(model, entity);
        if (model.RoleUsersSelectedIds == null)
        {
            model.RoleUsersSelectedIds = new Guid[0];
        }
        var oldRoleUsersRelations = _userRoleRepository.TableAsNoTracking.Where(w => w.RoleId == entity.Id).ToDictionary(d => d.UserId);
        var roleUsersToAdd = model.RoleUsersSelectedIds.Where(w => !oldRoleUsersRelations.ContainsKey(w));
        var roleUsersToRemove = oldRoleUsersRelations.Where(w => !model.RoleUsersSelectedIds.Contains(w.Key));

        using (var transaction = _transactionProvider.BeginTransaction())
        {
            if (!_eventPublisher.Publish(new BeforeEntityUpdateTransaction<Role>(entity)))
                return;

            foreach (var toAdd in roleUsersToAdd)
            {
                await _userRoleService.InsertAsync(new UserRole { Id = Guid.NewGuid(), RoleId = entity.Id, UserId = toAdd });
            }
            foreach (var toRemove in roleUsersToRemove)
            {
                await _userRoleService.DeleteAsync(toRemove.Value);
            }

            await _roleService.UpdateAsync(entity);
            if (!_eventPublisher.Publish(new AfterEntityUpdateTransaction<Role>(entity)))
                return;
            transaction.Commit();
        }
        if (!_eventPublisher.Publish(new AfterEntityUpdate<Role>(entity)))
            return;
        _notificationService.Success("Zapisano zmiany.");
    }


    public async Task DeleteAsync(Guid id)
    {
        var entity = await _roleService.GetByIdAsync(id);
        if (!_eventPublisher.Publish(new BeforeEntityDelete<Role>(entity)))
            return;
        await _roleService.DeleteAsync(entity);
        if (!_eventPublisher.Publish(new AfterEntityDelete<Role>(entity)))
            return;
        _notificationService.Success("Encja została usunięta.");
    }

}
}