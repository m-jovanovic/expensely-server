namespace Expensely.Application.Queries.Handlers.Users.GetUserCurrencies
{
    /// <summary>
    /// Represents the user currency model.
    /// </summary>
    public sealed record UserCurrencyModel
    {
        /// <summary>
        /// Gets the value.
        /// </summary>
        public int Value { get; init; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        public string Name { get; init; }

        /// <summary>
        /// Gets the code.
        /// </summary>
        public string Code { get; init; }

        /// <summary>
        /// Gets a value indicating whether the currency is primary or not.
        /// </summary>
        public bool IsPrimary { get; init; }
    }
}
