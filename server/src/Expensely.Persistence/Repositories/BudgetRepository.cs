using System;
using System.Threading;
using System.Threading.Tasks;
using Expensely.Common.Primitives.Maybe;
using Expensely.Domain.Modules.Budgets;
using Raven.Client.Documents.Session;

namespace Expensely.Persistence.Repositories
{
    /// <summary>
    /// Represents the budget repository.
    /// </summary>
    public sealed class BudgetRepository : IBudgetRepository
    {
        private readonly IAsyncDocumentSession _session;

        /// <summary>
        /// Initializes a new instance of the <see cref="BudgetRepository"/> class.
        /// </summary>
        /// <param name="session">The document session.</param>
        public BudgetRepository(IAsyncDocumentSession session) => _session = session;

        /// <inheritdoc />
        public async Task<Maybe<Budget>> GetByIdAsync(Ulid budgetId, CancellationToken cancellationToken = default) =>
            await _session.LoadAsync<Budget>(budgetId.ToString(), cancellationToken);

        /// <inheritdoc />
        public async Task<Maybe<Budget>> GetByIdWithUserAsync(Ulid budgetId, CancellationToken cancellationToken = default) =>
            await _session.Include(nameof(Budget.UserId)).LoadAsync<Budget>(budgetId.ToString(), cancellationToken);

        /// <inheritdoc />
        public async Task AddAsync(Budget budget, CancellationToken cancellationToken = default) =>
            await _session.StoreAsync(budget, cancellationToken);

        /// <inheritdoc />
        public void Remove(Budget budget) => _session.Delete(budget);
    }
}
