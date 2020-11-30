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
        public static implicit operator Expression<Func<TEntity, bool>>(Specification<TEntity> specification) =>
            specification.ToExpression();

        /// <summary>
        /// Converts the current specification instance to a boolean expression.
        /// </summary>
        /// <returns>The boolean expression defined by the current specification instance.</returns>
        public abstract Expression<Func<TEntity, bool>> ToExpression();
    }
}
