using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Abstractions.Authentication;
using Expensely.Application.Abstractions.Data;
using Expensely.Application.Abstractions.Messaging;
using Expensely.Application.Expenses.Commands.UpdateExpense;
using Expensely.Application.Validation;
using Expensely.Domain.Core;
using Expensely.Domain.Primitives.Maybe;
using Expensely.Domain.Primitives.Result;

namespace Expensely.Application.Budgets.Commands.UpdateBudget
{
    /// <summary>
    /// Represents the <see cref="UpdateBudgetCommand"/> handler.
    /// </summary>
    internal sealed class UpdateBudgetCommandHandler : ICommandHandler<UpdateBudgetCommand, Result>
    {
        private readonly IDbContext _dbContext;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserIdentifierProvider _userIdentifierProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateBudgetCommandHandler"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="userIdentifierProvider">The user identifier provider.</param>
        public UpdateBudgetCommandHandler(IDbContext dbContext, IUnitOfWork unitOfWork, IUserIdentifierProvider userIdentifierProvider)
        {
            _dbContext = dbContext;
            _unitOfWork = unitOfWork;
            _userIdentifierProvider = userIdentifierProvider;
        }

        /// <inheritdoc />
        public async Task<Result> Handle(UpdateBudgetCommand request, CancellationToken cancellationToken)
        {
            Maybe<Budget> maybeBudget = await _dbContext.GetBydIdAsync<Budget>(request.BudgetId);

            if (maybeBudget.HasNoValue)
            {
                return Result.Failure(Errors.Budget.NotFound);
            }

            Budget budget = maybeBudget.Value;

            if (budget.UserId != _userIdentifierProvider.UserId)
            {
                return Result.Failure(Errors.User.InvalidPermissions);
            }

            Result<Name> nameResult = Name.Create(request.Name);

            if (nameResult.IsFailure)
            {
                return Result.Failure(nameResult.Error);
            }

            Maybe<Currency> maybeCurrency = Currency.FromValue(request.Currency);

            if (maybeCurrency.HasNoValue)
            {
                return Result.Failure(Errors.Currency.NotFound);
            }

            budget.ChangeName(nameResult.Value);

            budget.ChangeMoney(new Money(request.Amount, maybeCurrency.Value));

            budget.ChangeDates(request.StartDate, request.EndDate);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
