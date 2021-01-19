using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Abstractions.Authentication;
using Expensely.Application.Abstractions.Messaging;
using Expensely.Application.Commands.Expenses.DeleteExpense;
using Expensely.Application.Commands.Handlers.Validation;
using Expensely.Common.Abstractions.Messaging;
using Expensely.Domain.Abstractions.Maybe;
using Expensely.Domain.Abstractions.Result;
using Expensely.Domain.Core;
using Expensely.Domain.Errors;
using Expensely.Domain.Events.Expenses;
using Expensely.Domain.Repositories;

namespace Expensely.Application.Commands.Handlers.Expenses.DeleteExpense
{
    /// <summary>
    /// Represents the <see cref="DeleteExpenseCommand"/> handler.
    /// </summary>
    internal sealed class DeleteExpenseCommandHandler : ICommandHandler<DeleteExpenseCommand, Result>
    {
        private readonly IExpenseRepository _expenseRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserInformationProvider _userInformationProvider;
        private readonly IEventPublisher _eventPublisher;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteExpenseCommandHandler"/> class.
        /// </summary>
        /// <param name="expenseRepository">The expense repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="userInformationProvider">The user information provider.</param>
        /// <param name="eventPublisher">The event publisher.</param>
        public DeleteExpenseCommandHandler(
            IExpenseRepository expenseRepository,
            IUnitOfWork unitOfWork,
            IUserInformationProvider userInformationProvider,
            IEventPublisher eventPublisher)
        {
            _expenseRepository = expenseRepository;
            _userInformationProvider = userInformationProvider;
            _eventPublisher = eventPublisher;
            _unitOfWork = unitOfWork;
        }

        /// <inheritdoc />
        public async Task<Result> Handle(DeleteExpenseCommand request, CancellationToken cancellationToken)
        {
            Maybe<Expense> maybeExpense = await _expenseRepository.GetByIdAsync(request.ExpenseId, cancellationToken);

            if (maybeExpense.HasNoValue)
            {
                return Result.Failure(DomainErrors.Expense.NotFound);
            }

            Expense expense = maybeExpense.Value;

            if (expense.UserId != _userInformationProvider.UserId)
            {
                return Result.Failure(ValidationErrors.User.InvalidPermissions);
            }

            _expenseRepository.Remove(expense);

            // TODO: Figure out how to make this a single transaction.
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // TODO: Figure out how to make this a single transaction.
            await _eventPublisher.PublishAsync(new ExpenseDeletedEvent
            {
                UserId = expense.UserId,
                Category = expense.Category.Value,
                Amount = expense.Money.Amount,
                Currency = expense.Money.Currency.Value,
                OccurredOn = expense.OccurredOn
            });

            return Result.Success();
        }
    }
}
