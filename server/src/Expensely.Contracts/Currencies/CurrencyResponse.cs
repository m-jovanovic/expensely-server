namespace Expensely.Contracts.Currencies
{
    /// <summary>
    /// Represents the currency response.
    /// </summary>
    public sealed class CurrencyResponse
    {
        /// <summary>
        /// Gets the currency value.
        /// </summary>
        public int Value { get; init; }

        /// <summary>
        /// Gets the currency name.
        /// </summary>
        public string Name { get; init; }

        /// <summary>
        /// Gets the currency code.
        /// </summary>
        public string Code { get; init; }
    }
}
