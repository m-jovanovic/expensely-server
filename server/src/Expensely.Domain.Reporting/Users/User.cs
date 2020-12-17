using System;

namespace Expensely.Domain.Reporting.Users
{
    /// <summary>
    /// Represents the user entity.
    /// </summary>
    public sealed class User
    {
        /// <summary>
        /// Gets the identifier.
        /// </summary>
        public Guid Id { get; init; }
    }
}
