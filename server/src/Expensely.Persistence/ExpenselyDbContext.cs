﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Abstractions.Data;
using Expensely.Application.Abstractions.Specifications;
using Expensely.Common.Clock;
using Expensely.Domain.Abstractions.Events;
using Expensely.Domain.Abstractions.Maybe;
using Expensely.Domain.Abstractions.Primitives;
using Expensely.Messaging.Abstractions.Entities;
using Expensely.Persistence.Extensions;
using Expensely.Persistence.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;

namespace Expensely.Persistence
{
    /// <summary>
    /// Represents the applications database context.
    /// </summary>
    public sealed class ExpenselyDbContext : DbContext, IDbContext
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
            where TEntity : class =>
            base.Set<TEntity>();

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

        /// <summary>
        /// Saves all of the pending changes in the database context.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The number of entities that have been saved.</returns>
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            StoreDomainEvents();

            UpdateAuditableEntities(_dateTime.UtcNow);

            return await base.SaveChangesAsync(cancellationToken);
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

        private void StoreDomainEvents()
        {
            var messages = ChangeTracker
                .Entries<AggregateRoot>()
                .Where(x => x.Entity.Events.Any())
                .SelectMany(x =>
                {
                    IReadOnlyCollection<IEvent> events = x.Entity.Events;

                    x.Entity.ClearEvents();

                    return events;
                })
                .Select(x => new Message
                {
                    Id = Guid.NewGuid(),
                    Name = x.GetType().Name,
                    Content = JsonConvert.SerializeObject(x, new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.All
                    })
                })
                .ToList();

            if (messages.Any())
            {
                Set<Message>().AddRange(messages);
            }
        }
    }
}