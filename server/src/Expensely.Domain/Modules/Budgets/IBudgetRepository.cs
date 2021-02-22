using System.Threading;
using System.Threading.Tasks;
using Expensely.Shared.Primitives.Maybe;

namespace Expensely.Domain.Modules.Budgets
{
    /// <summary>
    /// Represents the budget repository interface.
    /// </summary>
    public interface IBudgetRepository
    {
        /// <summary>
        /// Gets the budget with the specified identifier, if one exists.
        /// </summary>
        /// <param name="budgetId">The budget identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The maybe instance that may contain the budget with the specified identifier.</returns>
        Task<Maybe<Budget>> GetByIdAsync(string budgetId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Adds the specified budget to the repository.
        /// </summary>
        /// <param name="budget">The budget to be added.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The completed task.</returns>
        Task AddAsync(Budget budget, CancellationToken cancellationToken = default);

        /// <summary>
        /// Removes the specified budget from the repository.
        /// </summary>
        /// <param name="budget">The budget to be removed.</param>
        void Remove(Budget budget);
    }
}
