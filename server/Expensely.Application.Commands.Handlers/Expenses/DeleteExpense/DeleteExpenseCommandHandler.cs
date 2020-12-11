using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Abstractions.Authentication;
using Expensely.Application.Abstractions.Data;
using Expensely.Application.Commands.Expenses.DeleteExpense;
using Expensely.Application.Commands.Handlers.Validation;
using Expensely.Common.Messaging;
using Expensely.Domain.Abstractions.Maybe;
using Expensely.Domain.Abstractions.Result;
using Expensely.Domain.Core;
using Expensely.Domain.Errors;

namespace Expensely.Application.Commands.Handlers.Expenses.DeleteExpense
{
    /// <summary>
    /// Represents the <see cref="DeleteExpenseCommand"/> handler.
    /// </summary>
    internal sealed class DeleteExpenseCommandHandler : ICommandHandler<DeleteExpenseCommand, Result>
    {
        private readonly IDbContext _dbContext;
        private readonly IUserInformationProvider _userInformationProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteExpenseCommandHandler"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="userInformationProvider">The user information provider.</param>
        public DeleteExpenseCommandHandler(IDbContext dbContext, IUserInformationProvider userInformationProvider)
        {
            _dbContext = dbContext;
            _userInformationProvider = userInformationProvider;
        }

        /// <inheritdoc />
        public async Task<Result> Handle(DeleteExpenseCommand request, CancellationToken cancellationToken)
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

            _dbContext.Remove(expense);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
