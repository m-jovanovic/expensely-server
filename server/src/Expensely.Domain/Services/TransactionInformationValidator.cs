using System;
using Expensely.Domain.Abstractions.Result;
using Expensely.Domain.Contracts;
using Expensely.Domain.Core;
using Expensely.Domain.Errors;

namespace Expensely.Domain.Services
{
    /// <summary>
    /// Represents the transaction information validator.
    /// </summary>
    public sealed class TransactionInformationValidator
    {
        /// <summary>
        /// Validates the provided transaction information and returns the result of the validation.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="name">The name.</param>
        /// <param name="description">The description.</param>
        /// <param name="categoryValue">The category value.</param>
        /// <param name="amount">The amount.</param>
        /// <param name="currencyValue">The currency value.</param>
        /// <param name="occurredOn">The occurred on date.</param>
        /// <returns>The result of the transaction validation process containing the transaction information or an error.</returns>
        public Result<TransactionInformation> Validate(
            User user,
            string name,
            string description,
            int categoryValue,
            decimal amount,
            int currencyValue,
            DateTime occurredOn)
        {
            Result<Name> nameResult = Name.Create(name);

            Result<Description> descriptionResult = Description.Create(description);

            var firstFailureOrSuccess = Result.FirstFailureOrSuccess(nameResult, descriptionResult);

            if (firstFailureOrSuccess.IsFailure)
            {
                return Result.Failure<TransactionInformation>(firstFailureOrSuccess.Error);
            }

            Currency currency = Currency.FromValue(currencyValue).Value;

            if (!user.HasCurrency(currency))
            {
                return Result.Failure<TransactionInformation>(DomainErrors.User.CurrencyDoesNotExist);
            }

            return new TransactionInformation
            {
                UserId = user.Id,
                Name = nameResult.Value,
                Description = descriptionResult.Value,
                Category = Category.FromValue(categoryValue).Value,
                Money = new Money(amount, currency),
                OccurredOn = occurredOn
            };
        }
    }
}
