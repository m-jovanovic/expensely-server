using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Abstractions.Data;
using Expensely.Domain.Abstractions.Maybe;
using Expensely.Persistence.Core;
using Microsoft.EntityFrameworkCore;

namespace Expensely.Persistence.Reporting
{
    /// <summary>
    /// Represents the reporting database context.
    /// </summary>
    public sealed class ReportingDbContext : BaseDbContext, IReportingDbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReportingDbContext"/> class.
        /// </summary>
        /// <param name="options">The database context options.</param>
        public ReportingDbContext(DbContextOptions<ReportingDbContext> options)
            : base(options)
        {
        }

        /// <inheritdoc />
        public async Task<Maybe<TEntity>> FirstOrDefaultAsync<TEntity>(
            Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
            where TEntity : class =>
            await Set<TEntity>().FirstOrDefaultAsync(predicate, cancellationToken);

        /// <inheritdoc />
        public async Task<bool> AnyAsync<TEntity>(
            Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
            where TEntity : class =>
            await Set<TEntity>().AnyAsync(predicate, cancellationToken);

        /// <inheritdoc />
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
        }
    }
}
