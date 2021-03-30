using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Abstractions.Data;
using Expensely.Application.Commands.Authentication;
using Expensely.Common.Abstractions.Messaging;
using Expensely.Common.Primitives.Result;
using Expensely.Domain.Modules.Users;

namespace Expensely.Application.Commands.Handlers.Users.CreateUser
{
    /// <summary>
    /// Represents the <see cref="RegisterCommand"/> handler.
    /// </summary>
    public sealed class RegisterCommandHandler : ICommandHandler<RegisterCommand, Result>
    {
        private readonly IUserFactory _userFactory;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="RegisterCommandHandler"/> class.
        /// </summary>
        /// <param name="userFactory">The user factory.</param>
        /// <param name="userRepository">The user repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        public RegisterCommandHandler(
            IUserFactory userFactory,
            IUserRepository userRepository,
            IUnitOfWork unitOfWork)
        {
            _userFactory = userFactory;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        /// <inheritdoc />
        public async Task<Result> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            Result<User> userResult = await _userFactory.CreateAsync(
                request.FirstName,
                request.LastName,
                request.Email,
                request.Password,
                cancellationToken);

            if (userResult.IsFailure)
            {
                return Result.Failure(userResult.Error);
            }

            await _userRepository.AddAsync(userResult.Value, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
