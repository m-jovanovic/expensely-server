using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Abstractions.Specifications;
using Expensely.Domain.Abstractions.Maybe;
using Expensely.Domain.Abstractions.Primitives;
using Microsoft.EntityFrameworkCore;

namespace Expensely.Application.Abstractions.Data
{
    /// <summary>
    /// Represents the database context interface.
    /// </summary>
    public interface IDbContext
    {
        /// <summary>
        /// Gets the database set for the specified entity type.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <returns>The database set for the specified entity type.</returns>
        DbSet<TEntity> Set<TEntity>()
            where TEntity : class;

        /// <summary>
        /// Gets the entity with the specified identifier, if it exists.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <param name="id">The entity identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The maybe instance that may contain the <typeparamref name="TEntity"/> with the specified identifier.</returns>
        Task<Maybe<TEntity>> GetBydIdAsync<TEntity>(Guid id, CancellationToken cancellationToken = default)
            where TEntity : Entity;

        /// <summary>
        /// Gets the entity that satisfies the specified specification, if it exists.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <param name="specification">The specification.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// The maybe instance that may contain the <typeparamref name="TEntity"/> that satisfies the specified specification.
        /// </returns>
        Task<Maybe<TEntity>> FirstOrDefaultAsync<TEntity>(
            Specification<TEntity> specification, CancellationToken cancellationToken = default)
            where TEntity : class;

        /// <summary>
        /// Gets the entities that satisfy the specified specification, if any exist.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <param name="specification">The specification.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The entities that satisfy the specified specification.</returns>
        Task<IList<TEntity>> ListAsync<TEntity>(Specification<TEntity> specification, CancellationToken cancellationToken = default)
            where TEntity : class;

        /// <summary>
        /// Checks if any entity satisfies the specification.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <param name="specification">The specification.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>True if any entity satisfies the specification, otherwise false.</returns>
        Task<bool> AnyAsync<TEntity>(Specification<TEntity> specification, CancellationToken cancellationToken = default)
            where TEntity : class;

        /// <summary>
        /// Inserts the specified entity into the database.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <param name="entity">The entity to be inserted into the database.</param>
        void Insert<TEntity>(TEntity entity)
            where TEntity : class;

        /// <summary>
        /// Removes the specified entity from the database.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <param name="entity">The entity to be removed from the database.</param>
        void Remove<TEntity>(TEntity entity)
            where TEntity : class;

        /// <summary>
        /// Saves all of the pending changes in the unit of work.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The number of entities that have been saved.</returns>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
