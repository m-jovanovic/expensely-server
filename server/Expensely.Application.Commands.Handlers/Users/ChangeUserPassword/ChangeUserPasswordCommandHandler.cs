using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Abstractions.Data;
using Expensely.Application.Commands.Users.ChangeUserPassword;
using Expensely.Common.Messaging;
using Expensely.Domain.Core;
using Expensely.Domain.Core.Errors;
using Expensely.Domain.Primitives.Maybe;
using Expensely.Domain.Primitives.Result;
using Expensely.Domain.Services;

namespace Expensely.Application.Commands.Handlers.Users.ChangeUserPassword
{
    /// <summary>
    /// Represents the <see cref="ChangeUserPasswordCommand"/> handler.
    /// </summary>
    internal sealed class ChangeUserPasswordCommandHandler : ICommandHandler<ChangeUserPasswordCommand, Result>
    {
        private readonly IDbContext _dbContext;
        private readonly IPasswordService _passwordService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChangeUserPasswordCommandHandler"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="passwordService">The password service.</param>
        public ChangeUserPasswordCommandHandler(IDbContext dbContext, IPasswordService passwordService)
        {
            _dbContext = dbContext;
            _passwordService = passwordService;
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

            Maybe<User> maybeUser = await _dbContext.GetBydIdAsync<User>(request.UserId);

            if (maybeUser.HasNoValue)
            {
                return Result.Failure(DomainErrors.User.NotFound);
            }

            Result changePasswordResult = maybeUser.Value.ChangePassword(
                currentPasswordResult.Value,
                newPasswordResult.Value,
                _passwordService);

            if (changePasswordResult.IsFailure)
            {
                return Result.Failure(changePasswordResult.Error);
            }

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
