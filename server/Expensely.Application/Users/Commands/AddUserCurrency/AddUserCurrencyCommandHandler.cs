using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Abstractions.Authentication;
using Expensely.Application.Abstractions.Data;
using Expensely.Application.Abstractions.Messaging;
using Expensely.Application.Validation;
using Expensely.Domain.Core;
using Expensely.Domain.Primitives.Maybe;
using Expensely.Domain.Primitives.Result;

namespace Expensely.Application.Users.Commands.AddUserCurrency
{
    /// <summary>
    /// Represents the <see cref="AddUserCurrencyCommand"/> handler.
    /// </summary>
    internal sealed class AddUserCurrencyCommandHandler : ICommandHandler<AddUserCurrencyCommand, Result>
    {
        private readonly IDbContext _dbContext;
        private readonly IUserInformationProvider _userInformationProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="AddUserCurrencyCommandHandler"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="userInformationProvider">The user information provider.</param>
        public AddUserCurrencyCommandHandler(
            IDbContext dbContext,
            IUserInformationProvider userInformationProvider)
        {
            _dbContext = dbContext;
            _userInformationProvider = userInformationProvider;
        }

        /// <inheritdoc />
        public async Task<Result> Handle(AddUserCurrencyCommand request, CancellationToken cancellationToken)
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

            Result result = maybeUser.Value.AddCurrency(currency);

            if (result.IsFailure)
            {
                return Result.Failure(result.Error);
            }

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
