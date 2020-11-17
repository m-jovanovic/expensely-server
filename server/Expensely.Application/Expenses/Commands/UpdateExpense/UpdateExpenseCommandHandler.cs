using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Abstractions.Authentication;
using Expensely.Application.Abstractions.Data;
using Expensely.Application.Abstractions.Messaging;
using Expensely.Application.Validation;
using Expensely.Domain.Core;
using Expensely.Domain.Primitives.Maybe;
using Expensely.Domain.Primitives.Result;

namespace Expensely.Application.Expenses.Commands.UpdateExpense
{
    /// <summary>
    /// Represents the <see cref="UpdateExpenseCommand"/> handler.
    /// </summary>
    internal sealed class UpdateExpenseCommandHandler : ICommandHandler<UpdateExpenseCommand, Result>
    {
        private readonly IDbContext _dbContext;
        private readonly IUserInformationProvider _userInformationProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateExpenseCommandHandler"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="userInformationProvider">The user information provider.</param>
        public UpdateExpenseCommandHandler(IDbContext dbContext, IUserInformationProvider userInformationProvider)
        {
            _dbContext = dbContext;
            _userInformationProvider = userInformationProvider;
        }

        /// <inheritdoc />
        public async Task<Result> Handle(UpdateExpenseCommand request, CancellationToken cancellationToken)
        {
            Maybe<Expense> maybeExpense = await _dbContext.GetBydIdAsync<Expense>(request.ExpenseId);

            if (maybeExpense.HasNoValue)
            {
                return Result.Failure(ValidationErrors.Expense.NotFound);
            }

            Expense expense = maybeExpense.Value;

            if (expense.UserId != _userInformationProvider.UserId)
            {
                return Result.Failure(ValidationErrors.User.InvalidPermissions);
            }

            Result<Name> nameResult = Name.Create(request.Name);
            Result<Description> descriptionResult = Description.Create(request.Description);

            var result = Result.FirstFailureOrSuccess(nameResult, descriptionResult);

            if (result.IsFailure)
            {
                return Result.Failure(result.Error);
            }

            Maybe<Currency> maybeCurrency = Currency.FromValue(request.Currency);

            if (maybeCurrency.HasNoValue)
            {
                return Result.Failure(ValidationErrors.Currency.NotFound);
            }

            expense.ChangeName(nameResult.Value);

            expense.ChangeMoney(new Money(request.Amount, maybeCurrency.Value));

            expense.ChangeOccurredOnDate(request.OccurredOn);

            expense.ChangeDescription(descriptionResult.Value);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
