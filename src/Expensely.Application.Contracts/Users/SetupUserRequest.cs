namespace Expensely.Application.Contracts.Users
{
    /// <summary>
    /// Represents the setup user request.
    /// </summary>
    public sealed class SetupUserRequest
    {
        /// <summary>
        /// Gets the currency value.
        /// </summary>
        public int Currency { get; init; }

        /// <summary>
        /// Gets the time zone identifier.
        /// </summary>
        public string TimeZoneId { get; init; }
    }
}
