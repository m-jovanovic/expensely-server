using System;
using System.Threading;
using System.Threading.Tasks;
using Expensely.Domain.Abstractions.Maybe;
using Expensely.Domain.Core;

namespace Expensely.Domain.Repositories
{
    /// <summary>
    /// Represents the expense repository interface.
    /// </summary>
    public interface IExpenseRepository
    {
        /// <summary>
        /// Gets the expense with the specified identifier, if one exists.
        /// </summary>
        /// <param name="expenseId">The expense identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The maybe instance that may contain the expense with the specified identifier.</returns>
        Task<Maybe<Expense>> GetByIdAsync(Guid expenseId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Adds the specified expense to the repository.
        /// </summary>
        /// <param name="expense">The expense to be added.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The completed task.</returns>
        Task AddAsync(Expense expense, CancellationToken cancellationToken = default);

        /// <summary>
        /// Removes the specified expense from the repository.
        /// </summary>
        /// <param name="expense">The expense to be removed.</param>
        void Remove(Expense expense);
    }
}
