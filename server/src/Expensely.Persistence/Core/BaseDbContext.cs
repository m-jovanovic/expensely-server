using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Abstractions.Data;
using Expensely.Application.Abstractions.Specifications;
using Expensely.Domain.Abstractions.Maybe;
using Expensely.Persistence.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Expensely.Persistence.Core
{
    /// <summary>
    /// Represents base database context.
    /// </summary>
    public abstract class BaseDbContext : DbContext, IDbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseDbContext"/> class.
        /// </summary>
        /// <param name="options">The database context options.</param>
        protected BaseDbContext(DbContextOptions options)
            : base(options)
        {
        }

        /// <inheritdoc />
        public new DbSet<TEntity> Set<TEntity>()
            where TEntity : class =>
            base.Set<TEntity>();

        /// <inheritdoc />
        public async Task<Maybe<TEntity>> FirstOrDefaultAsync<TEntity>(
            Specification<TEntity> specification, CancellationToken cancellationToken = default)
            where TEntity : class =>
            await Set<TEntity>().FirstOrDefaultAsync(specification, cancellationToken);

        /// <inheritdoc />
        public async Task<IList<TEntity>> ListAsync<TEntity>(
            Specification<TEntity> specification, CancellationToken cancellationToken = default)
            where TEntity : class =>
            await SpecificationEvaluator.GetQuery(Set<TEntity>(), specification).ToListAsync(cancellationToken);

        /// <inheritdoc />
        public async Task<bool> AnyAsync<TEntity>(Specification<TEntity> specification, CancellationToken cancellationToken = default)
            where TEntity : class =>
            await Set<TEntity>().AnyAsync(specification, cancellationToken);

        /// <inheritdoc />
        public void Insert<TEntity>(TEntity entity)
            where TEntity : class =>
            Set<TEntity>().Add(entity);

        /// <inheritdoc />
        public new void Remove<TEntity>(TEntity entity)
            where TEntity : class =>
            Set<TEntity>().Remove(entity);
    }
}
