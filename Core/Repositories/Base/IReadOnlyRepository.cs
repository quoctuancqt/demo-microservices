namespace Core.Repositories
{
    using Core.Interfaces;
    using Core.Entities;
    using System.Linq;
    using System.Threading.Tasks;

    public interface IReadOnlyRepository<T> where T: IEntity
    {
        IQueryable<T> FindAll(ISpecification<T> spec);

        Task<T> FindByAsync(string id);
    }
}
