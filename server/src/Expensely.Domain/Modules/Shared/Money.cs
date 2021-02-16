using System.Collections.Generic;
using Expensely.Domain.Abstractions.Primitives;
using Expensely.Domain.Exceptions;
using Expensely.Domain.Utility;

namespace Expensely.Domain.Modules.Shared
{
    /// <summary>
    /// Represents a monetary amount for a specific currency.
    /// </summary>
    public sealed class Money : ValueObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Money"/> class.
        /// </summary>
        /// <param name="amount">The monetary amount.</param>
        /// <param name="currency">The currency.</param>
        public Money(decimal amount, Currency currency)
        {
            Ensure.NotEmpty(currency, "The currency is required.", nameof(currency));

            Amount = amount;
            Currency = currency;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Money"/> class.
        /// </summary>
        /// <remarks>
        /// Required for deserialization.
        /// </remarks>
        private Money()
        {
        }

        /// <summary>
        /// Gets the monetary amount.
        /// </summary>
        public decimal Amount { get; private set; }

        /// <summary>
        /// Gets the currency.
        /// </summary>
        public Currency Currency { get; private set; }

        public static Money operator +(Money left, Money right)
        {
            EnsureCurrenciesAreEqual(left, right);

            return new Money(left.Amount + right.Amount, left.Currency);
        }

        public static Money operator -(Money left, Money right)
        {
            EnsureCurrenciesAreEqual(left, right);

            return new Money(left.Amount - right.Amount, left.Currency);
        }

        /// <summary>
        /// Formats the amount with the currency.
        /// </summary>
        /// <returns>The formatted amount along with the currency.</returns>
        public string Format() => Currency.Format(Amount);

        /// <inheritdoc />
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Amount;
            yield return Currency;
        }

        /// <summary>
        /// Ensures that the specified currencies are the same currency, otherwise throws an exception.
        /// </summary>
        /// <param name="left">The first currency.</param>
        /// <param name="right">The second currency.</param>
        /// <exception cref="CurrenciesNotEqualDomainException"> when the specified currencies are not the same.</exception>
        private static void EnsureCurrenciesAreEqual(Money left, Money right)
        {
            if (left.Currency != right.Currency)
            {
                throw new CurrenciesNotEqualDomainException(left.Currency, right.Currency);
            }
        }
    }
}
