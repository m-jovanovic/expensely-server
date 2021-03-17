﻿using System.Threading;
using System.Threading.Tasks;
using Expensely.Common.Primitives.Result;
using Expensely.Common.Primitives.ServiceLifetimes;
using Expensely.Domain.Errors;

namespace Expensely.Domain.Modules.Users
{
    /// <summary>
    /// Represents the user factory.
    /// </summary>
    public sealed class UserFactory : IUserFactory, IScoped
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordService _passwordService;
        private readonly IRoleProvider _roleProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserFactory"/> class.
        /// </summary>
        /// <param name="userRepository">The user repository.</param>
        /// <param name="passwordService">The password service.</param>
        /// <param name="roleProvider">The role provider.</param>
        public UserFactory(IUserRepository userRepository, IPasswordService passwordService, IRoleProvider roleProvider)
        {
            _passwordService = passwordService;
            _roleProvider = roleProvider;
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

            foreach (string role in _roleProvider.GetStandardRoles())
            {
                user.AddRole(role);
            }

            return user;
        }
    }
}
