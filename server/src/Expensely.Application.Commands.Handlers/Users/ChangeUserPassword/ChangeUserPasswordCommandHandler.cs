﻿using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Commands.Users;
using Expensely.Common.Abstractions.Messaging;
using Expensely.Domain.Abstractions.Maybe;
using Expensely.Domain.Abstractions.Result;
using Expensely.Domain.Core;
using Expensely.Domain.Errors;
using Expensely.Domain.Modules.Users;
using Expensely.Domain.Repositories;
using Expensely.Domain.Services;

namespace Expensely.Application.Commands.Handlers.Users.ChangeUserPassword
{
    /// <summary>
    /// Represents the <see cref="ChangeUserPasswordCommand"/> handler.
    /// </summary>
    internal sealed class ChangeUserPasswordCommandHandler : ICommandHandler<ChangeUserPasswordCommand, Result>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordService _passwordService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChangeUserPasswordCommandHandler"/> class.
        /// </summary>
        /// <param name="userRepository">The user repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="passwordService">The password service.</param>
        public ChangeUserPasswordCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork, IPasswordService passwordService)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _passwordService = passwordService;
        }

        /// <inheritdoc />
        public async Task<Result> Handle(ChangeUserPasswordCommand request, CancellationToken cancellationToken)
        {
            Result<Password> currentPasswordResult = Password.Create(request.CurrentPassword);
            Result<Password> newPasswordResult = Password.Create(request.NewPassword);

            var result = Result.FirstFailureOrSuccess(currentPasswordResult, newPasswordResult);

            if (result.IsFailure)
            {
                return Result.Failure(result.Error);
            }

            Maybe<User> maybeUser = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);

            if (maybeUser.HasNoValue)
            {
                return Result.Failure(DomainErrors.User.NotFound);
            }

            Result changePasswordResult = maybeUser.Value.ChangePassword(
                currentPasswordResult.Value,
                newPasswordResult.Value,
                _passwordService);

            if (changePasswordResult.IsFailure)
            {
                return Result.Failure(changePasswordResult.Error);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
