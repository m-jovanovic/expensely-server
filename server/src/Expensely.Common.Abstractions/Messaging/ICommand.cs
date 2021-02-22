using Expensely.Common.Primitives.Result;
using MediatR;

namespace Expensely.Common.Abstractions.Messaging
{
    /// <summary>
    /// Represents the command interface.
    /// </summary>
    /// <typeparam name="TResponse">The command response type.</typeparam>
    public interface ICommand<out TResponse> : IRequest<TResponse>
        where TResponse : Result
    {
    }
}
