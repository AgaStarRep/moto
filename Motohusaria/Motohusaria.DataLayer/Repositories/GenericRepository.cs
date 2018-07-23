using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Motohusaria.DomainClasses;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Motohusaria.DataLayer.Repositories
{
    public class GenericRepository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly MotohusariaDbContext _db;

        public string EntityName => typeof(TEntity).Name;

        public GenericRepository(MotohusariaDbContext db)
        {
            _db = db;
        }

        public IQueryable<TEntity> Table => _db.Set<TEntity>();

        public IQueryable<TEntity> TableAsNoTracking => _db.Set<TEntity>().AsNoTracking();

        public IQueryable<TEntity> FromSql(string sql, params object[] parms)
        {
            return _db.Set<TEntity>().FromSql(sql, parms);
        }

        public async Task DeleteAsync(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            var isDetached = !_db.Set<TEntity>().Local.Any(a => a.Id == entity.Id);
            if (isDetached)
            {
                _db.Entry(entity).State = EntityState.Modified;
            }
            _db.Set<TEntity>().Remove(entity);
            await _db.SaveChangesAsync();
        }

        /// <summary>
        /// Asynchroniczne pobranie wszystkich encji
        /// </summary>
        /// <returns>Wszystkie encje</returns>
        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await Table.ToArrayAsync();
        }

        /// <summary>
        /// Asynchroniczne pobranie encji po id
        /// </summary>
        /// <param name="id">id encji</param>
        /// <returns>Encja</returns>
        public async Task<TEntity> GetByIdAsync(Guid id)
        {
            return await Table.SingleOrDefaultAsync(s => s.Id == id);
        }

        /// <summary>
        /// Asynchroniczne pobranie encji po id
        /// </summary>
        /// <param name="id">id encji</param>
        /// <returns>Encja</returns>
        public async Task<TEntity> GetByIdAsync(Guid id, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes)
        {
            return await includes(Table).SingleOrDefaultAsync(s => s.Id == id);
        }

        /// <summary>
        /// Asynchroniczne dodanie encji
        /// </summary>
        /// <param name="entity">Encja</param>
        /// <returns>void</returns>
        public async Task InsertAsync(TEntity entity)
        {
            _db.Set<TEntity>().Add(entity);
            await _db.SaveChangesAsync();
        }

        /// <summary>
        /// Asychroniczna aktualizacja encji
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>void</returns>
        public async Task UpdateAsync(TEntity entity)
        {
            var isAttached = _db.Set<TEntity>().Local.Any(a => a.Id == entity.Id);
            if (!isAttached)
            {
                _db.Entry(entity).State = EntityState.Modified;
            }
            await _db.SaveChangesAsync();
        }
    }
}
