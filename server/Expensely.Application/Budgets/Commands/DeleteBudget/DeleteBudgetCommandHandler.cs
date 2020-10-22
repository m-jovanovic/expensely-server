using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Abstractions.Authentication;
using Expensely.Application.Abstractions.Data;
using Expensely.Application.Abstractions.Messaging;
using Expensely.Application.Validation;
using Expensely.Domain.Core;
using Expensely.Domain.Primitives.Maybe;
using Expensely.Domain.Primitives.Result;

namespace Expensely.Application.Budgets.Commands.DeleteBudget
{
    /// <summary>
    /// Represents the <see cref="DeleteBudgetCommand"/> handler.
    /// </summary>
    internal sealed class DeleteBudgetCommandHandler : ICommandHandler<DeleteBudgetCommand, Result>
    {
        private readonly IDbContext _dbContext;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserIdentifierProvider _userIdentifierProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteBudgetCommandHandler"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="userIdentifierProvider">The user identifier provider.</param>
        public DeleteBudgetCommandHandler(IDbContext dbContext, IUnitOfWork unitOfWork, IUserIdentifierProvider userIdentifierProvider)
        {
            _dbContext = dbContext;
            _unitOfWork = unitOfWork;
            _userIdentifierProvider = userIdentifierProvider;
        }

        /// <inheritdoc />
        public async Task<Result> Handle(DeleteBudgetCommand request, CancellationToken cancellationToken)
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

            _dbContext.Remove(budget);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
