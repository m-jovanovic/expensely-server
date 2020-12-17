using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Expensely.Domain.Abstractions.Maybe;

namespace Expensely.Application.Abstractions.Data
{
    /// <summary>
    /// Represents the reporting database context interface.
    /// </summary>
    public interface IReportingDbContext : IDbContext
    {
        /// <summary>
        /// Gets the entity that satisfies the specified predicate, if it exists.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <param name="predicate">The predicate.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// The maybe instance that may contain the <typeparamref name="TEntity"/> that satisfies the specified predicate.
        /// </returns>
        Task<Maybe<TEntity>> FirstOrDefaultAsync<TEntity>(
            Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
            where TEntity : class;

        /// <summary>
        /// Checks if any entity satisfies the predicate.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <param name="predicate">The predicate.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>True if any entity satisfies the predicate, otherwise false.</returns>
        Task<bool> AnyAsync<TEntity>(
            Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
            where TEntity : class;
    }
}
