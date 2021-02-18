using System;
using Expensely.Domain.Contracts;
using Expensely.Domain.Errors;
using Expensely.Domain.Modules.Shared;
using Expensely.Domain.Modules.Transactions;
using Expensely.Domain.Modules.Users;
using Expensely.Domain.Primitives.Result;

namespace Expensely.Domain.Services
{
    /// <summary>
    /// Represents the transaction details validator.
    /// </summary>
    public sealed class TransactionDetailsValidator : ITransactionDetailsValidator
    {
        /// <inheritdoc />
        public Result<TransactionDetails> Validate(
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
                return Result.Failure<TransactionDetails>(descriptionResult.Error);
            }

            Currency currency = Currency.FromValue(currencyId).Value;

            if (!user.HasCurrency(currency))
            {
                return Result.Failure<TransactionDetails>(DomainErrors.User.CurrencyDoesNotExist);
            }

            var money = new Money(amount, currency);

            TransactionType transactionType = TransactionType.FromValue(transactionTypeId).Value;

            Result transactionTypeResult = transactionType.ValidateAmount(money);

            if (transactionTypeResult.IsFailure)
            {
                return Result.Failure<TransactionDetails>(transactionTypeResult.Error);
            }

            return new TransactionDetails
            {
                Description = descriptionResult.Value,
                Category = Category.FromValue(categoryId).Value,
                Money = money,
                OccurredOn = occurredOn,
                TransactionType = transactionType
            };
        }
    }
}
