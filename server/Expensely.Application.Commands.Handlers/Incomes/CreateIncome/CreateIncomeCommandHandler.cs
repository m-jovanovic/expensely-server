using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Abstractions.Data;
using Expensely.Application.Commands.Incomes.CreateIncome;
using Expensely.Common.Messaging;
using Expensely.Domain.Abstractions.Maybe;
using Expensely.Domain.Abstractions.Result;
using Expensely.Domain.Core;
using Expensely.Domain.Core.Errors;

namespace Expensely.Application.Commands.Handlers.Incomes.CreateIncome
{
    /// <summary>
    /// Represents the <see cref="CreateIncomeCommand"/> handler.
    /// </summary>
    internal sealed class CreateIncomeCommandHandler : ICommandHandler<CreateIncomeCommand, Result>
    {
        private readonly IDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateIncomeCommandHandler"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        public CreateIncomeCommandHandler(IDbContext dbContext) => _dbContext = dbContext;

        /// <inheritdoc />
        public async Task<Result> Handle(CreateIncomeCommand request, CancellationToken cancellationToken)
        {
            Result<Name> nameResult = Name.Create(request.Name);
            Result<Description> descriptionResult = Description.Create(request.Description);

            var firstFailureOrSuccess = Result.FirstFailureOrSuccess(nameResult, descriptionResult);

            if (firstFailureOrSuccess.IsFailure)
            {
                return Result.Failure(firstFailureOrSuccess.Error);
            }

            Maybe<User> maybeUser = await _dbContext.GetBydIdAsync<User>(request.UserId);

            if (maybeUser.HasNoValue)
            {
                return Result.Failure(DomainErrors.User.NotFound);
            }

            Currency currency = Currency.FromValue(request.Currency).Value;

            if (!maybeUser.Value.HasCurrency(currency))
            {
                return Result.Failure(DomainErrors.User.CurrencyDoesNotExist);
            }

            var income = new Income(
                maybeUser.Value.Id,
                nameResult.Value,
                new Money(request.Amount, currency),
                request.OccurredOn,
                descriptionResult.Value);

            _dbContext.Insert(income);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
