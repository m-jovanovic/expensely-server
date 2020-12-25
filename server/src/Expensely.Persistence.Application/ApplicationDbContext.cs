using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Abstractions.Data;
using Expensely.Application.Abstractions.Messaging;
using Expensely.Common.Abstractions.Clock;
using Expensely.Domain.Abstractions.Events;
using Expensely.Domain.Abstractions.Maybe;
using Expensely.Domain.Abstractions.Primitives;
using Expensely.Persistence.Application.Extensions;
using Expensely.Persistence.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;

namespace Expensely.Persistence.Application
{
    /// <summary>
    /// Represents the applications database context.
    /// </summary>
    public sealed class ApplicationDbContext : BaseDbContext, IApplicationDbContext
    {
        private readonly IEventPublisher _eventPublisher;
        private readonly IDateTime _dateTime;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationDbContext"/> class.
        /// </summary>
        /// <param name="options">The database context options.</param>
        /// <param name="eventPublisher">The event publisher.</param>
        /// <param name="dateTime">The current date and time.</param>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IEventPublisher eventPublisher, IDateTime dateTime)
            : base(options)
        {
            _eventPublisher = eventPublisher;
            _dateTime = dateTime;
        }

        /// <inheritdoc />
        public async Task<Maybe<TEntity>> GetBydIdAsync<TEntity>(Guid id, CancellationToken cancellationToken = default)
            where TEntity : Entity
        {
            if (id == Guid.Empty)
            {
                return Maybe<TEntity>.None;
            }

            return await Set<TEntity>().FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        }

        /// <summary>
        /// Saves all of the pending changes in the database context.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The number of entities that have been saved.</returns>
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            IReadOnlyCollection<IEvent> events = GetRaisedEventsForPublishing();

            if (!events.Any())
            {
                return await SaveChangesInternalAsync(cancellationToken);
            }

            return await SaveChangesAndPublishEventsAsync(events, cancellationToken);
        }

        /// <inheritdoc />
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            modelBuilder.ApplyUtcDateTimeConverter();

            base.OnModelCreating(modelBuilder);
        }

        /// <summary>
        /// Saves the pending changes and updates the auditable entities.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The number of entities that have been saved.</returns>
        private Task<int> SaveChangesInternalAsync(CancellationToken cancellationToken)
        {
            UpdateAuditableEntities(_dateTime.UtcNow);

            return base.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Saves the pending changes and publishes the specified events within a transaction.
        /// </summary>
        /// <param name="events">The events to be published.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The number of entities that have been saved.</returns>
        private async Task<int> SaveChangesAndPublishEventsAsync(IReadOnlyCollection<IEvent> events, CancellationToken cancellationToken)
        {
            await using IDbContextTransaction transaction = await Database
                .BeginTransactionAsync(IsolationLevel.ReadCommitted, cancellationToken);

            int result = await SaveChangesInternalAsync(cancellationToken);

            await _eventPublisher.PublishAsync(events, transaction.GetDbTransaction());

            await transaction.CommitAsync(cancellationToken);

            return result;
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

        /// <summary>
        /// Gets the events that have been raised for publishing.
        /// </summary>
        /// <returns>The enumerable collection of events.</returns>
        private IReadOnlyCollection<IEvent> GetRaisedEventsForPublishing() =>
            ChangeTracker
                .Entries<AggregateRoot>()
                .Where(x => x.Entity.Events.Any())
                .SelectMany(x =>
                {
                    IReadOnlyCollection<IEvent> entityEvents = x.Entity.Events;

                    x.Entity.ClearEvents();

                    return entityEvents;
                }).ToArray();
    }
}
