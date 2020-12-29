using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Abstractions.Data;
using Expensely.Application.Commands.Incomes.CreateIncome;
using Expensely.Common.Abstractions.Messaging;
using Expensely.Domain.Abstractions.Maybe;
using Expensely.Domain.Abstractions.Result;
using Expensely.Domain.Contracts;
using Expensely.Domain.Core;
using Expensely.Domain.Errors;
using Expensely.Domain.Services;

namespace Expensely.Application.Commands.Handlers.Incomes.CreateIncome
{
    /// <summary>
    /// Represents the <see cref="CreateIncomeCommand"/> handler.
    /// </summary>
    internal sealed class CreateIncomeCommandHandler : ICommandHandler<CreateIncomeCommand, Result>
    {
        private readonly IApplicationDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateIncomeCommandHandler"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        public CreateIncomeCommandHandler(IApplicationDbContext dbContext) => _dbContext = dbContext;

        /// <inheritdoc />
        public async Task<Result> Handle(CreateIncomeCommand request, CancellationToken cancellationToken)
        {
            Maybe<User> maybeUser = await _dbContext.GetBydIdAsync<User>(request.UserId, cancellationToken);

            if (maybeUser.HasNoValue)
            {
                return Result.Failure(DomainErrors.User.NotFound);
            }

            Result<TransactionInformation> transactionInformationResult = new TransactionInformationService().Validate(
                maybeUser.Value,
                request.Name,
                request.Description,
                default,
                request.Currency);

            if (transactionInformationResult.IsFailure)
            {
                return Result.Failure(transactionInformationResult.Error);
            }

            // TODO: Implement support for categories.
            var income = Income.Create(
                maybeUser.Value.Id,
                transactionInformationResult.Value.Name,
                Category.UnCategorized,
                new Money(request.Amount, transactionInformationResult.Value.Currency),
                request.OccurredOn,
                transactionInformationResult.Value.Description);

            _dbContext.Insert(income);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
