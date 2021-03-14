using System.Threading;
using System.Threading.Tasks;
using Expensely.Common.Primitives.Result;
using Expensely.Domain.Errors;

namespace Expensely.Domain.Modules.Users
{
    /// <summary>
    /// Represents the user factory.
    /// </summary>
    public sealed class UserFactory : IUserFactory
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordService _passwordService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserFactory"/> class.
        /// </summary>
        /// <param name="userRepository">The user repository.</param>
        /// <param name="passwordService">The password service.</param>
        public UserFactory(IUserRepository userRepository, IPasswordService passwordService)
        {
            _passwordService = passwordService;
            _userRepository = userRepository;
        }

        /// <inheritdoc />
        public async Task<Result<User>> Create(
            string firstName,
            string lastName,
            string email,
            string password,
            CancellationToken cancellationToken)
        {
            Result<FirstName> firstNameResult = FirstName.Create(firstName);
            Result<LastName> lastNameResult = LastName.Create(lastName);
            Result<Email> emailResult = Email.Create(email);
            Result<Password> passwordResult = Password.Create(password);

            var result = Result.FirstFailureOrSuccess(firstNameResult, lastNameResult, emailResult, passwordResult);

            if (result.IsFailure)
            {
                return Result.Failure<User>(result.Error);
            }

            bool emailAlreadyInUse = await _userRepository.AnyWithEmailAsync(emailResult.Value, cancellationToken);

            if (emailAlreadyInUse)
            {
                return Result.Failure<User>(DomainErrors.User.EmailAlreadyInUse);
            }

            var user = User.Create(
                firstNameResult.Value,
                lastNameResult.Value,
                emailResult.Value,
                passwordResult.Value,
                _passwordService);

            return user;
        }
    }
}
