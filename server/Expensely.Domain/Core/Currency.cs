using Expensely.Domain.Primitives;

namespace Expensely.Domain.Core
{
    /// <summary>
    /// Represents a currency with a currency name and code.
    /// </summary>
    public sealed class Currency : Enumeration<Currency>
    {
        /// <summary>
        /// The United States dollar.
        /// </summary>
        public static readonly Currency Usd = new Currency(1, "Dollar", "USD");

        /// <summary>
        /// The euro.
        /// </summary>
        public static readonly Currency Eur = new Currency(2, "Euro", "EUR");

        /// <summary>
        /// The Serbian dinar.
        /// </summary>
        public static readonly Currency Rsd = new Currency(3, "Serbian dinar", "RSD");

        /// <summary>
        /// The empty currency instance.
        /// </summary>
        internal static readonly Currency None = new Currency(default, string.Empty, string.Empty);

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
        /// Gets the currency code.
        /// </summary>
        public string Code { get; private set; }
    }
}
