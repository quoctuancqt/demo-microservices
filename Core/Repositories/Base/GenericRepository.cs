namespace Core.Repositories
{
    using Core.Entities;
    using Microsoft.EntityFrameworkCore;

    public sealed class GenericRepository<T, TContext> : Repository<T, TContext>, IRepository<T, TContext>
        where T : EntityBase, IEntity
        where TContext : DbContext
    {
        public GenericRepository(TContext context) : base(context)
        {
        }
    }
}
