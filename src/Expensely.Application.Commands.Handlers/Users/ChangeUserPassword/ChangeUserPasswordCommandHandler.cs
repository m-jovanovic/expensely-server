using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Abstractions.Data;
using Expensely.Application.Commands.Handlers.Validation;
using Expensely.Application.Commands.Users;
using Expensely.Common.Abstractions.Messaging;
using Expensely.Common.Primitives.Maybe;
using Expensely.Common.Primitives.Result;
using Expensely.Domain.Modules.Users;

namespace Expensely.Application.Commands.Handlers.Users.ChangeUserPassword
{
    /// <summary>
    /// Represents the <see cref="ChangeUserPasswordCommand"/> handler.
    /// </summary>
    public sealed class ChangeUserPasswordCommandHandler : ICommandHandler<ChangeUserPasswordCommand, Result>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher _passwordHasher;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChangeUserPasswordCommandHandler"/> class.
        /// </summary>
        /// <param name="userRepository">The user repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="passwordHasher">The password hasher.</param>
        public ChangeUserPasswordCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork, IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
        }

        /// <inheritdoc />
        public async Task<Result> Handle(ChangeUserPasswordCommand request, CancellationToken cancellationToken)
        {
            Result<Password> currentPasswordResult = Password.Create(request.CurrentPassword);
            Result<Password> newPasswordResult = Password.Create(request.NewPassword);

            var result = Result.FirstFailureOrSuccess(currentPasswordResult, newPasswordResult);

            if (result.IsFailure)
            {
                return Result.Failure(result.Error);
            }

            Maybe<User> maybeUser = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);

            if (maybeUser.HasNoValue)
            {
                return Result.Failure(ValidationErrors.User.NotFound);
            }

            Result changePasswordResult = maybeUser.Value.ChangePassword(
                currentPasswordResult.Value,
                newPasswordResult.Value,
                _passwordHasher);

            if (changePasswordResult.IsFailure)
            {
                return Result.Failure(changePasswordResult.Error);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
