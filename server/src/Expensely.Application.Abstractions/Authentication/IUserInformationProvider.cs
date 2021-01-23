using Expensely.Domain.Abstractions.Maybe;
using Expensely.Domain.Core;

namespace Expensely.Application.Abstractions.Authentication
{
    /// <summary>
    /// Represents the user information provider interface.
    /// </summary>
    public interface IUserInformationProvider
    {
        /// <summary>
        /// Gets a value indicating whether or not the the current user is authenticated.
        /// </summary>
        bool IsAuthenticated { get; }

        /// <summary>
        /// Gets the user identifier of the currently authenticated user.
        /// </summary>
        string UserId { get; }

        /// <summary>
        /// Gets the maybe instance that may contain the primary currency of the currently authenticated user.
        /// </summary>
        Maybe<Currency> PrimaryCurrency { get; }
    }
}
