using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Motohusaria.DomainClasses;
using Motohusaria.DataLayer.Repositories;
using Motohusaria.Services.Events;

namespace Motohusaria.Services
{
        public abstract class BaseCachedService<TEntity> : IService<TEntity> where TEntity : BaseEntity
{
    protected readonly IRepository<TEntity> _repository;
    protected readonly ICacheService _cacheService;
    private IRepository<UserRole> repository;
    private ICacheService cacheService;

    protected BaseCachedService(IRepository<TEntity> repository, ICacheService cacheService)
    {
        _repository = repository;
        _cacheService = cacheService;
    }

    protected BaseCachedService(IRepository<UserRole> repository, ICacheService cacheService)
    {
        this.repository = repository;
        this.cacheService = cacheService;
    }


    /// <summary>
    /// Klucz pod którym keszowane są encje z GetAll
    /// </summary>
    protected abstract string GetAllKey { get; }
    /// <summary>
    /// Na ile minut cache'owane są encje z GetAll
    /// </summary>
    protected virtual int GetAllCacheTime { get; set; } = 60;

    /// <summary>
    /// Klucz pod którym keszowane są pojedyńcze encje
    /// </summary>
    protected abstract string GetByIdKey { get; }
    /// <summary>
    /// Na ile minut cache'owane są pojedyńcze encje
    /// </summary>
    protected virtual int GetByIdCacheTime { get; set; } = 60;

    /// <summary>
    /// Dodaje encje do bazy danych.
    /// </summary>
    /// <param name="entity">Encja do dodania</param>
    /// <returns></returns>
    public virtual async Task InsertAsync(TEntity entity)
    {
        await _repository.InsertAsync(entity);
        ClearAllCacheKeys(entity.Id);
    }

    /// <summary>
    /// Usuwanie z bazy danych
    /// </summary>
    /// <param name="entity">Encja do usunięcia</param>
    /// <returns></returns>
    public virtual async Task DeleteAsync(TEntity entity)
    {
        await _repository.DeleteAsync(entity);
        ClearAllCacheKeys(entity.Id);
    }

    /// <summary>
    /// Aktualizuje encje z bazy danych
    /// </summary>
    /// <param name="entity">Encja do aktualizacji</param>
    /// <returns></returns>
    public virtual async Task UpdateAsync(TEntity entity)
    {
        await _repository.UpdateAsync(entity);
        ClearAllCacheKeys(entity.Id);
    }

    /// <summary>
    /// Pobiera wszystkie encje z cache lub bazy danych jezeli nie ma ich w cache
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        var cachedData = _cacheService.Get<IEnumerable<TEntity>>(GetAllKey);
        if (cachedData == null)
        {
            var data = await _repository.GetAllAsync();
            _cacheService.Store(GetAllKey, data, GetAllCacheTime);
            return data;
        }
        return cachedData;
    }

    /// <summary>
    /// Pobiera encje o podanym Id z cache lub bazy danych jezeli nie ma jej w cache
    /// </summary>
    /// <param name="id">Identyfikator encji</param>
    /// <returns></returns>
    public virtual async Task<TEntity> GetByIdAsync(Guid id)
    {
        var cachedData = _cacheService.Get<TEntity>(GetByIdKey);
        if (cachedData == null)
        {
            var data = await _repository.GetByIdAsync(id);
            _cacheService.Store(GetByIdKey + id.ToString(), data, GetByIdCacheTime);
            return data;
        }
        return cachedData;
    }

    /// <summary>
    /// Czyści cały cache związany z encją.
    /// </summary>
    /// <param name="entityId"></param>
    protected void ClearAllCacheKeys(Guid? entityId = null)
    {
        _cacheService.DeleteKeysStartingWith(typeof(TEntity).Name + ".");
    }

    /// <summary>
    /// Zwraca obiekt o podanym typie po kluczu, jeżeli obiektu nie ma to wykonuje podaną funkcje i jej rezultat zapisuje do cache.
    /// </summary>
    /// <typeparam name="T">Typ obiektu</typeparam>
    /// <param name="key">Klucz do cache</param>
    /// <param name="cacheTime">Na jak długo obiekt jest cache'owany w minutach</param>
    /// <param name="functionToRunIfNotInCache">Asynchroniczna Funkcja wykonywana jeżeli nie ma obiektu w cache</param>
    /// <returns></returns>
    protected virtual async Task<T> GetCachedAsync<T>(string key, int cacheTime, Func<Task<T>> functionToRunIfNotInCache) where T : class
    {
        var cachedData = _cacheService.Get<T>(key);
        if (cachedData == null)
        {
            var data = await functionToRunIfNotInCache.Invoke();
            if (data != null)
            {
                _cacheService.Store(key, data, cacheTime);
            }
            return data;
        }
        return cachedData;
    }

    public async Task<TEntity> GetByIdAsync(Guid id, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes)
    {
        return await _repository.GetByIdAsync(id, includes);
    }
}
}
