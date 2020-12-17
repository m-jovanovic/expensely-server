using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Abstractions.Data;
using Expensely.Application.Commands.Users.RemoveUserCurrency;
using Expensely.Common.Abstractions.Messaging;
using Expensely.Domain.Abstractions.Maybe;
using Expensely.Domain.Abstractions.Result;
using Expensely.Domain.Core;
using Expensely.Domain.Errors;

namespace Expensely.Application.Commands.Handlers.Users.RemoveUserCurrency
{
    /// <summary>
    /// Represents the <see cref="RemoveUserCurrencyCommand"/> handler.
    /// </summary>
    internal sealed class RemoveUserCurrencyCommandHandler : ICommandHandler<RemoveUserCurrencyCommand, Result>
    {
        private readonly IDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="RemoveUserCurrencyCommandHandler"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        public RemoveUserCurrencyCommandHandler(IDbContext dbContext) => _dbContext = dbContext;

        /// <inheritdoc />
        public async Task<Result> Handle(RemoveUserCurrencyCommand request, CancellationToken cancellationToken)
        {
            Maybe<User> maybeUser = await _dbContext.GetBydIdAsync<User>(request.UserId, cancellationToken);

            if (maybeUser.HasNoValue)
            {
                return Result.Failure(DomainErrors.User.NotFound);
            }

            Currency currency = Currency.FromValue(request.Currency).Value;

            Result result = maybeUser.Value.RemoveCurrency(currency);

            if (result.IsFailure)
            {
                return Result.Failure(result.Error);
            }

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
