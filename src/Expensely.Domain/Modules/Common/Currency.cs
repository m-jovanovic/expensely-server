using System;
using System.Globalization;
using Expensely.Domain.Primitives;

namespace Expensely.Domain.Modules.Common
{
    /// <summary>
    /// Represents a currency with a currency name and code.
    /// </summary>
    public sealed class Currency : Enumeration<Currency>
    {
        /// <summary>
        /// The United States Dollar.
        /// </summary>
        public static readonly Currency Usd = new(1, "US Dollar", "USD");

        /// <summary>
        /// The Euro.
        /// </summary>
        public static readonly Currency Eur = new(2, "Euro", "EUR");

        /// <summary>
        /// The Serbian Dinar.
        /// </summary>
        public static readonly Currency Rsd = new(3, "Serbian Dinar", "RSD");

        private static readonly IFormatProvider NumberFormat = new CultureInfo("sr-RS");

        /// <summary>
        /// Initializes a new instance of the <see cref="Currency"/> class.
        /// </summary>
        /// <param name="value">The currency value.</param>
        /// <param name="name">The currency name.</param>
        /// <param name="code">The currency code.</param>
        private Currency(int value, string name, string code)
            : base(value, name) =>
            Code = code;

        /// <summary>
        /// Initializes a new instance of the <see cref="Currency"/> class.
        /// </summary>
        /// <remarks>
        /// Required for deserialization.
        /// </remarks>
        private Currency()
        {
        }

        /// <summary>
        /// Gets the currency code.
        /// </summary>
        public string Code { get; private set; }

        /// <summary>
        /// Formats the specified amount.
        /// </summary>
        /// <param name="amount">The amount to be formatted.</param>
        /// <returns>The formatted amount along with the currency.</returns>
        public string Format(decimal amount) => $"{amount.ToString("N2", NumberFormat)} {Code}";
    }
}
