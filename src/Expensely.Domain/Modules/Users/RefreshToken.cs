using System;
using System.Collections.Generic;
using Expensely.Domain.Primitives;
using Expensely.Domain.Utility;

namespace Expensely.Domain.Modules.Users
{
    /// <summary>
    /// Represents the refresh token.
    /// </summary>
    public sealed class RefreshToken : ValueObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RefreshToken"/> class.
        /// </summary>
        /// <param name="token">The token value.</param>
        /// <param name="expiresOnUtc">The expires on date and time in UTC format.</param>
        public RefreshToken(string token, DateTime expiresOnUtc)
            : this()
        {
            Ensure.NotEmpty(token, "The refresh token is required.", nameof(token));
            Ensure.NotEmpty(expiresOnUtc, "The expires on date and time is required.", nameof(expiresOnUtc));

            Token = token;
            ExpiresOnUtc = expiresOnUtc;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RefreshToken"/> class.
        /// </summary>
        /// <remarks>
        /// Required for deserialization.
        /// </remarks>
        private RefreshToken()
        {
        }

        /// <summary>
        /// Gets the refresh token value.
        /// </summary>
        public string Token { get; private set; }

        /// <summary>
        /// Gets the expires on date and time in UTC format.
        /// </summary>
        public DateTime ExpiresOnUtc { get; private set; }

        /// <summary>
        /// Checks if the refresh token has expired.
        /// </summary>
        /// <param name="utcNow">The current date and time in UTC format.</param>
        /// <returns>True if the refresh token has expired, otherwise false.</returns>
        public bool IsExpired(DateTime utcNow) => ExpiresOnUtc < utcNow;

        /// <inheritdoc />
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Token;
            yield return ExpiresOnUtc;
        }
    }
}
