using System;
using System.Threading;
using System.Threading.Tasks;
using Expensely.Domain.Abstractions.Maybe;
using Expensely.Domain.Core;

namespace Expensely.Domain.Repositories
{
    /// <summary>
    /// Represents the income repository interface.
    /// </summary>
    public interface IIncomeRepository
    {
        /// <summary>
        /// Gets the income with the specified identifier, if one exists.
        /// </summary>
        /// <param name="incomeId">The income identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The maybe instance that may contain the income with the specified identifier.</returns>
        Task<Maybe<Income>> GetByIdAsync(Guid incomeId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Adds the specified income to the repository.
        /// </summary>
        /// <param name="income">The income to be added.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The completed task.</returns>
        Task AddAsync(Income income, CancellationToken cancellationToken = default);

        /// <summary>
        /// Removes the specified income from the repository.
        /// </summary>
        /// <param name="income">The income to be removed.</param>
        void Remove(Income income);
    }
}
