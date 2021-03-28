using System;
using Expensely.Common.Primitives.Result;
using Expensely.Common.Primitives.ServiceLifetimes;
using Expensely.Domain.Errors;
using Expensely.Domain.Modules.Common;
using Expensely.Domain.Modules.Users;

namespace Expensely.Domain.Modules.Transactions
{
    /// <summary>
    /// Represents the transaction details validator.
    /// </summary>
    public sealed class TransactionDetailsValidator : ITransactionDetailsValidator, ITransient
    {
        /// <inheritdoc />
        public Result<ITransactionDetails> Validate(
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
                return Result.Failure<ITransactionDetails>(descriptionResult.Error);
            }

            Currency currency = Currency.FromValue(currencyId).Value;

            if (!user.HasCurrency(currency))
            {
                return Result.Failure<ITransactionDetails>(DomainErrors.User.CurrencyDoesNotExist);
            }

            var money = new Money(amount, currency);

            TransactionType transactionType = TransactionType.FromValue(transactionTypeId).Value;

            Result transactionTypeResult = transactionType.ValidateAmount(money);

            if (transactionTypeResult.IsFailure)
            {
                return Result.Failure<ITransactionDetails>(transactionTypeResult.Error);
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
