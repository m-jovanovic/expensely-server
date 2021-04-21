using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Abstractions.Data;
using Expensely.Application.Commands.Handlers.Validation;
using Expensely.Application.Commands.Users;
using Expensely.Common.Abstractions.Messaging;
using Expensely.Common.Primitives.Maybe;
using Expensely.Common.Primitives.Result;
using Expensely.Domain.Modules.Users;

namespace Expensely.Application.Commands.Handlers.Users.ChangeUserName
{
    /// <summary>
    /// Represents the <see cref="ChangeUserNameCommand"/> handler.
    /// </summary>
    internal sealed class ChangeUserNameCommandHandler : ICommandHandler<ChangeUserNameCommand, Result>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChangeUserNameCommandHandler"/> class.
        /// </summary>
        /// <param name="userRepository">The user repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        public ChangeUserNameCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        /// <inheritdoc />
        public async Task<Result> Handle(ChangeUserNameCommand request, CancellationToken cancellationToken)
        {
            Result<FirstName> firstNameResult = FirstName.Create(request.FirstName);
            Result<LastName> lastNameResult = LastName.Create(request.LastName);

            var result = Result.FirstFailureOrSuccess(firstNameResult, lastNameResult);

            if (result.IsFailure)
            {
                return Result.Failure(result.Error);
            }

            Maybe<User> maybeUser = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);

            if (maybeUser.HasNoValue)
            {
                return Result.Failure(ValidationErrors.User.NotFound);
            }

            maybeUser.Value.ChangeName(firstNameResult.Value, lastNameResult.Value);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
