using System;
using System.Security.Claims;
using Expensely.Application.Abstractions.Authentication;
using Microsoft.AspNetCore.Http;

namespace Expensely.Infrastructure.Authentication
{
    /// <summary>
    /// Represents the user identifier provider.
    /// </summary>
    internal sealed class UserIdentifierProvider : IUserIdentifierProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserIdentifierProvider"/> class.
        /// </summary>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        public UserIdentifierProvider(IHttpContextAccessor httpContextAccessor) =>
            UserId = new Guid(httpContextAccessor.HttpContext?.User?.FindFirstValue("userId") ??
                              throw new ArgumentException("The user identifier claim is required.", nameof(httpContextAccessor)));

        /// <inheritdoc />
        public Guid UserId { get; }
    }
}
