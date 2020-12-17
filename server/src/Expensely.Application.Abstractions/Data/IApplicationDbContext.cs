using System;
using System.Threading;
using System.Threading.Tasks;
using Expensely.Domain.Abstractions.Maybe;
using Expensely.Domain.Abstractions.Primitives;

namespace Expensely.Application.Abstractions.Data
{
    /// <summary>
    /// Represents the application database context interface.
    /// </summary>
    public interface IApplicationDbContext : IDbContext
    {
        /// <summary>
        /// Gets the entity with the specified identifier, if it exists.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <param name="id">The entity identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The maybe instance that may contain the <typeparamref name="TEntity"/> with the specified identifier.</returns>
        Task<Maybe<TEntity>> GetBydIdAsync<TEntity>(Guid id, CancellationToken cancellationToken = default)
            where TEntity : Entity;
    }
}
