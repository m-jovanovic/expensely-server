using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Abstractions.Authentication;
using Expensely.Application.Abstractions.Data;
using Expensely.Application.Commands.Authentication;
using Expensely.Application.Commands.Handlers.Validation;
using Expensely.Application.Contracts.Authentication;
using Expensely.Common.Abstractions.Messaging;
using Expensely.Common.Primitives.Maybe;
using Expensely.Common.Primitives.Result;
using Expensely.Domain.Modules.Users;

namespace Expensely.Application.Commands.Handlers.Authentication.Login
{
    /// <summary>
    /// Represents the <see cref="LoginCommand"/> handler.
    /// </summary>
    public sealed class LoginCommandHandler : ICommandHandler<LoginCommand, Result<TokenResponse>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtProvider _jwtProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginCommandHandler"/> class.
        /// </summary>
        /// <param name="userRepository">The user repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="passwordHasher">The password hasher.</param>
        /// <param name="jwtProvider">The JWT provider.</param>
        public LoginCommandHandler(
            IUserRepository userRepository,
            IUnitOfWork unitOfWork,
            IPasswordHasher passwordHasher,
            IJwtProvider jwtProvider)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
            _jwtProvider = jwtProvider;
        }

        /// <inheritdoc />
        public async Task<Result<TokenResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            // TODO: Make sure that when the request gets here there is no need to check value objects.
            Result<Email> emailResult = Email.Create(request.Email);
            Result<Password> passwordResult = Password.Create(request.Password);

            var result = Result.FirstFailureOrSuccess(emailResult, passwordResult);

            if (result.IsFailure)
            {
                return Result.Failure<TokenResponse>(ValidationErrors.User.InvalidEmailOrPassword);
            }

            Maybe<User> maybeUser = await _userRepository.GetByEmailAsync(emailResult.Value, cancellationToken);

            if (maybeUser.HasNoValue)
            {
                return Result.Failure<TokenResponse>(ValidationErrors.User.InvalidEmailOrPassword);
            }

            User user = maybeUser.Value;

            if (!user.VerifyPassword(passwordResult.Value, _passwordHasher))
            {
                return Result.Failure<TokenResponse>(ValidationErrors.User.InvalidEmailOrPassword);
            }

            AccessTokens accessTokens = _jwtProvider.GetAccessTokens(user);

            user.ChangeRefreshToken(accessTokens.RefreshToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return accessTokens.CreateTokenResponse();
        }
    }
}
