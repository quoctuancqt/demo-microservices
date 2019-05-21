namespace Core.Repositories
{
    using Core.Extensions;
    using Core.Interfaces;
    using Core.Entities;
    using Microsoft.EntityFrameworkCore;
    using System.Linq;
    using System.Threading.Tasks;

    public abstract class ReadOnlyRepository<T, TContext> : IReadOnlyRepository<T, TContext>
         where T : EntityBase, IEntity
        where TContext : DbContext
    {
        private TContext _context;

        public ReadOnlyRepository(TContext context)
        {
            _context = context;
        }

        public virtual IQueryable<T> FindAll(ISpecification<T> spec)
        {
            return _context.Set<T>().AsQueryable().ApplySpecification(spec);
        }

        public virtual async Task<T> FindByAsync(string id)
        {
            return await _context.Set<T>().FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

    }
}
