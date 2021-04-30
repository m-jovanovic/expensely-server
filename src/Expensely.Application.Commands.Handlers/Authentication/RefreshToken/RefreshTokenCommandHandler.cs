using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Abstractions.Authentication;
using Expensely.Application.Abstractions.Data;
using Expensely.Application.Commands.Authentication;
using Expensely.Application.Commands.Handlers.Validation;
using Expensely.Application.Contracts.Authentication;
using Expensely.Common.Abstractions.Clock;
using Expensely.Common.Abstractions.Messaging;
using Expensely.Common.Primitives.Maybe;
using Expensely.Common.Primitives.Result;
using Expensely.Domain.Modules.Users;

namespace Expensely.Application.Commands.Handlers.Authentication.RefreshToken
{
    /// <summary>
    /// Represents the <see cref="RefreshTokenCommand"/> handler.
    /// </summary>
    public sealed class RefreshTokenCommandHandler : ICommandHandler<RefreshTokenCommand, Result<TokenResponse>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtProvider _jwtProvider;
        private readonly ISystemTime _systemTime;

        /// <summary>
        /// Initializes a new instance of the <see cref="RefreshTokenCommandHandler"/> class.
        /// </summary>
        /// <param name="userRepository">The user repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="jwtProvider">The JWT provider.</param>
        /// <param name="systemTime">The system time.</param>
        public RefreshTokenCommandHandler(
            IUserRepository userRepository,
            IUnitOfWork unitOfWork,
            IJwtProvider jwtProvider,
            ISystemTime systemTime)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _jwtProvider = jwtProvider;
            _systemTime = systemTime;
        }

        /// <inheritdoc />
        public async Task<Result<TokenResponse>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            Maybe<User> maybeUser = await _userRepository.GetByRefreshTokenAsync(request.RefreshToken, cancellationToken);

            if (maybeUser.HasNoValue)
            {
                return Result.Failure<TokenResponse>(ValidationErrors.User.NotFound);
            }

            User user = maybeUser.Value;

            if (user.RefreshToken.IsExpired(_systemTime.UtcNow))
            {
                return Result.Failure<TokenResponse>(ValidationErrors.RefreshToken.Expired);
            }

            AccessTokens accessTokens = _jwtProvider.GetAccessTokens(user);

            user.ChangeRefreshToken(accessTokens.RefreshToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return accessTokens.CreateTokenResponse();
        }
    }
}
