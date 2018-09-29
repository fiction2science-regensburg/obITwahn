using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using obITwahn.Services.Common;

namespace ObITwahn.Trinity.Services.Web.Data
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : EntityBase
    {
        private readonly TrinityContext _context;
        private readonly DbSet<TEntity> _dbSet;
        private readonly AggregateIncludesInitializer _aggregateIncludesInitailizer = new AggregateIncludesInitializer();

        public Repository(TrinityContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        public async Task DeleteAsync(Guid? id)
        {
            TEntity entityToDelete = await _dbSet.FindAsync(id);

            if (entityToDelete != null)
            {
                _context.Remove(entityToDelete);

                await _context.SaveChangesAsync();
            }
        }

        public async Task<TEntity> GetAsync(Guid? id)
        {
            TEntity foundEntity = await _aggregateIncludesInitailizer.IncludePropertiesWhenNeeded(_dbSet)
                .Where(entitiy => entitiy.Id == id)
                .SingleOrDefaultAsync();

            if (foundEntity == null)
            {
                throw new Exception($"There is no entity with id '{id}");
            }

            return foundEntity;
        }

        public async Task SaveOrUpdateAsync(TEntity entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            bool isNew = !await _dbSet.AnyAsync(e => e.Id == entity.Id);

            _context.ChangeTracker.TrackGraph(entity, e =>
            {
                EntityBase currentEntity = e.Entry.Entity as EntityBase;

                if (currentEntity != null)
                {
                    if (currentEntity.Id != Guid.Empty)
                    {
                        e.Entry.State = EntityState.Modified;
                    }
                    else
                    {
                        e.Entry.State = EntityState.Added;
                        currentEntity.Id = Guid.NewGuid();
                    }

                }
            });

            if (isNew)
            {
                _dbSet.Add(entity); // Don't use AddAsync ... changetracker is not working / called
            }
            else
            {
                _dbSet.Update(entity);
            }



            await _context.SaveChangesAsync();
        }

        public async Task<List<TEntity>> GetAllAsync()
        {
            return await _aggregateIncludesInitailizer.IncludePropertiesWhenNeeded(_dbSet)
                .ToListAsync();
        }

        public async Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> filter)
        {
            return await _aggregateIncludesInitailizer.IncludePropertiesWhenNeeded(_dbSet)
                .Where(filter)
                .ToListAsync();
        }

        public async Task<int> CountAsync()
        {
            return await _aggregateIncludesInitailizer.IncludePropertiesWhenNeeded(_dbSet)
                .CountAsync();
        }

        public async Task<int> CountAsync(Expression<Func<TEntity, bool>> filter)
        {
            return await _aggregateIncludesInitailizer.IncludePropertiesWhenNeeded(_dbSet)
                .Where(filter)
                .CountAsync();
        }
    }
}