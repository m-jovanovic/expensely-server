﻿using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Abstractions.Authentication;
using Expensely.Application.Abstractions.Data;
using Expensely.Application.Abstractions.Messaging;
using Expensely.Application.Contracts.Users;
using Expensely.Domain.Core;
using Expensely.Domain.Core.Errors;
using Expensely.Domain.Primitives.Maybe;
using Expensely.Domain.Primitives.Result;
using Expensely.Domain.Services;
using Microsoft.EntityFrameworkCore;

namespace Expensely.Application.Users.Commands.CreateUserTokenForCredentials
{
    /// <summary>
    /// Represents the <see cref="CreateUserTokenForCredentialsCommand"/> handler.
    /// </summary>
    internal sealed class CreateUserTokenForCredentialsCommandHandler
        : ICommandHandler<CreateUserTokenForCredentialsCommand, Result<TokenResponse>>
    {
        private readonly IDbContext _dbContext;
        private readonly IPasswordService _passwordService;
        private readonly IJwtProvider _jwtProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateUserTokenForCredentialsCommandHandler"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="passwordService">The password service.</param>
        /// <param name="jwtProvider">The JWT provider.</param>
        public CreateUserTokenForCredentialsCommandHandler(
            IDbContext dbContext,
            IJwtProvider jwtProvider,
            IPasswordService passwordService)
        {
            _dbContext = dbContext;
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

            Maybe<User> maybeUser = await _dbContext.Set<User>()
                .FirstOrDefaultAsync(u => u.Email.Value == emailResult.Value, cancellationToken);

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

            return new TokenResponse(token);
        }
    }
}
