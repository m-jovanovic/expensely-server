using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Abstractions.Data;
using Expensely.Application.Commands.Handlers.Specifications.Users;
using Expensely.Application.Commands.Users.CreateUser;
using Expensely.Common.Abstractions.Messaging;
using Expensely.Domain.Abstractions.Result;
using Expensely.Domain.Core;
using Expensely.Domain.Errors;
using Expensely.Domain.Services;

namespace Expensely.Application.Commands.Handlers.Users.CreateUser
{
    /// <summary>
    /// Represents the <see cref="CreateUserCommand"/> handler.
    /// </summary>
    internal sealed class CreateUserCommandHandler : ICommandHandler<CreateUserCommand, Result>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IPasswordService _passwordService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateUserCommandHandler"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="passwordService">The password service.</param>
        public CreateUserCommandHandler(IApplicationDbContext dbContext, IPasswordService passwordService)
        {
            _dbContext = dbContext;
            _passwordService = passwordService;
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

            bool emailAlreadyExists = await _dbContext.AnyAsync(new UserByEmailSpecification(emailResult.Value), cancellationToken);

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

            _dbContext.Insert(user);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
