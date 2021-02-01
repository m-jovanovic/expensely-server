using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Commands.Transactions;
using Expensely.Common.Abstractions.Messaging;
using Expensely.Domain.Abstractions.Maybe;
using Expensely.Domain.Abstractions.Result;
using Expensely.Domain.Contracts;
using Expensely.Domain.Core;
using Expensely.Domain.Errors;
using Expensely.Domain.Modules.Users;
using Expensely.Domain.Repositories;
using Expensely.Domain.Services;

namespace Expensely.Application.Commands.Handlers.Transactions.CreateTransaction
{
    /// <summary>
    /// Represents the <see cref="CreateTransactionCommand"/> handler.
    /// </summary>
    internal sealed class CreateTransactionCommandHandler : ICommandHandler<CreateTransactionCommand, Result>
    {
        private readonly IUserRepository _userRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateTransactionCommandHandler"/> class.
        /// </summary>
        /// <param name="userRepository">The user repository.</param>
        /// <param name="transactionRepository">The transaction repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        public CreateTransactionCommandHandler(
            IUserRepository userRepository,
            ITransactionRepository transactionRepository,
            IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _transactionRepository = transactionRepository;
            _unitOfWork = unitOfWork;
        }

        /// <inheritdoc />
        public async Task<Result> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
        {
            Maybe<User> maybeUser = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);

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
                request.TransactionType);

            if (transactionDetailsResult.IsFailure)
            {
                return Result.Failure(transactionDetailsResult.Error);
            }

            var transaction = new Transaction(transactionDetailsResult.Value);

            await _transactionRepository.AddAsync(transaction, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
