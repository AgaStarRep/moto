using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Motohusaria.DomainClasses;

namespace Motohusaria.Services
{
    /// <summary>
    /// Podstawowy serwis z operacjami crud
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IService<TEntity> where TEntity : BaseEntity
    {
        /// <summary>
        /// Dodaje encje do bazy danych.
        /// </summary>
        /// <param name="entity">Encja do dodania</param>
        /// <returns></returns>
        Task InsertAsync(TEntity entity);

        /// <summary>
        /// Usuwanie z bazy danych
        /// </summary>
        /// <param name="entity">Encja do usunięcia</param>
        /// <returns></returns>
        Task DeleteAsync(TEntity entity);

        /// <summary>
        /// Aktualizuje encje z bazy danych
        /// </summary>
        /// <param name="entity">Encja do aktualizacji</param>
        /// <returns></returns>
        Task UpdateAsync(TEntity entity);

        /// <summary>
        /// Pobiera wszystkie encje.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<IEnumerable<TEntity>> GetAllAsync();

        /// <summary>
        /// Pobiera encje o podanym Id.
        /// </summary>
        /// <param name="id">Identyfikator encji</param>
        /// <returns></returns>
        Task<TEntity> GetByIdAsync(Guid id);

        /// <summary>
        /// Pobiera encje o podanym Id z includami.
        /// </summary>
        /// <param name="id">Identyfikator encji</param>
        /// <returns></returns>
        Task<TEntity> GetByIdAsync(Guid id, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes);

    }
}
