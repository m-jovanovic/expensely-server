using Expensely.Domain.Primitives;

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
        public static readonly Currency Usd = new Currency(1, "US Dollar", "USD");

        /// <summary>
        /// The euro.
        /// </summary>
        public static readonly Currency Eur = new Currency(2, "Euro", "EUR");

        /// <summary>
        /// The Serbian Dinar.
        /// </summary>
        public static readonly Currency Rsd = new Currency(3, "Serbian Dinar", "RSD");

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
        /// Initializes a new instance of the <see cref="Currency"/> class.
        /// </summary>
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
    }
}
