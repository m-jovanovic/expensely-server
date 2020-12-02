using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Abstractions.Data;
using Expensely.Application.Commands.Expenses.CreateExpense;
using Expensely.Application.Commands.Handlers.Abstractions;
using Expensely.Domain.Core;
using Expensely.Domain.Core.Errors;
using Expensely.Domain.Primitives.Maybe;
using Expensely.Domain.Primitives.Result;

namespace Expensely.Application.Commands.Handlers.Expenses.CreateExpense
{
    /// <summary>
    /// Represents the <see cref="CreateExpenseCommand"/> handler.
    /// </summary>
    internal sealed class CreateExpenseCommandHandler : ICommandHandler<CreateExpenseCommand, Result>
    {
        private readonly IDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateExpenseCommandHandler"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        public CreateExpenseCommandHandler(IDbContext dbContext) => _dbContext = dbContext;

        /// <inheritdoc />
        public async Task<Result> Handle(CreateExpenseCommand request, CancellationToken cancellationToken)
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

            var expense = new Expense(
                maybeUser.Value.Id,
                nameResult.Value,
                new Money(request.Amount, currency),
                request.OccurredOn,
                descriptionResult.Value);

            _dbContext.Insert(expense);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
