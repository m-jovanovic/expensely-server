using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Expensely.Domain.Primitives;
using Expensely.Domain.Primitives.Maybe;
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
            where TEntity : Entity;

        /// <summary>
        /// Gets the entity with the specified identifier.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <param name="id">The entity identifier.</param>
        /// <returns>The maybe instance that may contain the <typeparamref name="TEntity"/> with the specified identifier.</returns>
        Task<Maybe<TEntity>> GetBydIdAsync<TEntity>(Guid id)
            where TEntity : Entity;

        /// <summary>
        /// Checks if any entity satisfies the specified predicate.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <param name="predicate">The predicate.</param>
        /// <returns>True if any entity satisfies the specified predicate, otherwise false.</returns>
        Task<bool> AnyAsync<TEntity>(Expression<Func<TEntity, bool>> predicate)
            where TEntity : Entity;

        /// <summary>
        /// Inserts the specified entity into the database.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <param name="entity">The entity to be inserted into the database.</param>
        void Insert<TEntity>(TEntity entity)
            where TEntity : Entity;

        /// <summary>
        /// Removes the specified entity from the database.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <param name="entity">The entity to be removed from the database.</param>
        void Remove<TEntity>(TEntity entity)
            where TEntity : Entity;

        /// <summary>
        /// Saves all of the pending changes in the unit of work.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The number of entities that have been saved.</returns>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
