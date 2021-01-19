﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Abstractions.Authentication;
using Expensely.Application.Commands.Users.CreateUserTokenForCredentials;
using Expensely.Common.Abstractions.Messaging;
using Expensely.Contracts.Users;
using Expensely.Domain.Abstractions.Maybe;
using Expensely.Domain.Abstractions.Result;
using Expensely.Domain.Core;
using Expensely.Domain.Errors;
using Expensely.Domain.Repositories;
using Expensely.Domain.Services;

namespace Expensely.Application.Commands.Handlers.Users.CreateUserTokenForCredentials
{
    /// <summary>
    /// Represents the <see cref="CreateUserTokenForCredentialsCommand"/> handler.
    /// </summary>
    internal sealed class CreateUserTokenForCredentialsCommandHandler
        : ICommandHandler<CreateUserTokenForCredentialsCommand, Result<TokenResponse>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordService _passwordService;
        private readonly IJwtProvider _jwtProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateUserTokenForCredentialsCommandHandler"/> class.
        /// </summary>
        /// <param name="userRepository">The user repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="passwordService">The password service.</param>
        /// <param name="jwtProvider">The JWT provider.</param>
        public CreateUserTokenForCredentialsCommandHandler(
            IUserRepository userRepository,
            IUnitOfWork unitOfWork,
            IPasswordService passwordService,
            IJwtProvider jwtProvider)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _passwordService = passwordService;
            _jwtProvider = jwtProvider;
        }

        /// <inheritdoc />
        public async Task<Result<TokenResponse>> Handle(CreateUserTokenForCredentialsCommand request, CancellationToken cancellationToken)
        {
            Result<Email> emailResult = Email.Create(request.Email);
            Result<Password> passwordResult = Password.Create(request.Password);

            var result = Result.FirstFailureOrSuccess(emailResult, passwordResult);

            if (result.IsFailure)
            {
                return Result.Failure<TokenResponse>(result.Error);
            }

            Maybe<User> maybeUser = await _userRepository.GetByEmailAsync(emailResult.Value, cancellationToken);

            if (maybeUser.HasNoValue)
            {
                return Result.Failure<TokenResponse>(DomainErrors.User.InvalidEmailOrPassword);
            }

            User user = maybeUser.Value;

            if (!user.VerifyPassword(passwordResult.Value, _passwordService))
            {
                return Result.Failure<TokenResponse>(DomainErrors.User.InvalidEmailOrPassword);
            }

            string token = _jwtProvider.CreateToken(user);

            // TODO: Move refresh token logic into user entity.
            // (string refreshToken, DateTime expiresOnUtc) = _jwtProvider.CreateRefreshToken();
            // Maybe<RefreshToken> maybeRefreshToken = await _dbContext.FirstOrDefaultAsync(
            //    new RefreshTokenByUserSpecification(user),
            //    cancellationToken);
            // if (maybeRefreshToken.HasNoValue)
            // {
            //     _dbContext.Insert(new RefreshToken(user, refreshToken, expiresOnUtc));
            // }
            // else
            // {
            //     maybeRefreshToken.Value.ChangeValues(refreshToken, expiresOnUtc);
            // }
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // TODO: Fix refresh token and expiration values.
            return new TokenResponse(token, string.Empty, DateTime.UtcNow);
        }
    }
}
