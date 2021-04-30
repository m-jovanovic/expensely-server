using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Abstractions.Data;
using Expensely.Application.Commands.Handlers.Validation;
using Expensely.Application.Commands.Transactions;
using Expensely.Application.Contracts.Common;
using Expensely.Common.Abstractions.Messaging;
using Expensely.Common.Primitives.Maybe;
using Expensely.Common.Primitives.Result;
using Expensely.Domain.Modules.Transactions;
using Expensely.Domain.Modules.Transactions.Contracts;
using Expensely.Domain.Modules.Users;

namespace Expensely.Application.Commands.Handlers.Transactions.CreateTransaction
{
    /// <summary>
    /// Represents the <see cref="CreateTransactionCommand"/> handler.
    /// </summary>
    public sealed class CreateTransactionCommandHandler : ICommandHandler<CreateTransactionCommand, Result<EntityCreatedResponse>>
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
        public async Task<Result<EntityCreatedResponse>> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
        {
            Maybe<User> maybeUser = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);

            if (maybeUser.HasNoValue)
            {
                return Result.Failure<EntityCreatedResponse>(ValidationErrors.User.NotFound);
            }

            var createTransactionRequest = new CreateTransactionRequest(
                maybeUser.Value,
                request.Description,
                request.Category,
                request.Amount,
                request.Currency,
                request.OccurredOn,
                request.TransactionType);

            Result<Transaction> transactionResult = _transactionFactory.Create(createTransactionRequest);

            if (transactionResult.IsFailure)
            {
                return Result.Failure<EntityCreatedResponse>(transactionResult.Error);
            }

            await _transactionRepository.AddAsync(transactionResult.Value, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new EntityCreatedResponse(transactionResult.Value.Id);
        }
    }
}
