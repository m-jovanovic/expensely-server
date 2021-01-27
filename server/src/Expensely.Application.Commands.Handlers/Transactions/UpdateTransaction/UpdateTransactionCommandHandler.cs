using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Abstractions.Authentication;
using Expensely.Application.Commands.Handlers.Validation;
using Expensely.Application.Commands.Transactions;
using Expensely.Common.Abstractions.Messaging;
using Expensely.Domain.Abstractions.Maybe;
using Expensely.Domain.Abstractions.Result;
using Expensely.Domain.Contracts;
using Expensely.Domain.Core;
using Expensely.Domain.Errors;
using Expensely.Domain.Repositories;
using Expensely.Domain.Services;

namespace Expensely.Application.Commands.Handlers.Transactions.UpdateTransaction
{
    /// <summary>
    /// Represents the <see cref="UpdateTransactionCommand"/> handler.
    /// </summary>
    internal sealed class UpdateTransactionCommandHandler : ICommandHandler<UpdateTransactionCommand, Result>
    {
        private readonly IUserRepository _userRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserInformationProvider _userInformationProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateTransactionCommandHandler"/> class.
        /// </summary>
        /// <param name="userRepository">The user repository.</param>
        /// <param name="transactionRepository">The transaction repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="userInformationProvider">The user information provider.</param>
        public UpdateTransactionCommandHandler(
            IUserRepository userRepository,
            ITransactionRepository transactionRepository,
            IUnitOfWork unitOfWork,
            IUserInformationProvider userInformationProvider)
        {
            _userRepository = userRepository;
            _transactionRepository = transactionRepository;
            _unitOfWork = unitOfWork;
            _userInformationProvider = userInformationProvider;
        }

        /// <inheritdoc />
        public async Task<Result> Handle(UpdateTransactionCommand request, CancellationToken cancellationToken)
        {
            Maybe<Transaction> maybeTransaction = await _transactionRepository
                .GetByIdWithUserAsync(request.TransactionId, cancellationToken);

            if (maybeTransaction.HasNoValue)
            {
                return Result.Failure(DomainErrors.Transaction.NotFound);
            }

            Transaction transaction = maybeTransaction.Value;

            if (transaction.UserId != _userInformationProvider.UserId)
            {
                return Result.Failure(ValidationErrors.User.InvalidPermissions);
            }

            Maybe<User> maybeUser = await _userRepository.GetByIdAsync(transaction.UserId, cancellationToken);

            if (maybeUser.HasNoValue)
            {
                return Result.Failure(DomainErrors.User.NotFound);
            }

            Result<TransactionDetails> transactionDetailsResult = new TransactionDetailsValidator().Validate(
                maybeUser.Value,
                request.Name,
                request.Description,
                request.Category,
                request.Amount,
                request.Currency,
                request.OccurredOn,
                transaction.TransactionType.Value);

            if (transactionDetailsResult.IsFailure)
            {
                return Result.Failure(transactionDetailsResult.Error);
            }

            transaction.Update(transactionDetailsResult.Value);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
