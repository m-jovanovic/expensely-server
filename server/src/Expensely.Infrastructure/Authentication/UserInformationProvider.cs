using Expensely.Application.Abstractions.Authentication;
using Expensely.Common.Abstractions.ServiceLifetimes;
using Expensely.Common.Primitives.Maybe;
using Expensely.Domain.Modules.Shared;
using Expensely.Infrastructure.Extensions;
using Microsoft.AspNetCore.Http;

namespace Expensely.Infrastructure.Authentication
{
    /// <summary>
    /// Represents the user identifier provider.
    /// </summary>
    public sealed class UserInformationProvider : IUserInformationProvider, IScoped
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserInformationProvider"/> class.
        /// </summary>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        public UserInformationProvider(IHttpContextAccessor httpContextAccessor)
        {
            string userId = httpContextAccessor.HttpContext?.User.GetUserId();

            IsAuthenticated = !string.IsNullOrWhiteSpace(userId);

            UserId = userId;

            PrimaryCurrency = CreatePrimaryCurrency(httpContextAccessor);
        }

        /// <inheritdoc />
        public bool IsAuthenticated { get; }

        /// <inheritdoc />
        public string UserId { get; }

        /// <inheritdoc />
        public Maybe<Currency> PrimaryCurrency { get; }

        private static Maybe<Currency> CreatePrimaryCurrency(IHttpContextAccessor httpContextAccessor)
        {
            string primaryCurrencyString = httpContextAccessor.HttpContext?.User.GetPrimaryCurrency() ?? string.Empty;

            bool parsed = int.TryParse(primaryCurrencyString, out int primaryCurrency);

            if (!parsed || !Currency.ContainsValue(primaryCurrency))
            {
                return Maybe<Currency>.None;
            }

            return Currency.FromValue(primaryCurrency);
        }
    }
}
