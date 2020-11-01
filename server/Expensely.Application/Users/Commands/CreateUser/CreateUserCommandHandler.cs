using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Abstractions.Authentication;
using Expensely.Application.Abstractions.Data;
using Expensely.Application.Abstractions.Messaging;
using Expensely.Application.Contracts.Users;
using Expensely.Application.Validation;
using Expensely.Domain.Core;
using Expensely.Domain.Primitives.Result;
using Expensely.Domain.Services;

namespace Expensely.Application.Users.Commands.CreateUser
{
    /// <summary>
    /// Represents the <see cref="CreateUserCommand"/> handler.
    /// </summary>
    internal sealed class CreateUserCommandHandler : ICommandHandler<CreateUserCommand, Result<TokenResponse>>
    {
        private readonly IDbContext _dbContext;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordService _passwordService;
        private readonly IJwtProvider _jwtProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateUserCommandHandler"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="passwordService">The password service.</param>
        /// <param name="jwtProvider">The JWT provider.</param>
        public CreateUserCommandHandler(IDbContext dbContext, IUnitOfWork unitOfWork, IPasswordService passwordService, IJwtProvider jwtProvider)
        {
            _dbContext = dbContext;
            _unitOfWork = unitOfWork;
            _passwordService = passwordService;
            _jwtProvider = jwtProvider;
        }

        /// <inheritdoc />
        public async Task<Result<TokenResponse>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            Result<FirstName> firstNameResult = FirstName.Create(request.FirstName);
            Result<LastName> lastNameResult = LastName.Create(request.LastName);
            Result<Email> emailResult = Email.Create(request.Email);
            Result<Password> passwordResult = Password.Create(request.Password);

            var result = Result.FirstFailureOrSuccess(firstNameResult, lastNameResult, emailResult, passwordResult);

            if (result.IsFailure)
            {
                return Result.Failure<TokenResponse>(result.Error);
            }

            bool emailAlreadyExists = await _dbContext.AnyAsync<User>(x => x.Email.Value == emailResult.Value);

            if (emailAlreadyExists)
            {
                return Result.Failure<TokenResponse>(Errors.User.EmailAlreadyInUse);
            }

            var user = new User(firstNameResult.Value, lastNameResult.Value, emailResult.Value, passwordResult.Value, _passwordService);

            _dbContext.Insert(user);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            string token = _jwtProvider.CreateToken(user);

            return new TokenResponse(token);
        }
    }
}
