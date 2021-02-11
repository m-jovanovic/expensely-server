using System;
using Expensely.Domain.Abstractions.Result;
using Expensely.Domain.Contracts;
using Expensely.Domain.Core;
using Expensely.Domain.Errors;
using Expensely.Domain.Modules.Users;

namespace Expensely.Domain.Services
{
    /// <summary>
    /// Represents the transaction details validator.
    /// </summary>
    public sealed class TransactionDetailsValidator
    {
        /// <summary>
        /// Validates the provided transaction information and returns the result of the validation.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="description">The description.</param>
        /// <param name="categoryValue">The category value.</param>
        /// <param name="amount">The amount.</param>
        /// <param name="currencyValue">The currency value.</param>
        /// <param name="occurredOn">The occurred on date.</param>
        /// <param name="transactionTypeValue">The transaction type.</param>
        /// <returns>The result of the transaction validation process containing the transaction information or an error.</returns>
        public Result<TransactionDetails> Validate(
            User user,
            string description,
            int categoryValue,
            decimal amount,
            int currencyValue,
            DateTime occurredOn,
            int transactionTypeValue)
        {
            Result<Description> descriptionResult = Description.Create(description);

            if (descriptionResult.IsFailure)
            {
                return Result.Failure<TransactionDetails>(descriptionResult.Error);
            }

            Currency currency = Currency.FromValue(currencyValue).Value;

            if (!user.HasCurrency(currency))
            {
                return Result.Failure<TransactionDetails>(DomainErrors.User.CurrencyDoesNotExist);
            }

            var money = new Money(amount, currency);

            TransactionType transactionType = TransactionType.FromValue(transactionTypeValue).Value;

            Result transactionTypeResult = transactionType.ValidateAmount(money);

            if (transactionTypeResult.IsFailure)
            {
                return Result.Failure<TransactionDetails>(transactionTypeResult.Error);
            }

            return new TransactionDetails
            {
                UserId = user.Id,
                Description = descriptionResult.Value,
                Category = Category.FromValue(categoryValue).Value,
                Money = money,
                OccurredOn = occurredOn,
                TransactionType = transactionType
            };
        }
    }
}
