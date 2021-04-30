namespace Expensely.Application.Contracts.Users
{
    /// <summary>
    /// Represents the user currency response.
    /// </summary>
    public sealed class UserCurrencyResponse
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

        /// <summary>
        /// Gets a value indicating whether or not the currency is the user's primary currency.
        /// </summary>
        public bool IsPrimary { get; init; }
    }
}
