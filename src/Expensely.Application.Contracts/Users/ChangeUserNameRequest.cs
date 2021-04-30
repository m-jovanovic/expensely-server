namespace Expensely.Application.Contracts.Users
{
    /// <summary>
    /// Represents the change user name request.
    /// </summary>
    public sealed class ChangeUserNameRequest
    {
        /// <summary>
        /// Gets the first name.
        /// </summary>
        public string FirstName { get; init; }

        /// <summary>
        /// Gets the last name.
        /// </summary>
        public string LastName { get; init; }
    }
}
