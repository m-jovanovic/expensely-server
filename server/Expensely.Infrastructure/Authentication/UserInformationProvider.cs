using System;
using Expensely.Application.Abstractions.Authentication;
using Expensely.Infrastructure.Extensions;
using Microsoft.AspNetCore.Http;

namespace Expensely.Infrastructure.Authentication
{
    /// <summary>
    /// Represents the user identifier provider.
    /// </summary>
    internal sealed class UserInformationProvider : IUserInformationProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserInformationProvider"/> class.
        /// </summary>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        public UserInformationProvider(IHttpContextAccessor httpContextAccessor) =>
            UserId = new Guid(
                httpContextAccessor.HttpContext?.User.GetUserId() ??
                throw new ArgumentException("The user identifier claim is required.", nameof(httpContextAccessor)));

        /// <inheritdoc />
        public Guid UserId { get; }
    }
}
