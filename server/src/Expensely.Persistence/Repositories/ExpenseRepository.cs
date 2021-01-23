using System.Threading;
using System.Threading.Tasks;
using Expensely.Domain.Abstractions.Maybe;
using Expensely.Domain.Core;
using Expensely.Domain.Repositories;
using Raven.Client.Documents.Session;

namespace Expensely.Persistence.Repositories
{
    /// <summary>
    /// Represents the expense repository.
    /// </summary>
    internal sealed class ExpenseRepository : IExpenseRepository
    {
        private readonly IAsyncDocumentSession _session;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpenseRepository"/> class.
        /// </summary>
        /// <param name="session">The document session.</param>
        public ExpenseRepository(IAsyncDocumentSession session) => _session = session;

        /// <inheritdoc />
        public async Task<Maybe<Expense>> GetByIdAsync(string expenseId, CancellationToken cancellationToken = default) =>
            await _session.LoadAsync<Expense>(expenseId, cancellationToken);

        /// <inheritdoc />
        public async Task AddAsync(Expense expense, CancellationToken cancellationToken = default) =>
            await _session.StoreAsync(expense, cancellationToken);

        /// <inheritdoc />
        public void Remove(Expense expense) => _session.Delete(expense);
    }
}
