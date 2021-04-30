using System.Threading;
using System.Threading.Tasks;

namespace Expensely.Application.Abstractions.Data
{
    /// <summary>
    /// Represents the unit of work interface.
    /// </summary>
    public interface IUnitOfWork
    {
        /// <summary>
        /// Saves all of the pending changes in the unit of work.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The completed task.</returns>
        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
