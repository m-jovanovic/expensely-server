using System;

namespace Expensely.Application.Abstractions.Authentication
{
    /// <summary>
    /// Represents the user information provider interface.
    /// </summary>
    public interface IUserInformationProvider
    {
        /// <summary>
        /// Gets the user identifier of the currently authenticated user.
        /// </summary>
        Guid UserId { get; }
    }
}
