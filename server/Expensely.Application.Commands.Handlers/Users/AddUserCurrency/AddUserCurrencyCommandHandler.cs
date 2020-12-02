using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Abstractions.Data;
using Expensely.Application.Commands.Handlers.Abstractions;
using Expensely.Application.Commands.Users.AddUserCurrency;
using Expensely.Domain.Core;
using Expensely.Domain.Core.Errors;
using Expensely.Domain.Primitives.Maybe;
using Expensely.Domain.Primitives.Result;

namespace Expensely.Application.Commands.Handlers.Users.AddUserCurrency
{
    /// <summary>
    /// Represents the <see cref="AddUserCurrencyCommand"/> handler.
    /// </summary>
    internal sealed class AddUserCurrencyCommandHandler : ICommandHandler<AddUserCurrencyCommand, Result>
    {
        private readonly IDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="AddUserCurrencyCommandHandler"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        public AddUserCurrencyCommandHandler(IDbContext dbContext) => _dbContext = dbContext;

        /// <inheritdoc />
        public async Task<Result> Handle(AddUserCurrencyCommand request, CancellationToken cancellationToken)
        {
            Maybe<User> maybeUser = await _dbContext.GetBydIdAsync<User>(request.UserId);

            if (maybeUser.HasNoValue)
            {
                return Result.Failure(DomainErrors.User.NotFound);
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
