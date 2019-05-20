namespace Core.Repositories
{
    using Core.Entities;
    using Microsoft.EntityFrameworkCore;

    public sealed class GenericReadOnlyRepository<T> : ReadOnlyRepository<T>, IReadOnlyRepository<T> where T : EntityBase, IEntity
    {
        public GenericReadOnlyRepository(DbContext context) : base(context)
        {
        }
    }

}
