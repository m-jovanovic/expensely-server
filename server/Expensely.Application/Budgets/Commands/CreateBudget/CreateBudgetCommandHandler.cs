using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Abstractions.Authentication;
using Expensely.Application.Abstractions.Data;
using Expensely.Application.Abstractions.Messaging;
using Expensely.Application.Validation;
using Expensely.Domain.Core;
using Expensely.Domain.Primitives.Maybe;
using Expensely.Domain.Primitives.Result;

namespace Expensely.Application.Budgets.Commands.CreateBudget
{
    /// <summary>
    /// Represents the <see cref="CreateBudgetCommand"/> handler.
    /// </summary>
    internal sealed class CreateBudgetCommandHandler : ICommandHandler<CreateBudgetCommand, Result>
    {
        private readonly IDbContext _dbContext;
        private readonly IUserInformationProvider _userInformationProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateBudgetCommandHandler"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="userInformationProvider">The user information provider.</param>
        public CreateBudgetCommandHandler(IDbContext dbContext, IUserInformationProvider userInformationProvider)
        {
            _dbContext = dbContext;
            _userInformationProvider = userInformationProvider;
        }

        /// <inheritdoc />
        public async Task<Result> Handle(CreateBudgetCommand request, CancellationToken cancellationToken)
        {
            if (request.UserId != _userInformationProvider.UserId)
            {
                return Result.Failure(ValidationErrors.User.InvalidPermissions);
            }

            Result<Name> nameResult = Name.Create(request.Name);

            if (nameResult.IsFailure)
            {
                return Result.Failure(nameResult.Error);
            }

            Maybe<Currency> maybeCurrency = Currency.FromValue(request.Currency);

            if (maybeCurrency.HasNoValue)
            {
                return Result.Failure(ValidationErrors.Currency.NotFound);
            }

            var budget = new Budget(
                request.UserId,
                nameResult.Value,
                new Money(request.Amount, maybeCurrency.Value),
                request.StartDate,
                request.EndDate);

            // TODO: Add domain rule about the allowed # of budgets.
            _dbContext.Insert(budget);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
