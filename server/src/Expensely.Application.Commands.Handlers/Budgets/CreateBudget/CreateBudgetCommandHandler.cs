using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Abstractions.Data;
using Expensely.Application.Commands.Budgets;
using Expensely.Application.Commands.Handlers.Validation;
using Expensely.Application.Contracts.Common;
using Expensely.Common.Abstractions.Messaging;
using Expensely.Common.Primitives.Maybe;
using Expensely.Common.Primitives.Result;
using Expensely.Domain.Modules.Budgets;
using Expensely.Domain.Modules.Common;
using Expensely.Domain.Modules.Users;

namespace Expensely.Application.Commands.Handlers.Budgets.CreateBudget
{
    /// <summary>
    /// Represents the <see cref="CreateBudgetCommand"/> handler.
    /// </summary>
    public sealed class CreateBudgetCommandHandler : ICommandHandler<CreateBudgetCommand, Result<EntityCreatedResponse>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IBudgetRepository _budgetRepository;
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateBudgetCommandHandler"/> class.
        /// </summary>
        /// <param name="userRepository">The user repository.</param>
        /// <param name="budgetRepository">The budget repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        public CreateBudgetCommandHandler(IUserRepository userRepository, IBudgetRepository budgetRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _budgetRepository = budgetRepository;
            _unitOfWork = unitOfWork;
        }

        /// <inheritdoc />
        public async Task<Result<EntityCreatedResponse>> Handle(CreateBudgetCommand request, CancellationToken cancellationToken)
        {
            Result<Name> nameResult = Name.Create(request.Name);

            if (nameResult.IsFailure)
            {
                return Result.Failure<EntityCreatedResponse>(nameResult.Error);
            }

            Maybe<User> maybeUser = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);

            if (maybeUser.HasNoValue)
            {
                return Result.Failure<EntityCreatedResponse>(ValidationErrors.User.NotFound);
            }

            Category[] categories = request.Categories
                .Select(category => Category.FromValue(category).Value)
                .Where(category => category.IsExpense)
                .ToArray();

            var budget = new Budget(
                maybeUser.Value,
                nameResult.Value,
                new Money(request.Amount, Currency.FromValue(request.Currency).Value),
                categories,
                request.StartDate,
                request.EndDate);

            // TODO: Add domain rule about the allowed # of budgets.
            await _budgetRepository.AddAsync(budget, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new EntityCreatedResponse(budget.Id);
        }
    }
}
