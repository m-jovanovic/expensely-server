using System.Linq;
using Expensely.Application.Abstractions.Specifications;

namespace Expensely.Persistence.Infrastructure
{
    /// <summary>
    /// Represents the specification evaluator.
    /// </summary>
    public static class SpecificationEvaluator
    {
        /// <summary>
        /// Gets the resulting queryable when the provided specification instance is applied to the specified queryable.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <param name="queryable">The queryable.</param>
        /// <param name="specification">The specification.</param>
        /// <returns>The resulting queryable when the provided specification is applied to the initial queryable.</returns>
        public static IQueryable<TEntity> GetQuery<TEntity>(IQueryable<TEntity> queryable, Specification<TEntity> specification)
            where TEntity : class
        {
            IQueryable<TEntity> result = queryable;

            result = result.Where(specification.ToExpression());

            if (specification.OrderByExpression is not null)
            {
                result = result.OrderBy(specification.OrderByExpression);
            }

            if (specification.Take > 0)
            {
                result = result.Take(specification.Take);
            }

            return result;
        }
    }
}
