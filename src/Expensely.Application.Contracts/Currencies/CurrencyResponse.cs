namespace Expensely.Application.Contracts.Currencies
{
    /// <summary>
    /// Represents the currency response.
    /// </summary>
    public sealed class CurrencyResponse
    {
        /// <summary>
        /// Gets the currency identifier.
        /// </summary>
        public int Id { get; init; }

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
