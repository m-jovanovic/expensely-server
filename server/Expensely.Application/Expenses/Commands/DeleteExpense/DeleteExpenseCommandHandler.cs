﻿using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Abstractions.Authentication;
using Expensely.Application.Abstractions.Data;
using Expensely.Application.Abstractions.Messaging;
using Expensely.Application.Validation;
using Expensely.Domain.Core;
using Expensely.Domain.Primitives.Maybe;
using Expensely.Domain.Primitives.Result;

namespace Expensely.Application.Expenses.Commands.DeleteExpense
{
    /// <summary>
    /// Represents the <see cref="DeleteExpenseCommand"/> handler.
    /// </summary>
    internal sealed class DeleteExpenseCommandHandler : ICommandHandler<DeleteExpenseCommand, Result>
    {
        private readonly IDbContext _dbContext;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserIdentifierProvider _userIdentifierProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteExpenseCommandHandler"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="userIdentifierProvider">The user identifier provider.</param>
        public DeleteExpenseCommandHandler(IDbContext dbContext, IUnitOfWork unitOfWork, IUserIdentifierProvider userIdentifierProvider)
        {
            _dbContext = dbContext;
            _unitOfWork = unitOfWork;
            _userIdentifierProvider = userIdentifierProvider;
        }

        /// <inheritdoc />
        public async Task<Result> Handle(DeleteExpenseCommand request, CancellationToken cancellationToken)
        {
            Maybe<Expense> maybeExpense = await _dbContext.GetBydIdAsync<Expense>(request.ExpenseId);

            if (maybeExpense.HasNoValue)
            {
                return Result.Failure(Errors.Expense.NotFound);
            }

            Expense expense = maybeExpense.Value;

            if (expense.UserId != _userIdentifierProvider.UserId)
            {
                return Result.Failure(Errors.User.InvalidPermissions);
            }

            _dbContext.Remove(expense);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}