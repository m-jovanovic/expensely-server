using System;
using Expensely.Application.Abstractions.Authentication;
using Expensely.Domain.Core;
using Expensely.Domain.Primitives.Maybe;
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
        public UserInformationProvider(IHttpContextAccessor httpContextAccessor)
        {
            UserId = CreateUserId(httpContextAccessor);
            PrimaryCurrency = CreatePrimaryCurrency(httpContextAccessor);
        }

        /// <inheritdoc />
        public Guid UserId { get; }

        /// <inheritdoc />
        public Maybe<Currency> PrimaryCurrency { get; }

        /// <summary>
        /// Creates the user identifier value.
        /// </summary>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        /// <returns>The user identifier value.</returns>
        /// <exception cref="ArgumentException"> if the user identifier claim is not found.</exception>
        private static Guid CreateUserId(IHttpContextAccessor httpContextAccessor) =>
            new Guid(
                httpContextAccessor.HttpContext?.User.GetUserId() ??
                throw new ArgumentException("The user identifier claim is required.", nameof(httpContextAccessor)));

        /// <summary>
        /// Crates the primary currency, if it exists.
        /// </summary>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        /// <returns>The maybe instance that may contain the primary currency.</returns>
        private static Maybe<Currency> CreatePrimaryCurrency(IHttpContextAccessor httpContextAccessor) =>
            int.TryParse(
                httpContextAccessor.HttpContext?.User.GetPrimaryCurrency() ?? string.Empty,
                out int primaryCurrency) &&
            Currency.ContainsValue(primaryCurrency)
                ? Currency.FromValue(primaryCurrency)
                : Maybe<Currency>.None;
    }
}
