namespace Core.Repositories
{
    using Core.Interfaces;
    using Core.Entities;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;

    public interface IReadOnlyRepository<T, TContext> 
        where T : IEntity 
        where TContext : DbContext
    {
        IQueryable<T> FindAll(ISpecification<T> spec);

        Task<T> FindByAsync(string id);
    }
}
