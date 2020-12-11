using System;
using System.Linq.Expressions;

namespace Expensely.Application.Abstractions.Specifications
{
    /// <summary>
    /// Represents the base class for a specification.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    public abstract class Specification<TEntity>
        where TEntity : class
    {
        /// <summary>
        /// Gets the order by expression.
        /// </summary>
        public Expression<Func<TEntity, object>> OrderByExpression { get; private set; }

        /// <summary>
        /// Gets the take value.
        /// </summary>
        public int Take { get; }

        public static implicit operator Expression<Func<TEntity, bool>>(Specification<TEntity> specification) =>
            specification.ToExpression();

        /// <summary>
        /// Converts the current specification instance to a boolean expression.
        /// </summary>
        /// <returns>The boolean expression defined by the current specification instance.</returns>
        public abstract Expression<Func<TEntity, bool>> ToExpression();

        /// <summary>
        /// Applies the specified order by expression.
        /// </summary>
        /// <param name="orderByExpression">The order by expression.</param>
        protected void ApplyOrderBy(Expression<Func<TEntity, object>> orderByExpression) =>
            OrderByExpression = orderByExpression;
    }
}
