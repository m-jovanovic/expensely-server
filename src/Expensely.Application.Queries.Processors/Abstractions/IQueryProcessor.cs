using System.Threading;
using System.Threading.Tasks;
using Expensely.Common.Abstractions.Messaging;

namespace Expensely.Application.Queries.Processors.Abstractions
{
    /// <summary>
    /// Represents the query processor interface.
    /// </summary>
    /// <typeparam name="TQuery">The query type.</typeparam>
    /// <typeparam name="TResponse">The response type.</typeparam>
    public interface IQueryProcessor<in TQuery, TResponse>
        where TQuery : IQuery<TResponse>
    {
        /// <summary>
        /// Processes the specified query and returns the result.
        /// </summary>
        /// <param name="query">The query to be executed.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The result of the processing of the provided query.</returns>
        Task<TResponse> Process(TQuery query, CancellationToken cancellationToken = default);
    }
}
