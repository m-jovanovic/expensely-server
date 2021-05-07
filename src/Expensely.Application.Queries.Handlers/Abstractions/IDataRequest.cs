using System.Threading;
using System.Threading.Tasks;

namespace Expensely.Application.Queries.Handlers.Abstractions
{
    /// <summary>
    /// Represents the data request interface.
    /// </summary>
    /// <typeparam name="TRequest">The request type.</typeparam>
    /// <typeparam name="TResponse">The response type.</typeparam>
    public interface IDataRequest<in TRequest, TResponse>
    {
        /// <summary>
        /// Gets the response the satisfies the specified data request.
        /// </summary>
        /// <param name="request">The data request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The responses that satisfies the specified data request.</returns>
        Task<TResponse> GetAsync(TRequest request, CancellationToken cancellationToken = default);
    }
}
