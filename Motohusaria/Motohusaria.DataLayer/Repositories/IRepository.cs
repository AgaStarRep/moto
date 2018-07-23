using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Motohusaria.DomainClasses;
using System.Threading.Tasks;

namespace Motohusaria.DataLayer.Repositories
{
    public interface IRepository<TEntity> where TEntity : BaseEntity
    {
        IQueryable<TEntity> Table { get; }

        IQueryable<TEntity> TableAsNoTracking { get; }

        string EntityName { get; }

        IQueryable<TEntity> FromSql(string sql, params object[] parms);

        Task<TEntity> GetByIdAsync(Guid id);

        Task<TEntity> GetByIdAsync(Guid id, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes);

        Task<IEnumerable<TEntity>> GetAllAsync();

        Task InsertAsync(TEntity entity);

        Task UpdateAsync(TEntity entity);

        Task DeleteAsync(TEntity entity);

    }
}
