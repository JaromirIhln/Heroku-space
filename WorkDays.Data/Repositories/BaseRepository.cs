using System.Collections.Generic;
using System.Threading.Tasks;
using WorkDays.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;

namespace WorkDays.Data.Repositories
{

    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        protected readonly AppDbContext _context;
        protected readonly DbSet<TEntity> _dbSet;

        public BaseRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
        }
        public virtual Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return Task.FromResult(_context.Set<TEntity>().AsEnumerable());
        }

        public virtual Task<TEntity?> GetByIdAsync(int id)
        {
             TEntity entity = _context.Find<TEntity>(id);
            if (entity == null)
            {
                throw new KeyNotFoundException($"Entity with id {id} not found.");
            }
            _context.Entry(entity).State = EntityState.Detached; // Detach to prevent tracking issues
            return Task.FromResult(entity);
        }

        public virtual Task<TEntity> AddAsync(TEntity entity)
        {
            _context.Set<TEntity>().Add(entity);
            _context.SaveChanges();
            return Task.FromResult(entity);
        }

        public virtual Task<TEntity> UpdateAsync(TEntity entity)
        {
            _context.Set<TEntity>().Update(entity);
            _context.SaveChanges();
            return Task.FromResult(entity);
        }

        public virtual Task? DeleteAsync(int id)
        {
            TEntity? entity = _context.Find<TEntity>(id);
            if (entity is null)
                return null;
            try
            {
                _context.Entry(entity).State = EntityState.Unchanged; // Detach to prevent tracking issues
            }
            catch (Exception)
            {
                // No logging framework is set up, so this is commented out
                // Logger.LogWarning($"Entity with id {id} could not be detached.");
                throw new KeyNotFoundException($"Entity with id {id} not found.");
            }
            _context.Set<TEntity>().Remove(entity);
            _context.SaveChanges();
            return Task.CompletedTask;
        }
    }
}
