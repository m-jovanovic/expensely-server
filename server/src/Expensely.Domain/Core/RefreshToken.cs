﻿using System;
using System.Collections.Generic;
using Expensely.Domain.Abstractions.Primitives;
using Expensely.Domain.Utility;

namespace Expensely.Domain.Core
{
    /// <summary>
    /// Represents the refresh token.
    /// </summary>
    public sealed class RefreshToken : ValueObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RefreshToken"/> class.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="token">The token value.</param>
        /// <param name="expiresOnUtc">The expires on date and time in UTC format.</param>
        public RefreshToken(User user, string token, DateTime expiresOnUtc)
        {
            Ensure.NotNull(user, "The user is required.", nameof(user));
            Ensure.NotEmpty(token, "The refresh token is required", nameof(token));
            Ensure.NotEmpty(expiresOnUtc, "The expires on date and time is required.", nameof(expiresOnUtc));

            UserId = user.Id;
            Token = token;
            ExpiresOnUtc = expiresOnUtc;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RefreshToken"/> class.
        /// </summary>
        /// <remarks>
        /// Required by EF Core.
        /// </remarks>
        private RefreshToken()
        {
        }

        /// <summary>
        /// Gets the user identifier.
        /// </summary>
        public Guid UserId { get; private set; }

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
        public bool Expired(DateTime utcNow) => ExpiresOnUtc < utcNow;

        /// <summary>
        /// Changes the values of the refresh token with the specified values.
        /// </summary>
        /// <param name="token">The token value.</param>
        /// <param name="expiresOnUtc">The expires on date and time in UTC format.</param>
        public void ChangeValues(string token, DateTime expiresOnUtc)
        {
            Ensure.NotEmpty(token, "The refresh token is required", nameof(token));
            Ensure.NotEmpty(expiresOnUtc, "The expires on date and time is required.", nameof(expiresOnUtc));

            Token = token;
            ExpiresOnUtc = expiresOnUtc;
        }

        /// <inheritdoc />
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return UserId;
            yield return Token;
            yield return ExpiresOnUtc;
        }
    }
}