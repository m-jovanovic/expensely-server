using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Abstractions.Authentication;
using Expensely.Application.Abstractions.Data;
using Expensely.Application.Commands.Handlers.Validation;
using Expensely.Application.Commands.Incomes.DeleteIncome;
using Expensely.Common.Messaging;
using Expensely.Domain.Core;
using Expensely.Domain.Core.Errors;
using Expensely.Domain.Primitives.Maybe;
using Expensely.Domain.Primitives.Result;

namespace Expensely.Application.Commands.Handlers.Incomes.DeleteIncome
{
    /// <summary>
    /// Represents the <see cref="DeleteIncomeCommand"/> handler.
    /// </summary>
    internal sealed class DeleteIncomeCommandHandler : ICommandHandler<DeleteIncomeCommand, Result>
    {
        private readonly IDbContext _dbContext;
        private readonly IUserInformationProvider _userInformationProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteIncomeCommandHandler"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="userInformationProvider">The user information provider.</param>
        public DeleteIncomeCommandHandler(IDbContext dbContext, IUserInformationProvider userInformationProvider)
        {
            _dbContext = dbContext;
            _userInformationProvider = userInformationProvider;
        }

        /// <inheritdoc />
        public async Task<Result> Handle(DeleteIncomeCommand request, CancellationToken cancellationToken)
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

            _dbContext.Remove(income);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
