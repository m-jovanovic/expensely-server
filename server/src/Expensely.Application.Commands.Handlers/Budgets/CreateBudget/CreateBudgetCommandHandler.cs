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
using Expensely.Domain.Modules.Budgets.Contracts;
using Expensely.Domain.Modules.Users;

namespace Expensely.Application.Commands.Handlers.Budgets.CreateBudget
{
    /// <summary>
    /// Represents the <see cref="CreateBudgetCommand"/> handler.
    /// </summary>
    public sealed class CreateBudgetCommandHandler : ICommandHandler<CreateBudgetCommand, Result<EntityCreatedResponse>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IBudgetFactory _budgetFactory;
        private readonly IBudgetRepository _budgetRepository;
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateBudgetCommandHandler"/> class.
        /// </summary>
        /// <param name="userRepository">The user repository.</param>
        /// <param name="budgetFactory">The budget factory.</param>
        /// <param name="budgetRepository">The budget repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        public CreateBudgetCommandHandler(
            IUserRepository userRepository,
            IBudgetFactory budgetFactory,
            IBudgetRepository budgetRepository,
            IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _budgetFactory = budgetFactory;
            _budgetRepository = budgetRepository;
            _unitOfWork = unitOfWork;
        }

        /// <inheritdoc />
        public async Task<Result<EntityCreatedResponse>> Handle(CreateBudgetCommand request, CancellationToken cancellationToken)
        {
            Maybe<User> maybeUser = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);

            if (maybeUser.HasNoValue)
            {
                return Result.Failure<EntityCreatedResponse>(ValidationErrors.User.NotFound);
            }

            var createBudgetRequest = new CreateBudgetRequest(
                maybeUser.Value,
                request.Name,
                request.Categories,
                request.Amount,
                request.Currency,
                request.StartDate,
                request.EndDate);

            Result<Budget> budgetResult = _budgetFactory.Create(createBudgetRequest);

            if (budgetResult.IsFailure)
            {
                return Result.Failure<EntityCreatedResponse>(budgetResult.Error);
            }

            await _budgetRepository.AddAsync(budgetResult.Value, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new EntityCreatedResponse(budgetResult.Value.Id);
        }
    }
}
