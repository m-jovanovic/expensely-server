using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Abstractions.Authentication;
using Expensely.Application.Abstractions.Data;
using Expensely.Application.Abstractions.Messaging;
using Expensely.Application.Expenses.Commands.CreateExpense;
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
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserIdentifierProvider _userIdentifierProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateBudgetCommandHandler"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="userIdentifierProvider">The user identifier provider.</param>
        public CreateBudgetCommandHandler(IDbContext dbContext, IUnitOfWork unitOfWork, IUserIdentifierProvider userIdentifierProvider)
        {
            _dbContext = dbContext;
            _unitOfWork = unitOfWork;
            _userIdentifierProvider = userIdentifierProvider;
        }

        /// <inheritdoc />
        public async Task<Result> Handle(CreateBudgetCommand request, CancellationToken cancellationToken)
        {
            // TODO: Add domain rule about the allowed # of budgets.
            if (request.UserId != _userIdentifierProvider.UserId)
            {
                return Result.Failure(Errors.User.InvalidPermissions);
            }

            Result<Name> nameResult = Name.Create(request.Name);

            if (nameResult.IsFailure)
            {
                return Result.Failure(nameResult.Error);
            }

            Maybe<Currency> maybeCurrency = Currency.FromValue(request.Currency);

            if (maybeCurrency.HasNoValue)
            {
                return Result.Failure(Errors.Currency.NotFound);
            }

            var budget = new Budget(
                request.UserId,
                nameResult.Value,
                new Money(request.Amount, maybeCurrency.Value),
                request.StartDate,
                request.EndDate);

            _dbContext.Insert(budget);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
