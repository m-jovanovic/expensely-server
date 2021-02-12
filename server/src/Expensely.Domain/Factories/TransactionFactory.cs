using System;
using Expensely.Domain.Abstractions.Result;
using Expensely.Domain.Core;
using Expensely.Domain.Errors;
using Expensely.Domain.Modules.Users;

namespace Expensely.Domain.Factories
{
    /// <summary>
    /// Represents the transaction factory.
    /// </summary>
    public sealed class TransactionFactory : ITransactionFactory
    {
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
            Result<Description> descriptionResult = Description.Create(description);

            if (descriptionResult.IsFailure)
            {
                return Result.Failure<Transaction>(descriptionResult.Error);
            }

            Currency currency = Currency.FromValue(currencyId).Value;

            if (!user.HasCurrency(currency))
            {
                return Result.Failure<Transaction>(DomainErrors.User.CurrencyDoesNotExist);
            }

            var money = new Money(amount, currency);

            TransactionType transactionType = TransactionType.FromValue(transactionTypeId).Value;

            Result transactionTypeResult = transactionType.ValidateAmount(money);

            if (transactionTypeResult.IsFailure)
            {
                return Result.Failure<Transaction>(transactionTypeResult.Error);
            }

            var transaction = new Transaction(
                user,
                descriptionResult.Value,
                Category.FromValue(categoryId).Value,
                money,
                occurredOn,
                transactionType);

            return transaction;
        }
    }
}
