namespace Expensely.Application.Contracts.Currencies
{
    /// <summary>
    /// Represents the currency response.
    /// </summary>
    public sealed class CurrencyResponse
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CurrencyResponse"/> class.
        /// </summary>
        /// <param name="value">The currency value.</param>
        /// <param name="name">The currency name.</param>
        /// <param name="code">The currency code.</param>
        public CurrencyResponse(int value, string name, string code)
        {
            Value = value;
            Name = name;
            Code = code;
        }

        /// <summary>
        /// Gets the currency value.
        /// </summary>
        public int Value { get; }

        /// <summary>
        /// Gets the currency name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the currency code.
        /// </summary>
        public string Code { get; }
    }
}
