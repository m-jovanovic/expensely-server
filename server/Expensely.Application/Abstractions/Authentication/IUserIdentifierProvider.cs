using System;

namespace Expensely.Application.Abstractions.Authentication
{
    /// <summary>
    /// Represents the user identifier provider interface.
    /// </summary>
    public interface IUserIdentifierProvider
    {
        /// <summary>
        /// Gets the user identifier of the currently authenticated user.
        /// </summary>
        Guid UserId { get; }
    }
}
