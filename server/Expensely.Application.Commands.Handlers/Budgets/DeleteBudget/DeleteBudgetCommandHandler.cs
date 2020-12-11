using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Abstractions.Authentication;
using Expensely.Application.Abstractions.Data;
using Expensely.Application.Commands.Budgets.DeleteBudget;
using Expensely.Application.Commands.Handlers.Validation;
using Expensely.Common.Messaging;
using Expensely.Domain.Abstractions.Maybe;
using Expensely.Domain.Abstractions.Result;
using Expensely.Domain.Core;
using Expensely.Domain.Errors;

namespace Expensely.Application.Commands.Handlers.Budgets.DeleteBudget
{
    /// <summary>
    /// Represents the <see cref="DeleteBudgetCommand"/> handler.
    /// </summary>
    internal sealed class DeleteBudgetCommandHandler : ICommandHandler<DeleteBudgetCommand, Result>
    {
        private readonly IDbContext _dbContext;
        private readonly IUserInformationProvider _userInformationProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteBudgetCommandHandler"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="userInformationProvider">The user information provider.</param>
        public DeleteBudgetCommandHandler(IDbContext dbContext, IUserInformationProvider userInformationProvider)
        {
            _dbContext = dbContext;
            _userInformationProvider = userInformationProvider;
        }

        /// <inheritdoc />
        public async Task<Result> Handle(DeleteBudgetCommand request, CancellationToken cancellationToken)
        {
            Maybe<Budget> maybeBudget = await _dbContext.GetBydIdAsync<Budget>(request.BudgetId, cancellationToken);

            if (maybeBudget.HasNoValue)
            {
                return Result.Failure(DomainErrors.Budget.NotFound);
            }

            Budget budget = maybeBudget.Value;

            if (budget.UserId != _userInformationProvider.UserId)
            {
                return Result.Failure(ValidationErrors.User.InvalidPermissions);
            }

            _dbContext.Remove(budget);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
