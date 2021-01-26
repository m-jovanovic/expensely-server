using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Abstractions.Authentication;
using Expensely.Application.Commands.Budgets;
using Expensely.Application.Commands.Handlers.Validation;
using Expensely.Common.Abstractions.Messaging;
using Expensely.Domain.Abstractions.Maybe;
using Expensely.Domain.Abstractions.Result;
using Expensely.Domain.Core;
using Expensely.Domain.Errors;
using Expensely.Domain.Repositories;

namespace Expensely.Application.Commands.Handlers.Budgets.UpdateBudget
{
    /// <summary>
    /// Represents the <see cref="UpdateBudgetCommand"/> handler.
    /// </summary>
    internal sealed class UpdateBudgetCommandHandler : ICommandHandler<UpdateBudgetCommand, Result>
    {
        private readonly IBudgetRepository _budgetRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserInformationProvider _userInformationProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateBudgetCommandHandler"/> class.
        /// </summary>
        /// <param name="budgetRepository">The database context.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="userInformationProvider">The user information provider.</param>
        public UpdateBudgetCommandHandler(
            IBudgetRepository budgetRepository,
            IUserInformationProvider userInformationProvider,
            IUnitOfWork unitOfWork)
        {
            _budgetRepository = budgetRepository;
            _unitOfWork = unitOfWork;
            _userInformationProvider = userInformationProvider;
        }

        /// <inheritdoc />
        public async Task<Result> Handle(UpdateBudgetCommand request, CancellationToken cancellationToken)
        {
            Maybe<Budget> maybeBudget = await _budgetRepository.GetByIdAsync(request.BudgetId, cancellationToken);

            if (maybeBudget.HasNoValue)
            {
                return Result.Failure(DomainErrors.Budget.NotFound);
            }

            Budget budget = maybeBudget.Value;

            if (budget.UserId != _userInformationProvider.UserId)
            {
                return Result.Failure(ValidationErrors.User.InvalidPermissions);
            }

            Result<Name> nameResult = Name.Create(request.Name);

            if (nameResult.IsFailure)
            {
                return Result.Failure(nameResult.Error);
            }

            budget.ChangeName(nameResult.Value);

            budget.ChangeMoney(new Money(request.Amount, Currency.FromValue(request.Currency).Value));

            budget.ChangeDates(request.StartDate, request.EndDate);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
