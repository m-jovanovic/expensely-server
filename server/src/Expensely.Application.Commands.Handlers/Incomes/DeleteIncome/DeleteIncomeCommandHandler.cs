using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Abstractions.Authentication;
using Expensely.Application.Abstractions.Data;
using Expensely.Application.Abstractions.Messaging;
using Expensely.Application.Commands.Handlers.Validation;
using Expensely.Application.Commands.Incomes.DeleteIncome;
using Expensely.Common.Abstractions.Messaging;
using Expensely.Domain.Abstractions.Maybe;
using Expensely.Domain.Abstractions.Result;
using Expensely.Domain.Core;
using Expensely.Domain.Errors;
using Expensely.Domain.Events.Incomes;

namespace Expensely.Application.Commands.Handlers.Incomes.DeleteIncome
{
    /// <summary>
    /// Represents the <see cref="DeleteIncomeCommand"/> handler.
    /// </summary>
    internal sealed class DeleteIncomeCommandHandler : ICommandHandler<DeleteIncomeCommand, Result>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IUserInformationProvider _userInformationProvider;
        private readonly IEventPublisher _eventPublisher;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteIncomeCommandHandler"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="userInformationProvider">The user information provider.</param>
        /// <param name="eventPublisher">The event publisher.</param>
        public DeleteIncomeCommandHandler(
            IApplicationDbContext dbContext,
            IUserInformationProvider userInformationProvider,
            IEventPublisher eventPublisher)
        {
            _dbContext = dbContext;
            _userInformationProvider = userInformationProvider;
            _eventPublisher = eventPublisher;
        }

        /// <inheritdoc />
        public async Task<Result> Handle(DeleteIncomeCommand request, CancellationToken cancellationToken)
        {
            Maybe<Income> maybeIncome = await _dbContext.GetBydIdAsync<Income>(request.IncomeId, cancellationToken);

            if (maybeIncome.HasNoValue)
            {
                return Result.Failure(DomainErrors.Expense.NotFound);
            }

            Income income = maybeIncome.Value;

            if (income.UserId != _userInformationProvider.UserId)
            {
                return Result.Failure(ValidationErrors.User.InvalidPermissions);
            }

            _dbContext.Remove(income);

            await _dbContext.SaveChangesAsync(cancellationToken);

            await _eventPublisher.PublishAsync(new IncomeDeletedEvent
            {
                Amount = income.Money.Amount,
                Currency = income.Money.Currency.Value
            });

            return Result.Success();
        }
    }
}
