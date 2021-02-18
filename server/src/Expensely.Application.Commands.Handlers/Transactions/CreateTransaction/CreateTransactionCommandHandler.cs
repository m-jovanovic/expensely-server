using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Abstractions.Data;
using Expensely.Application.Abstractions.Messaging;
using Expensely.Application.Commands.Transactions;
using Expensely.Domain.Errors;
using Expensely.Domain.Factories;
using Expensely.Domain.Modules.Transactions;
using Expensely.Domain.Modules.Users;
using Expensely.Domain.Primitives.Maybe;
using Expensely.Domain.Primitives.Result;
using Expensely.Domain.Repositories;

namespace Expensely.Application.Commands.Handlers.Transactions.CreateTransaction
{
    /// <summary>
    /// Represents the <see cref="CreateTransactionCommand"/> handler.
    /// </summary>
    public sealed class CreateTransactionCommandHandler : ICommandHandler<CreateTransactionCommand, Result>
    {
        private readonly IUserRepository _userRepository;
        private readonly ITransactionFactory _transactionFactory;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateTransactionCommandHandler"/> class.
        /// </summary>
        /// <param name="userRepository">The user repository.</param>
        /// <param name="transactionFactory">The transaction factory.</param>
        /// <param name="transactionRepository">The transaction repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        public CreateTransactionCommandHandler(
            IUserRepository userRepository,
            ITransactionFactory transactionFactory,
            ITransactionRepository transactionRepository,
            IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _transactionFactory = transactionFactory;
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

            Result<Transaction> transactionResult = _transactionFactory.Create(
                maybeUser.Value,
                request.Description,
                request.Category,
                request.Amount,
                request.Currency,
                request.OccurredOn,
                request.TransactionType);

            if (transactionResult.IsFailure)
            {
                return Result.Failure(transactionResult.Error);
            }

            await _transactionRepository.AddAsync(transactionResult.Value, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
