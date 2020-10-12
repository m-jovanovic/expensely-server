using System.Collections.Generic;
using Expensely.Domain.Exceptions;
using Expensely.Domain.Primitives;
using Expensely.Domain.Utility;

namespace Expensely.Domain.Core
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
        /// Required by EF Core.
        /// </remarks>
        private Money() => Currency = Currency.None;

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
            AssertCurrenciesAreEqual(left, right);

            return new Money(left.Amount + right.Amount, left.Currency);
        }

        public static Money operator -(Money left, Money right)
        {
            AssertCurrenciesAreEqual(left, right);

            return new Money(left.Amount - right.Amount, left.Currency);
        }

        /// <inheritdoc />
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Amount;
            yield return Currency;
        }

        /// <summary>
        /// Asserts that the specified currencies are the same currency.
        /// </summary>
        /// <param name="left">The first currency.</param>
        /// <param name="right">The second currency.</param>
        private static void AssertCurrenciesAreEqual(Money left, Money right)
        {
            if (left.Currency != right.Currency)
            {
                throw new CurrenciesNotEqualDomainException(left.Currency, right.Currency);
            }
        }
    }
}
