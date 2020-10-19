using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Abstractions.Common;
using Expensely.Application.Abstractions.Data;
using Expensely.Domain.Abstractions;
using Expensely.Domain.Primitives;
using Expensely.Domain.Primitives.Maybe;
using Expensely.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Expensely.Persistence
{
    /// <summary>
    /// Represents the applications database context.
    /// </summary>
    public sealed class ExpenselyDbContext : DbContext, IDbContext, IUnitOfWork
    {
        private readonly IDateTime _dateTime;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpenselyDbContext"/> class.
        /// </summary>
        /// <param name="options">The database context options.</param>
        /// <param name="dateTime">The current date and time.</param>
        public ExpenselyDbContext(DbContextOptions options, IDateTime dateTime)
            : base(options) =>
            _dateTime = dateTime;

        /// <inheritdoc />
        public new DbSet<TEntity> Set<TEntity>()
            where TEntity : Entity =>
            base.Set<TEntity>();

        /// <inheritdoc />
        public async Task<Maybe<TEntity>> GetBydIdAsync<TEntity>(Guid id)
            where TEntity : Entity
        {
            if (id == Guid.Empty)
            {
                return Maybe<TEntity>.None;
            }

            return await Set<TEntity>().FirstOrDefaultAsync(e => e.Id == id);
        }

        /// <inheritdoc />
        public async Task<bool> AnyAsync<TEntity>(Expression<Func<TEntity, bool>> predicate)
            where TEntity : Entity =>
            await Set<TEntity>().AnyAsync(predicate);

        /// <inheritdoc />
        public void Insert<TEntity>(TEntity entity)
            where TEntity : Entity =>
            Set<TEntity>().Add(entity);

        /// <inheritdoc />
        public new void Remove<TEntity>(TEntity entity)
            where TEntity : Entity =>
            Set<TEntity>().Remove(entity);

        /// <summary>
        /// Saves all of the pending changes in the unit of work.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The number of entities that have been saved.</returns>
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            DateTime utcNow = _dateTime.UtcNow;

            UpdateAuditableEntities(utcNow);

            return base.SaveChangesAsync(cancellationToken);
        }

        /// <inheritdoc />
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            modelBuilder.ApplyUtcDateTimeConverter();

            base.OnModelCreating(modelBuilder);
        }

        /// <summary>
        /// Updates the entities implementing <see cref="IAuditableEntity"/> interface.
        /// </summary>
        /// <param name="utcNow">The current date and time in UTC format.</param>
        private void UpdateAuditableEntities(DateTime utcNow)
        {
            foreach (EntityEntry<IAuditableEntity> entityEntry in ChangeTracker.Entries<IAuditableEntity>())
            {
                if (entityEntry.State == EntityState.Added)
                {
                    entityEntry.Property(nameof(IAuditableEntity.CreatedOnUtc)).CurrentValue = utcNow;
                }

                if (entityEntry.State == EntityState.Modified)
                {
                    entityEntry.Property(nameof(IAuditableEntity.ModifiedOnUtc)).CurrentValue = utcNow;
                }
            }
        }
    }
}
