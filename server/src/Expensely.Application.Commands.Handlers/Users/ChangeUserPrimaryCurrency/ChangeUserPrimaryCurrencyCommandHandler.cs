using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Abstractions.Data;
using Expensely.Application.Commands.Users.ChangeUserPrimaryCurrency;
using Expensely.Common.Abstractions.Messaging;
using Expensely.Domain.Abstractions.Maybe;
using Expensely.Domain.Abstractions.Result;
using Expensely.Domain.Core;
using Expensely.Domain.Errors;

namespace Expensely.Application.Commands.Handlers.Users.ChangeUserPrimaryCurrency
{
    /// <summary>
    /// Represents the <see cref="ChangeUserPrimaryCurrencyCommand"/> handler.
    /// </summary>
    internal sealed class ChangeUserPrimaryCurrencyCommandHandler : ICommandHandler<ChangeUserPrimaryCurrencyCommand, Result>
    {
        private readonly IApplicationDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChangeUserPrimaryCurrencyCommandHandler"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        public ChangeUserPrimaryCurrencyCommandHandler(IApplicationDbContext dbContext) => _dbContext = dbContext;

        /// <inheritdoc />
        public async Task<Result> Handle(ChangeUserPrimaryCurrencyCommand request, CancellationToken cancellationToken)
        {
            Maybe<User> maybeUser = await _dbContext.GetBydIdAsync<User>(request.UserId, cancellationToken);

            if (maybeUser.HasNoValue)
            {
                return Result.Failure(DomainErrors.User.NotFound);
            }

            Currency currency = Currency.FromValue(request.Currency).Value;

            Result result = maybeUser.Value.ChangePrimaryCurrency(currency);

            if (result.IsFailure)
            {
                return result;
            }

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
