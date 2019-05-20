namespace Core.Repositories
{
    using Core.Interfaces;
    using Core.Entities;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public interface IRepository<T> where T : EntityBase, IEntity
    {
        IQueryable<T> FindAll();

        IQueryable<T> FindAll(ISpecification<T> spec);

        IQueryable<T> AsNoTracking(ISpecification<T> spec);

        T FindBy(string id);

        Task<T> FindByAsync(string id);

        Task<T> FindSingleAsync(ISpecification<T> spec);

        Task<T> AddAsync(T entity);

        void Add(T entity);

        Task EditAsync(T entity);

        void Edit(T entity);

        void AddRange(IEnumerable<T> entities);

        void EditRange(IEnumerable<T> entities);

        void Remove(T entity);

        void RemoveRange(IEnumerable<T> entities);
    }
}
