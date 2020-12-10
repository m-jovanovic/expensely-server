using System.Threading;
using System.Threading.Tasks;
using Expensely.Domain.Abstractions.Events;
using Expensely.Domain.Events.Users;

namespace Expensely.Application.Events.Handlers.Users
{
    /// <summary>
    /// Represents the <see cref="UserCurrencyAddedEvent"/> handler.
    /// </summary>
    public sealed class UserCurrencyAddedEventHandler : IEventHandler<UserCurrencyAddedEvent>
    {
        /// <inheritdoc />
        public Task Handle(UserCurrencyAddedEvent @event, CancellationToken cancellationToken = default) => Task.CompletedTask;
    }
}
