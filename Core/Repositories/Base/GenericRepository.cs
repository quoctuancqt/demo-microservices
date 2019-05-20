namespace Core.Repositories
{
    using Core.Entities;
    using Microsoft.EntityFrameworkCore;

    public sealed class GenericRepository<T> : Repository<T>, IRepository<T> where T : EntityBase, IEntity
    {
        public GenericRepository(DbContext context) : base(context)
        {
        }
    }
}
