﻿using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Commands.Budgets;
using Expensely.Common.Abstractions.Messaging;
using Expensely.Domain.Abstractions.Maybe;
using Expensely.Domain.Abstractions.Result;
using Expensely.Domain.Core;
using Expensely.Domain.Errors;
using Expensely.Domain.Modules.Users;
using Expensely.Domain.Repositories;

namespace Expensely.Application.Commands.Handlers.Budgets.CreateBudget
{
    /// <summary>
    /// Represents the <see cref="CreateBudgetCommand"/> handler.
    /// </summary>
    internal sealed class CreateBudgetCommandHandler : ICommandHandler<CreateBudgetCommand, Result>
    {
        private readonly IUserRepository _userRepository;
        private readonly IBudgetRepository _budgetRepository;
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateBudgetCommandHandler"/> class.
        /// </summary>
        /// <param name="userRepository">The user repository.</param>
        /// <param name="budgetRepository">The budget repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        public CreateBudgetCommandHandler(IUserRepository userRepository, IBudgetRepository budgetRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _budgetRepository = budgetRepository;
            _unitOfWork = unitOfWork;
        }

        /// <inheritdoc />
        public async Task<Result> Handle(CreateBudgetCommand request, CancellationToken cancellationToken)
        {
            Result<Name> nameResult = Name.Create(request.Name);

            if (nameResult.IsFailure)
            {
                return Result.Failure(nameResult.Error);
            }

            Maybe<User> maybeUser = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);

            if (maybeUser.HasNoValue)
            {
                return Result.Failure(DomainErrors.User.NotFound);
            }

            var budget = new Budget(
                maybeUser.Value.Id,
                nameResult.Value,
                new Money(request.Amount, Currency.FromValue(request.Currency).Value),
                request.StartDate,
                request.EndDate);

            // TODO: Add domain rule about the allowed # of budgets.
            await _budgetRepository.AddAsync(budget, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
