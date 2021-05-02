using System;
using System.Collections.Generic;
using System.Linq;
using Expensely.Domain.Modules.Common.Exceptions;
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

        public static Money operator +(Money first, Money second)
        {
            EnsureCurrenciesMatch(first.Currency, second.Currency);

            return new Money(first.Amount + second.Amount, first.Currency);
        }

        public static Money operator -(Money first, Money second)
        {
            EnsureCurrenciesMatch(first.Currency, second.Currency);

            return new Money(first.Amount - second.Amount, first.Currency);
        }

        /// <summary>
        /// Creates a new <see cref="Money"/> instance with the specified currency and zero amount.
        /// </summary>
        /// <param name="currency">The currency.</param>
        /// <returns>The new <see cref="Money"/> instance with the specified currency and zero amount.</returns>
        public static Money Zero(Currency currency) => new(default, currency);

        /// <summary>
        /// Sums the specified array of <see cref="Money"/> objects.
        /// </summary>
        /// <param name="moneyArray">The array of <see cref="Money"/> objects.</param>
        /// <returns>The resulting <see cref="Money"/> instance representing the sum.</returns>
        public static Money Sum(Money[] moneyArray)
        {
            if (moneyArray is null || !moneyArray.Any())
            {
                throw new ArgumentException("The array of money amounts is null or empty.", nameof(moneyArray));
            }

            Money sum = moneyArray[0];

            foreach (Money money in moneyArray.Skip(1))
            {
                EnsureCurrenciesMatch(sum.Currency, money.Currency);

                sum += money;
            }

            return sum;
        }

        /// <summary>
        /// Formats the amount with the currency.
        /// </summary>
        /// <returns>The formatted amount along with the currency.</returns>
        public string Format() => Currency.Format(Amount);

        /// <summary>
        /// Calculates the percentage of the specified money amount from the current amount.
        /// </summary>
        /// <param name="money">The money amount.</param>
        /// <returns>The percentage of the specified money amount from the current amount.</returns>
        public decimal PercentFrom(Money money)
        {
            EnsureCurrenciesMatch(Currency, money.Currency);

            return Math.Abs(money.Amount / Amount);
        }

        /// <inheritdoc />
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Amount;
            yield return Currency;
        }

        private static void EnsureCurrenciesMatch(Currency currency1, Currency currency2)
        {
            if (currency1 != currency2)
            {
                throw new CurrenciesDoNotMatchDomainException(currency1, currency2);
            }
        }
    }
}
