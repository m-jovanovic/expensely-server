using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Abstractions.Data;
using Expensely.Application.Commands.Budgets.CreateBudget;
using Expensely.Common.Messaging;
using Expensely.Domain.Abstractions.Result;
using Expensely.Domain.Core;

namespace Expensely.Application.Commands.Handlers.Budgets.CreateBudget
{
    /// <summary>
    /// Represents the <see cref="CreateBudgetCommand"/> handler.
    /// </summary>
    internal sealed class CreateBudgetCommandHandler : ICommandHandler<CreateBudgetCommand, Result>
    {
        private readonly IDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateBudgetCommandHandler"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        public CreateBudgetCommandHandler(IDbContext dbContext) => _dbContext = dbContext;

        /// <inheritdoc />
        public async Task<Result> Handle(CreateBudgetCommand request, CancellationToken cancellationToken)
        {
            Result<Name> nameResult = Name.Create(request.Name);

            if (nameResult.IsFailure)
            {
                return Result.Failure(nameResult.Error);
            }

            var budget = new Budget(
                request.UserId,
                nameResult.Value,
                new Money(request.Amount, Currency.FromValue(request.Currency).Value),
                request.StartDate,
                request.EndDate);

            // TODO: Add domain rule about the allowed # of budgets.
            _dbContext.Insert(budget);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
