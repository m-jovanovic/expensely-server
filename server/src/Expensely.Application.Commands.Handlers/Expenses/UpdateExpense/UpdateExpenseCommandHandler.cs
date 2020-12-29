using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Abstractions.Authentication;
using Expensely.Application.Abstractions.Data;
using Expensely.Application.Commands.Expenses.UpdateExpense;
using Expensely.Application.Commands.Handlers.Validation;
using Expensely.Common.Abstractions.Messaging;
using Expensely.Domain.Abstractions.Maybe;
using Expensely.Domain.Abstractions.Result;
using Expensely.Domain.Contracts;
using Expensely.Domain.Core;
using Expensely.Domain.Errors;
using Expensely.Domain.Services;

namespace Expensely.Application.Commands.Handlers.Expenses.UpdateExpense
{
    /// <summary>
    /// Represents the <see cref="UpdateExpenseCommand"/> handler.
    /// </summary>
    internal sealed class UpdateExpenseCommandHandler : ICommandHandler<UpdateExpenseCommand, Result>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IUserInformationProvider _userInformationProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateExpenseCommandHandler"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="userInformationProvider">The user information provider.</param>
        public UpdateExpenseCommandHandler(IApplicationDbContext dbContext, IUserInformationProvider userInformationProvider)
        {
            _dbContext = dbContext;
            _userInformationProvider = userInformationProvider;
        }

        /// <inheritdoc />
        public async Task<Result> Handle(UpdateExpenseCommand request, CancellationToken cancellationToken)
        {
            Maybe<Expense> maybeExpense = await _dbContext.GetBydIdAsync<Expense>(request.ExpenseId, cancellationToken);

            if (maybeExpense.HasNoValue)
            {
                return Result.Failure(DomainErrors.Expense.NotFound);
            }

            Expense expense = maybeExpense.Value;

            if (expense.UserId != _userInformationProvider.UserId)
            {
                return Result.Failure(ValidationErrors.User.InvalidPermissions);
            }

            Maybe<User> maybeUser = await _dbContext.GetBydIdAsync<User>(expense.UserId, cancellationToken);

            if (maybeUser.HasNoValue)
            {
                return Result.Failure(DomainErrors.User.NotFound);
            }

            Result<TransactionInformation> transactionInformationResult = new TransactionInformationService().Validate(
                maybeUser.Value,
                request.Name,
                request.Description,
                request.Category,
                request.Currency);

            if (transactionInformationResult.IsFailure)
            {
                return Result.Failure(transactionInformationResult.Error);
            }

            expense.Update(
                transactionInformationResult.Value.Name,
                transactionInformationResult.Value.Category,
                new Money(request.Amount, transactionInformationResult.Value.Currency),
                request.OccurredOn,
                transactionInformationResult.Value.Description);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
