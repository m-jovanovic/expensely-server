using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Abstractions.Authentication;
using Expensely.Application.Abstractions.Data;
using Expensely.Application.Commands.Handlers.Abstractions;
using Expensely.Application.Commands.Handlers.Validation;
using Expensely.Application.Commands.Incomes.UpdateIncome;
using Expensely.Domain.Core;
using Expensely.Domain.Core.Errors;
using Expensely.Domain.Primitives.Maybe;
using Expensely.Domain.Primitives.Result;

namespace Expensely.Application.Commands.Handlers.Incomes.UpdateIncome
{
    /// <summary>
    /// Represents the <see cref="UpdateIncomeCommand"/> handler.
    /// </summary>
    internal sealed class UpdateIncomeCommandHandler : ICommandHandler<UpdateIncomeCommand, Result>
    {
        private readonly IDbContext _dbContext;
        private readonly IUserInformationProvider _userInformationProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateIncomeCommandHandler"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="userInformationProvider">The user information provider.</param>
        public UpdateIncomeCommandHandler(IDbContext dbContext, IUserInformationProvider userInformationProvider)
        {
            _dbContext = dbContext;
            _userInformationProvider = userInformationProvider;
        }

        /// <inheritdoc />
        public async Task<Result> Handle(UpdateIncomeCommand request, CancellationToken cancellationToken)
        {
            Maybe<Income> maybeIncome = await _dbContext.GetBydIdAsync<Income>(request.IncomeId);

            if (maybeIncome.HasNoValue)
            {
                return Result.Failure(DomainErrors.Expense.NotFound);
            }

            Income income = maybeIncome.Value;

            if (income.UserId != _userInformationProvider.UserId)
            {
                return Result.Failure(ValidationErrors.User.InvalidPermissions);
            }

            Result<Name> nameResult = Name.Create(request.Name);
            Result<Description> descriptionResult = Description.Create(request.Description);

            var result = Result.FirstFailureOrSuccess(nameResult, descriptionResult);

            if (result.IsFailure)
            {
                return Result.Failure(result.Error);
            }

            income.ChangeMoney(new Money(request.Amount, Currency.FromValue(request.Currency).Value));

            income.ChangeName(nameResult.Value);

            income.ChangeDescription(descriptionResult.Value);

            income.ChangeOccurredOnDate(request.OccurredOn);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
