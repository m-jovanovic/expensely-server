using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Abstractions.Authentication;
using Expensely.Application.Abstractions.Data;
using Expensely.Application.Commands.Handlers.Validation;
using Expensely.Application.Commands.Incomes.UpdateIncome;
using Expensely.Common.Abstractions.Messaging;
using Expensely.Domain.Abstractions.Maybe;
using Expensely.Domain.Abstractions.Result;
using Expensely.Domain.Contracts;
using Expensely.Domain.Core;
using Expensely.Domain.Errors;
using Expensely.Domain.Services;

namespace Expensely.Application.Commands.Handlers.Incomes.UpdateIncome
{
    /// <summary>
    /// Represents the <see cref="UpdateIncomeCommand"/> handler.
    /// </summary>
    internal sealed class UpdateIncomeCommandHandler : ICommandHandler<UpdateIncomeCommand, Result>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IUserInformationProvider _userInformationProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateIncomeCommandHandler"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="userInformationProvider">The user information provider.</param>
        public UpdateIncomeCommandHandler(IApplicationDbContext dbContext, IUserInformationProvider userInformationProvider)
        {
            _dbContext = dbContext;
            _userInformationProvider = userInformationProvider;
        }

        /// <inheritdoc />
        public async Task<Result> Handle(UpdateIncomeCommand request, CancellationToken cancellationToken)
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

            Maybe<User> maybeUser = await _dbContext.GetBydIdAsync<User>(income.UserId, cancellationToken);

            if (maybeUser.HasNoValue)
            {
                return Result.Failure(DomainErrors.User.NotFound);
            }

            Result<TransactionInformation> transactionInformationResult = new TransactionInformationService().Validate(
                maybeUser.Value,
                request.Name,
                request.Description,
                request.Category,
                request.Currency);

            if (transactionInformationResult.IsFailure)
            {
                return Result.Failure(transactionInformationResult.Error);
            }

            income.Update(
                transactionInformationResult.Value.Name,
                transactionInformationResult.Value.Category,
                new Money(request.Amount, transactionInformationResult.Value.Currency),
                request.OccurredOn,
                transactionInformationResult.Value.Description);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
