using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Commands.Expenses;
using Expensely.Common.Abstractions.Messaging;
using Expensely.Domain.Abstractions.Maybe;
using Expensely.Domain.Abstractions.Result;
using Expensely.Domain.Contracts;
using Expensely.Domain.Core;
using Expensely.Domain.Errors;
using Expensely.Domain.Repositories;
using Expensely.Domain.Services;

namespace Expensely.Application.Commands.Handlers.Expenses.CreateExpense
{
    /// <summary>
    /// Represents the <see cref="CreateExpenseCommand"/> handler.
    /// </summary>
    internal sealed class CreateExpenseCommandHandler : ICommandHandler<CreateExpenseCommand, Result>
    {
        private readonly IUserRepository _userRepository;
        private readonly IExpenseRepository _expenseRepository;
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateExpenseCommandHandler"/> class.
        /// </summary>
        /// <param name="userRepository">The user repository.</param>
        /// <param name="expenseRepository">The expense repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        public CreateExpenseCommandHandler(IUserRepository userRepository, IExpenseRepository expenseRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _expenseRepository = expenseRepository;
            _unitOfWork = unitOfWork;
        }

        /// <inheritdoc />
        public async Task<Result> Handle(CreateExpenseCommand request, CancellationToken cancellationToken)
        {
            Maybe<User> maybeUser = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);

            if (maybeUser.HasNoValue)
            {
                return Result.Failure(DomainErrors.User.NotFound);
            }

            Result<TransactionInformation> transactionInformationResult = new TransactionInformationValidator().Validate(
                maybeUser.Value,
                request.Name,
                request.Description,
                request.Category,
                request.Amount,
                request.Currency,
                request.OccurredOn);

            if (transactionInformationResult.IsFailure)
            {
                return Result.Failure(transactionInformationResult.Error);
            }

            var expense = Expense.Create(transactionInformationResult.Value);

            await _expenseRepository.AddAsync(expense, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
