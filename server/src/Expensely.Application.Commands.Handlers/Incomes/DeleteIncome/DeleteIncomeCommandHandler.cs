using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Abstractions.Authentication;
using Expensely.Application.Abstractions.Messaging;
using Expensely.Application.Commands.Handlers.Validation;
using Expensely.Application.Commands.Incomes.DeleteIncome;
using Expensely.Common.Abstractions.Messaging;
using Expensely.Domain.Abstractions.Maybe;
using Expensely.Domain.Abstractions.Result;
using Expensely.Domain.Core;
using Expensely.Domain.Errors;
using Expensely.Domain.Events.Incomes;
using Expensely.Domain.Repositories;

namespace Expensely.Application.Commands.Handlers.Incomes.DeleteIncome
{
    /// <summary>
    /// Represents the <see cref="DeleteIncomeCommand"/> handler.
    /// </summary>
    internal sealed class DeleteIncomeCommandHandler : ICommandHandler<DeleteIncomeCommand, Result>
    {
        private readonly IIncomeRepository _incomeRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserInformationProvider _userInformationProvider;
        private readonly IEventPublisher _eventPublisher;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteIncomeCommandHandler"/> class.
        /// </summary>
        /// <param name="incomeRepository">The income repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="userInformationProvider">The user information provider.</param>
        /// <param name="eventPublisher">The event publisher.</param>
        public DeleteIncomeCommandHandler(
            IIncomeRepository incomeRepository,
            IUnitOfWork unitOfWork,
            IUserInformationProvider userInformationProvider,
            IEventPublisher eventPublisher)
        {
            _incomeRepository = incomeRepository;
            _unitOfWork = unitOfWork;
            _userInformationProvider = userInformationProvider;
            _eventPublisher = eventPublisher;
        }

        /// <inheritdoc />
        public async Task<Result> Handle(DeleteIncomeCommand request, CancellationToken cancellationToken)
        {
            Maybe<Income> maybeIncome = await _incomeRepository.GetByIdAsync(request.IncomeId, cancellationToken);

            if (maybeIncome.HasNoValue)
            {
                return Result.Failure(DomainErrors.Expense.NotFound);
            }

            Income income = maybeIncome.Value;

            if (income.UserId != _userInformationProvider.UserId)
            {
                return Result.Failure(ValidationErrors.User.InvalidPermissions);
            }

            _incomeRepository.Remove(income);

            // TODO: Figure out how to make this a single transaction.
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // TODO: Figure out how to make this a single transaction.
            await _eventPublisher.PublishAsync(new IncomeDeletedEvent
            {
                UserId = income.UserId,
                Category = income.Category.Value,
                Amount = income.Money.Amount,
                Currency = income.Money.Currency.Value,
                OccurredOn = income.OccurredOn
            });

            return Result.Success();
        }
    }
}
