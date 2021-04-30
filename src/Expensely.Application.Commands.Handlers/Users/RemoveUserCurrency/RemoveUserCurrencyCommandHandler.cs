using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Abstractions.Data;
using Expensely.Application.Commands.Handlers.Validation;
using Expensely.Application.Commands.Users;
using Expensely.Common.Abstractions.Messaging;
using Expensely.Common.Primitives.Maybe;
using Expensely.Common.Primitives.Result;
using Expensely.Domain.Modules.Common;
using Expensely.Domain.Modules.Users;

namespace Expensely.Application.Commands.Handlers.Users.RemoveUserCurrency
{
    /// <summary>
    /// Represents the <see cref="RemoveUserCurrencyCommand"/> handler.
    /// </summary>
    public sealed class RemoveUserCurrencyCommandHandler : ICommandHandler<RemoveUserCurrencyCommand, Result>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="RemoveUserCurrencyCommandHandler"/> class.
        /// </summary>
        /// <param name="userRepository">The user repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        public RemoveUserCurrencyCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        /// <inheritdoc />
        public async Task<Result> Handle(RemoveUserCurrencyCommand request, CancellationToken cancellationToken)
        {
            Maybe<User> maybeUser = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);

            if (maybeUser.HasNoValue)
            {
                return Result.Failure(ValidationErrors.User.NotFound);
            }

            Currency currency = Currency.FromValue(request.Currency).Value;

            Result result = maybeUser.Value.RemoveCurrency(currency);

            if (result.IsFailure)
            {
                return Result.Failure(result.Error);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
