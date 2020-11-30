using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Abstractions.Data;
using Expensely.Application.Commands.Handlers.Abstractions;
using Expensely.Application.Commands.Incomes.CreateIncome;
using Expensely.Domain.Core;
using Expensely.Domain.Primitives.Result;

namespace Expensely.Application.Commands.Handlers.Incomes.CreateIncome
{
    /// <summary>
    /// Represents the <see cref="CreateIncomeCommand"/> handler.
    /// </summary>
    internal sealed class CreateIncomeCommandHandler : ICommandHandler<CreateIncomeCommand, Result>
    {
        private readonly IDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateIncomeCommandHandler"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        public CreateIncomeCommandHandler(IDbContext dbContext) => _dbContext = dbContext;

        /// <inheritdoc />
        public async Task<Result> Handle(CreateIncomeCommand request, CancellationToken cancellationToken)
        {
            Result<Name> nameResult = Name.Create(request.Name);
            Result<Description> descriptionResult = Description.Create(request.Description);

            var result = Result.FirstFailureOrSuccess(nameResult, descriptionResult);

            if (result.IsFailure)
            {
                return Result.Failure(result.Error);
            }

            var income = new Income(
                request.UserId,
                nameResult.Value,
                new Money(request.Amount, Currency.FromValue(request.Currency).Value),
                request.OccurredOn,
                descriptionResult.Value);

            _dbContext.Insert(income);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
