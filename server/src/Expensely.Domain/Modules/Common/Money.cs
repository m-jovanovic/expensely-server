using System.Collections.Generic;
using Expensely.Domain.Primitives;
using Expensely.Domain.Utility;

namespace Expensely.Domain.Modules.Common
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
            : this()
        {
            Ensure.NotNull(currency, "The currency is required.", nameof(currency));

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
    }
}
