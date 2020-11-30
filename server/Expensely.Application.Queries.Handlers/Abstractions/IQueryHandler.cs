using Expensely.Common.Messaging;
using MediatR;

namespace Expensely.Application.Queries.Handlers.Abstractions
{
    /// <summary>
    /// Represents the query interface.
    /// </summary>
    /// <typeparam name="TQuery">The query type.</typeparam>
    /// <typeparam name="TResponse">The query response type.</typeparam>
    public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, TResponse>
        where TQuery : IQuery<TResponse>
    {
    }
}
