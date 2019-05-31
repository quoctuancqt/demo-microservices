namespace Core.Extensions
{
    using Core.Interfaces;
    using Core.Specifications;
    using Core.Entities;
    using System.Linq;

    public static class EfExtension
    {
        public static IQueryable<T> ApplySpecification<T>(this IQueryable<T> query, ISpecification<T> spec) where T : EntityBase, IEntity
        {
            return SpecificationEvaluator<T>.GetQuery(query, spec);
        }
    }
}
