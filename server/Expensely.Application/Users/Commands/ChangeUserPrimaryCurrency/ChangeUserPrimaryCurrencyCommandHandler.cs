using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Abstractions.Authentication;
using Expensely.Application.Abstractions.Data;
using Expensely.Application.Abstractions.Messaging;
using Expensely.Application.Validation;
using Expensely.Domain.Core;
using Expensely.Domain.Primitives.Maybe;
using Expensely.Domain.Primitives.Result;

namespace Expensely.Application.Users.Commands.ChangeUserPrimaryCurrency
{
    /// <summary>
    /// Represents the <see cref="ChangeUserPrimaryCurrencyCommand"/> handler.
    /// </summary>
    internal sealed class ChangeUserPrimaryCurrencyCommandHandler : ICommandHandler<ChangeUserPrimaryCurrencyCommand, Result>
    {
        private readonly IDbContext _dbContext;
        private readonly IUserInformationProvider _userInformationProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChangeUserPrimaryCurrencyCommandHandler"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="userInformationProvider">The user information provider.</param>
        public ChangeUserPrimaryCurrencyCommandHandler(
            IDbContext dbContext,
            IUserInformationProvider userInformationProvider)
        {
            _dbContext = dbContext;
            _userInformationProvider = userInformationProvider;
        }

        /// <inheritdoc />
        public async Task<Result> Handle(ChangeUserPrimaryCurrencyCommand request, CancellationToken cancellationToken)
        {
            if (request.UserId != _userInformationProvider.UserId)
            {
                return Result.Failure(ValidationErrors.User.InvalidPermissions);
            }

            Maybe<User> maybeUser = await _dbContext.GetBydIdAsync<User>(request.UserId);

            if (maybeUser.HasNoValue)
            {
                return Result.Failure(ValidationErrors.User.NotFound);
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
