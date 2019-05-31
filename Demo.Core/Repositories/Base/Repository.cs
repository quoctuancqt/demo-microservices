namespace Core.Repositories
{
    using Core.Extensions;
    using Core.Interfaces;
    using Core.Entities;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public abstract class Repository<T, TContext> : IRepository<T, TContext>
        where T : EntityBase, IEntity
        where TContext : DbContext
    {
        protected readonly TContext _context;

        public Repository(TContext context)
        {
            _context = context;
        }

        public virtual Task<T> AddAsync(T entity)
        {
            throw new NotImplementedException();
        }

        public virtual void Add(T entity)
        {
            _context.Set<T>().Add(entity);
        }

        public virtual void AddRange(IEnumerable<T> entities)
        {
            _context.Set<T>().AddRange(entities);
        }

        public virtual void Remove(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            _context.Set<T>().RemoveRange(entities);
        }

        public virtual void Edit(T entity)
        {
            _context.Set<T>().Update(entity);
        }

        public virtual Task EditAsync(T entity)
        {
            throw new NotImplementedException();

        }

        public virtual void EditRange(IEnumerable<T> entities)
        {
            _context.Set<T>().UpdateRange(entities);
        }

        public virtual T FindBy(string id)
        {
            return _context.Set<T>().FirstOrDefault(x => x.Id.Equals(id));
        }

        public virtual IQueryable<T> FindAll()
        {
            return _context.Set<T>().AsQueryable();
        }

        public virtual IQueryable<T> FindAll(ISpecification<T> spec)
        {
            return FindAll().ApplySpecification(spec);
        }

        public virtual async Task<T> FindByAsync(string id)
        {
            return await _context.Set<T>().FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public virtual async Task<T> FindSingleAsync(ISpecification<T> spec)
        {
            return await _context.Set<T>().ApplySpecification(spec).SingleOrDefaultAsync();
        }

        public virtual IQueryable<T> AsNoTracking(ISpecification<T> spec)
        {
            return _context.Set<T>().ApplySpecification(spec).AsNoTracking();
        }

    }
}
