namespace Core.Repositories
{
    using Core.Entities;
    using Microsoft.EntityFrameworkCore;

    public sealed class GenericReadOnlyRepository<T, TContext> : ReadOnlyRepository<T, TContext>, IReadOnlyRepository<T, TContext>
        where T : EntityBase, IEntity
        where TContext : DbContext
    {
        public GenericReadOnlyRepository(TContext context) : base(context)
        {
        }
    }

}
