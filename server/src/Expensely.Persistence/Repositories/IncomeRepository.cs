using System.Threading;
using System.Threading.Tasks;
using Expensely.Domain.Abstractions.Maybe;
using Expensely.Domain.Core;
using Expensely.Domain.Repositories;
using Raven.Client.Documents.Session;

namespace Expensely.Persistence.Repositories
{
    /// <summary>
    /// Represents the income repository.
    /// </summary>
    internal sealed class IncomeRepository : IIncomeRepository
    {
        private readonly IAsyncDocumentSession _session;

        /// <summary>
        /// Initializes a new instance of the <see cref="IncomeRepository"/> class.
        /// </summary>
        /// <param name="session">The document session.</param>
        public IncomeRepository(IAsyncDocumentSession session) => _session = session;

        /// <inheritdoc />
        public async Task<Maybe<Income>> GetByIdAsync(string incomeId, CancellationToken cancellationToken = default) =>
            await _session.LoadAsync<Income>(incomeId, cancellationToken);

        /// <inheritdoc />
        public async Task AddAsync(Income income, CancellationToken cancellationToken = default) =>
            await _session.StoreAsync(income, cancellationToken);

        /// <inheritdoc />
        public void Remove(Income income) => _session.Delete(income);
    }
}
