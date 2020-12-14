using System;
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
        /// Gets the user identifier of the currently authenticated user.
        /// </summary>
        Guid UserId { get; }

        /// <summary>
        /// Gets the maybe instance that may contain the primary currency of the currently authenticated user.
        /// </summary>
        Maybe<Currency> PrimaryCurrency { get; }
    }
}
