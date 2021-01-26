using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Abstractions.Authentication;
using Expensely.Application.Commands.Users;
using Expensely.Common.Abstractions.Clock;
using Expensely.Common.Abstractions.Messaging;
using Expensely.Contracts.Users;
using Expensely.Domain.Abstractions.Maybe;
using Expensely.Domain.Abstractions.Result;
using Expensely.Domain.Core;
using Expensely.Domain.Errors;
using Expensely.Domain.Repositories;

namespace Expensely.Application.Commands.Handlers.Users.RefreshUserToken
{
    /// <summary>
    /// Represents the <see cref="RefreshUserTokenCommand"/> handler.
    /// </summary>
    internal sealed class RefreshUserTokenCommandHandler : ICommandHandler<RefreshUserTokenCommand, Result<TokenResponse>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtProvider _jwtProvider;
        private readonly IDateTime _dateTime;

        /// <summary>
        /// Initializes a new instance of the <see cref="RefreshUserTokenCommandHandler"/> class.
        /// </summary>
        /// <param name="userRepository">The user repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="jwtProvider">The JWT provider.</param>
        /// <param name="dateTime">The date and time.</param>
        public RefreshUserTokenCommandHandler(
            IUserRepository userRepository,
            IUnitOfWork unitOfWork,
            IJwtProvider jwtProvider,
            IDateTime dateTime)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _jwtProvider = jwtProvider;
            _dateTime = dateTime;
        }

        /// <inheritdoc />
        public async Task<Result<TokenResponse>> Handle(RefreshUserTokenCommand request, CancellationToken cancellationToken)
        {
            Maybe<User> maybeUser = await _userRepository.GetByRefreshTokenAsync(request.RefreshToken, cancellationToken);

            if (maybeUser.HasNoValue)
            {
                return Result.Failure<TokenResponse>(DomainErrors.User.NotFound);
            }

            User user = maybeUser.Value;

            if (user.RefreshToken.IsExpired(_dateTime.UtcNow))
            {
                return Result.Failure<TokenResponse>(DomainErrors.RefreshToken.Expired);
            }

            string token = _jwtProvider.CreateToken(user);

            RefreshToken refreshToken = _jwtProvider.CreateRefreshToken();

            user.ChangeRefreshToken(refreshToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new TokenResponse(token, refreshToken.Token, refreshToken.ExpiresOnUtc);
        }
    }
}
