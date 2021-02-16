using System;
using Expensely.Domain.Abstractions.Result;
using Expensely.Domain.Contracts;
using Expensely.Domain.Modules.Transactions;
using Expensely.Domain.Modules.Users;
using Expensely.Domain.Services;

namespace Expensely.Domain.Factories
{
    /// <summary>
    /// Represents the transaction factory.
    /// </summary>
    public sealed class TransactionFactory : ITransactionFactory
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
            Result<TransactionDetails> transactionDetailsResult = _transactionDetailsValidator.Validate(
                user,
                description,
                categoryId,
                amount,
                currencyId,
                occurredOn,
                transactionTypeId);

            if (transactionDetailsResult.IsFailure)
            {
                return Result.Failure<Transaction>(transactionDetailsResult.Error);
            }

            var transaction = new Transaction(
                user,
                transactionDetailsResult.Value.Description,
                transactionDetailsResult.Value.Category,
                transactionDetailsResult.Value.Money,
                transactionDetailsResult.Value.OccurredOn,
                transactionDetailsResult.Value.TransactionType);

            return transaction;
        }
    }
}
