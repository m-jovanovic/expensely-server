﻿using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Abstractions.Authentication;
using Expensely.Application.Abstractions.Data;
using Expensely.Application.Abstractions.Messaging;
using Expensely.Application.Validation;
using Expensely.Domain.Core;
using Expensely.Domain.Primitives.Maybe;
using Expensely.Domain.Primitives.Result;

namespace Expensely.Application.Users.Commands.RemoveUserCurrency
{
    /// <summary>
    /// Represents the <see cref="RemoveUserCurrencyCommand"/> handler.
    /// </summary>
    internal sealed class RemoveUserCurrencyCommandHandler : ICommandHandler<RemoveUserCurrencyCommand, Result>
    {
        private readonly IDbContext _dbContext;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserInformationProvider _userInformationProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="RemoveUserCurrencyCommandHandler"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="userInformationProvider">The user information provider.</param>
        public RemoveUserCurrencyCommandHandler(
            IDbContext dbContext,
            IUnitOfWork unitOfWork,
            IUserInformationProvider userInformationProvider)
        {
            _dbContext = dbContext;
            _unitOfWork = unitOfWork;
            _userInformationProvider = userInformationProvider;
        }

        /// <inheritdoc />
        public async Task<Result> Handle(RemoveUserCurrencyCommand request, CancellationToken cancellationToken)
        {
            if (request.UserId != _userInformationProvider.UserId)
            {
                return Result.Failure(Errors.User.InvalidPermissions);
            }

            Maybe<User> maybeUser = await _dbContext.GetBydIdAsync<User>(request.UserId);

            if (maybeUser.HasNoValue)
            {
                return Result.Failure(Errors.User.NotFound);
            }

            Currency currency = Currency.FromValue(request.Currency).Value;

            Result result = maybeUser.Value.RemoveCurrency(currency);

            if (result.IsFailure)
            {
                return Result.Failure(result.Error);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
