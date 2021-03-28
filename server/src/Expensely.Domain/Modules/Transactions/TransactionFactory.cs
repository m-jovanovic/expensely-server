using System;
using Expensely.Common.Primitives.Result;
using Expensely.Common.Primitives.ServiceLifetimes;
using Expensely.Domain.Modules.Users;

namespace Expensely.Domain.Modules.Transactions
{
    /// <summary>
    /// Represents the transaction factory.
    /// </summary>
    public sealed class TransactionFactory : ITransactionFactory, ITransient
    {
        private readonly ITransactionDetailsValidator _transactionDetailsValidator;

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionFactory"/> class.
        /// </summary>
        /// <param name="transactionDetailsValidator">The transaction details validator.</param>
        public TransactionFactory(ITransactionDetailsValidator transactionDetailsValidator) =>
            _transactionDetailsValidator = transactionDetailsValidator;

        /// <inheritdoc />
        public Result<Transaction> Create(
            User user,
            string description,
            int categoryId,
            decimal amount,
            int currencyId,
            DateTime occurredOn,
            int transactionTypeId)
        {
            Result<ITransactionDetails> transactionDetailsResult = _transactionDetailsValidator
                .Validate(user, description, categoryId, amount, currencyId, occurredOn, transactionTypeId);

            if (transactionDetailsResult.IsFailure)
            {
                return Result.Failure<Transaction>(transactionDetailsResult.Error);
            }

            ITransactionDetails transactionDetails = transactionDetailsResult.Value;

            var transaction = new Transaction(
                user,
                transactionDetails.Description,
                transactionDetails.Category,
                transactionDetails.Money,
                transactionDetails.OccurredOn,
                transactionDetails.TransactionType);

            return transaction;
        }
    }
}
