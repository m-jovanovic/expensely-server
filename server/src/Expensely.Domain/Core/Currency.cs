using Expensely.Domain.Abstractions.Primitives;

namespace Expensely.Domain.Core
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
        /// The euro.
        /// </summary>
        public static readonly Currency Eur = new(2, "Euro", "EUR");

        /// <summary>
        /// The Serbian Dinar.
        /// </summary>
        public static readonly Currency Rsd = new(3, "Serbian Dinar", "RSD");

        /// <summary>
        /// The empty currency instance.
        /// </summary>
        internal static readonly Currency None = new(default, string.Empty, string.Empty);

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
        /// <param name="value">The currency value.</param>
        /// <remarks>
        /// Required by EF Core.
        /// </remarks>
        private Currency(int value)
            : base(value, ContainsValue(value) ? FromValue(value).Value.Name : None.Name) =>
            Code = ContainsValue(value) ? FromValue(value).Value.Code : None.Code;

        /// <summary>
        /// Gets the currency code.
        /// </summary>
        public string Code { get; private set; }

        /// <summary>
        /// Formats the specified amount.
        /// </summary>
        /// <param name="amount">The amount to be formatted.</param>
        /// <returns>The formatted amount along with the currency.</returns>
        public string Format(decimal amount) => $"{amount:n2} {Code}";
    }
}
