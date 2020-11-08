using System;
using System.Security.Claims;
using Expensely.Application.Abstractions.Authentication;
using Expensely.Domain.Core;
using Expensely.Domain.Primitives.Maybe;
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
        public UserInformationProvider(IHttpContextAccessor httpContextAccessor)
        {
            UserId = new Guid(
                FindClaim(httpContextAccessor, "userId") ??
                throw new ArgumentException("The user identifier claim is required.", nameof(httpContextAccessor)));

            PrimaryCurrency = int.TryParse(FindClaim(httpContextAccessor, "primaryCurrency"), out int value) ?
                Currency.FromValue(value) : Maybe<Currency>.None;
        }

        /// <inheritdoc />
        public Guid UserId { get; }

        /// <inheritdoc />
        public Maybe<Currency> PrimaryCurrency { get; }

        /// <summary>
        /// Finds the claim with the specified name if it exists.
        /// </summary>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        /// <param name="claim">The claim name.</param>
        /// <returns>The first value of the claim with the specified name, otherwise null.</returns>
        private static string FindClaim(IHttpContextAccessor httpContextAccessor, string claim) =>
            httpContextAccessor.HttpContext?.User?.FindFirstValue(claim);
    }
}
