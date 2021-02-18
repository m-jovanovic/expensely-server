using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Abstractions.Authentication;
using Expensely.Application.Abstractions.Data;
using Expensely.Application.Commands.Users;
using Expensely.Application.Contracts.Users;
using Expensely.Common.Abstractions.Messaging;
using Expensely.Domain.Abstractions.Maybe;
using Expensely.Domain.Abstractions.Result;
using Expensely.Domain.Errors;
using Expensely.Domain.Modules.Authentication;
using Expensely.Domain.Modules.Users;
using Expensely.Domain.Repositories;
using Expensely.Domain.Services;

namespace Expensely.Application.Commands.Handlers.Users.CreateUserToken
{
    /// <summary>
    /// Represents the <see cref="CreateUserTokenCommand"/> handler.
    /// </summary>
    public sealed class CreateUserTokenCommandHandler : ICommandHandler<CreateUserTokenCommand, Result<TokenResponse>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordService _passwordService;
        private readonly IJwtProvider _jwtProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateUserTokenCommandHandler"/> class.
        /// </summary>
        /// <param name="userRepository">The user repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="passwordService">The password service.</param>
        /// <param name="jwtProvider">The JWT provider.</param>
        public CreateUserTokenCommandHandler(
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
        public async Task<Result<TokenResponse>> Handle(CreateUserTokenCommand request, CancellationToken cancellationToken)
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

            RefreshToken refreshToken = _jwtProvider.CreateRefreshToken();

            user.ChangeRefreshToken(refreshToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new TokenResponse(token, refreshToken.Token, refreshToken.ExpiresOnUtc);
        }
    }
}
