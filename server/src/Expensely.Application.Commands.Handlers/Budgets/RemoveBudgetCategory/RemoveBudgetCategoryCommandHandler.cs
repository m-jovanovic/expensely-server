using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Abstractions.Authentication;
using Expensely.Application.Abstractions.Data;
using Expensely.Application.Commands.Budgets;
using Expensely.Application.Commands.Handlers.Validation;
using Expensely.Common.Abstractions.Messaging;
using Expensely.Common.Primitives.Maybe;
using Expensely.Common.Primitives.Result;
using Expensely.Domain.Modules.Budgets;
using Expensely.Domain.Modules.Common;

namespace Expensely.Application.Commands.Handlers.Budgets.RemoveBudgetCategory
{
    /// <summary>
    /// Represents the <see cref="RemoveBudgetCategoryCommand"/> handler.
    /// </summary>
    public sealed class RemoveBudgetCategoryCommandHandler : ICommandHandler<RemoveBudgetCategoryCommand, Result>
    {
        private readonly IBudgetRepository _budgetRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserInformationProvider _userInformationProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="RemoveBudgetCategoryCommandHandler"/> class.
        /// </summary>
        /// <param name="budgetRepository">The budget repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="userInformationProvider">The user information provider.</param>
        public RemoveBudgetCategoryCommandHandler(
            IBudgetRepository budgetRepository,
            IUnitOfWork unitOfWork,
            IUserInformationProvider userInformationProvider)
        {
            _budgetRepository = budgetRepository;
            _unitOfWork = unitOfWork;
            _userInformationProvider = userInformationProvider;
        }

        /// <inheritdoc />
        public async Task<Result> Handle(RemoveBudgetCategoryCommand request, CancellationToken cancellationToken)
        {
            Maybe<Budget> maybeBudget = await _budgetRepository.GetByIdAsync(request.BudgetId, cancellationToken);

            if (maybeBudget.HasNoValue)
            {
                return Result.Failure(ValidationErrors.Budget.NotFound);
            }

            Budget budget = maybeBudget.Value;

            if (budget.UserId != _userInformationProvider.UserId)
            {
                return Result.Failure(ValidationErrors.User.InvalidPermissions);
            }

            Category category = Category.FromValue(request.Category).Value;

            Result removeCategoryResult = budget.RemoveCategory(category);

            if (removeCategoryResult.IsFailure)
            {
                return Result.Failure(removeCategoryResult.Error);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
