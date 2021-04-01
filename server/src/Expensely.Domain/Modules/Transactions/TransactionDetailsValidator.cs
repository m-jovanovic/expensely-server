using Expensely.Common.Primitives.Result;
using Expensely.Common.Primitives.ServiceLifetimes;
using Expensely.Domain.Errors;
using Expensely.Domain.Modules.Common;
using Expensely.Domain.Modules.Transactions.Contracts;

namespace Expensely.Domain.Modules.Transactions
{
    /// <summary>
    /// Represents the transaction details validator.
    /// </summary>
    internal sealed class TransactionDetailsValidator : ITransactionDetailsValidator, ITransient
    {
        /// <inheritdoc />
        public Result<ITransactionDetails> Validate(ValidateTransactionDetailsRequest validateTransactionDetailsRequest)
        {
            Result<Description> descriptionResult = Description.Create(validateTransactionDetailsRequest.Description);

            if (descriptionResult.IsFailure)
            {
                return Result.Failure<ITransactionDetails>(descriptionResult.Error);
            }

            Currency currency = Currency.FromValue(validateTransactionDetailsRequest.Currency).Value;

            if (!validateTransactionDetailsRequest.User.HasCurrency(currency))
            {
                return Result.Failure<ITransactionDetails>(DomainErrors.User.CurrencyDoesNotExist);
            }

            var money = new Money(validateTransactionDetailsRequest.Amount, currency);

            TransactionType transactionType = TransactionType.FromValue(validateTransactionDetailsRequest.TransactionType).Value;

            Result transactionTypeResult = transactionType.ValidateAmount(money);

            if (transactionTypeResult.IsFailure)
            {
                return Result.Failure<ITransactionDetails>(transactionTypeResult.Error);
            }

            return new TransactionDetails
            {
                Description = descriptionResult.Value,
                Category = Category.FromValue(validateTransactionDetailsRequest.Category).Value,
                Money = money,
                OccurredOn = validateTransactionDetailsRequest.OccurredOn,
                TransactionType = transactionType
            };
        }
    }
}
