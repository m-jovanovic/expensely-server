using System;
using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Abstractions.Authentication;
using Expensely.Application.Abstractions.Data;
using Expensely.Application.Commands.Handlers.Specifications.RefreshTokens;
using Expensely.Application.Commands.Users.RefreshUserToken;
using Expensely.Common.Clock;
using Expensely.Common.Messaging;
using Expensely.Contracts.Users;
using Expensely.Domain.Core;
using Expensely.Domain.Core.Errors;
using Expensely.Domain.Primitives.Maybe;
using Expensely.Domain.Primitives.Result;

namespace Expensely.Application.Commands.Handlers.Users.RefreshUserToken
{
    /// <summary>
    /// Represents the <see cref="RefreshUserTokenCommand"/> handler.
    /// </summary>
    internal sealed class RefreshUserTokenCommandHandler : ICommandHandler<RefreshUserTokenCommand, Result<TokenResponse>>
    {
        private readonly IDbContext _dbContext;
        private readonly IJwtProvider _jwtProvider;
        private readonly IDateTime _dateTime;

        /// <summary>
        /// Initializes a new instance of the <see cref="RefreshUserTokenCommandHandler"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="jwtProvider">The JWT provider.</param>
        /// <param name="dateTime">The date and time.</param>
        public RefreshUserTokenCommandHandler(IDbContext dbContext, IJwtProvider jwtProvider, IDateTime dateTime)
        {
            _dbContext = dbContext;
            _jwtProvider = jwtProvider;
            _dateTime = dateTime;
        }

        /// <inheritdoc />
        public async Task<Result<TokenResponse>> Handle(RefreshUserTokenCommand request, CancellationToken cancellationToken)
        {
            Maybe<RefreshToken> maybeRefreshToken = await _dbContext
                .FirstOrDefaultAsync(new RefreshTokenByValueSpecification(request.RefreshToken));

            if (maybeRefreshToken.HasNoValue)
            {
                return Result.Failure<TokenResponse>(DomainErrors.RefreshToken.NotFound);
            }

            RefreshToken refreshTokenEntity = maybeRefreshToken.Value;

            if (refreshTokenEntity.Expired(_dateTime.UtcNow))
            {
                return Result.Failure<TokenResponse>(DomainErrors.RefreshToken.Expired);
            }

            Maybe<User> maybeUser = await _dbContext.GetBydIdAsync<User>(refreshTokenEntity.UserId);

            if (maybeUser.HasNoValue)
            {
                return Result.Failure<TokenResponse>(DomainErrors.User.NotFound);
            }

            string token = _jwtProvider.CreateToken(maybeUser.Value);

            (string refreshToken, DateTime expiresOnUtc) = _jwtProvider.CreateRefreshToken();

            refreshTokenEntity.ChangeValues(refreshToken, expiresOnUtc);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return new TokenResponse(token, refreshToken, expiresOnUtc);
        }
    }
}
