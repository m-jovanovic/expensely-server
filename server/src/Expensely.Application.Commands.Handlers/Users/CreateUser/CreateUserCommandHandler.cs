using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Abstractions.Data;
using Expensely.Application.Abstractions.Messaging;
using Expensely.Application.Commands.Users;
using Expensely.Domain.Errors;
using Expensely.Domain.Modules.Users;
using Expensely.Domain.Primitives.Result;

namespace Expensely.Application.Commands.Handlers.Users.CreateUser
{
    /// <summary>
    /// Represents the <see cref="CreateUserCommand"/> handler.
    /// </summary>
    public sealed class CreateUserCommandHandler : ICommandHandler<CreateUserCommand, Result>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordService _passwordService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateUserCommandHandler"/> class.
        /// </summary>
        /// <param name="userRepository">The user repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="passwordService">The password service.</param>
        public CreateUserCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork, IPasswordService passwordService)
        {
            _passwordService = passwordService;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        /// <inheritdoc />
        public async Task<Result> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            Result<FirstName> firstNameResult = FirstName.Create(request.FirstName);
            Result<LastName> lastNameResult = LastName.Create(request.LastName);
            Result<Email> emailResult = Email.Create(request.Email);
            Result<Password> passwordResult = Password.Create(request.Password);

            var result = Result.FirstFailureOrSuccess(firstNameResult, lastNameResult, emailResult, passwordResult);

            if (result.IsFailure)
            {
                return Result.Failure(result.Error);
            }

            bool emailAlreadyExists = await _userRepository.AnyWithEmailAsync(emailResult.Value, cancellationToken);

            if (emailAlreadyExists)
            {
                return Result.Failure(DomainErrors.User.EmailAlreadyInUse);
            }

            var user = User.Create(
                firstNameResult.Value,
                lastNameResult.Value,
                emailResult.Value,
                passwordResult.Value,
                _passwordService);

            await _userRepository.AddAsync(user, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
